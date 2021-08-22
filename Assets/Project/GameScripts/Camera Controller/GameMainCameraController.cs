using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameMainCameraController : MonoBehaviour
{
    [SerializeField] Transform m_target;
    [SerializeField] Vector3 m_targetOffset = Vector3.zero;
    [SerializeField, Min(0)] float m_targetToDistance = 10;
    [SerializeField] float m_rotateSpeedX = 100;
    [SerializeField] float m_rotateSpeedY = 200;
    [SerializeField] Vector2 m_currentLocalAngles = Vector2.zero;
    [SerializeField] bool m_mirrorX = false;
    [SerializeField] bool m_mirrorY = false;
    [SerializeField] float m_maxAngleX = 75;
    [SerializeField] float m_minAngleX = -75;

    [SerializeField] bool m_clamping = true;
    [SerializeField, Min(0)] float m_clampValue = 20;

    [SerializeField] bool m_avoidingCollisions = true;
    [SerializeField,Min(0)] float m_cameraCollisionScale = 0.1f;
    [SerializeField] string[] m_avoidCollisionTags = null;


    private InputAction CameraMove;

    #region Simple Propertys

    public Transform Target { get => m_target; set => m_target = value; }
    public Vector3 TargetOffset { get => m_targetOffset; set => m_targetOffset = value; }
    public float TargetToDistance { get => m_targetToDistance; set => m_targetToDistance = value; }
    public float RotateSpeedX { get => m_rotateSpeedX; set => m_rotateSpeedX = value; }
    public float RotateSpeedY { get => m_rotateSpeedY; set => m_rotateSpeedY = value; }
    public Vector2 CurrentAngles { get => m_currentLocalAngles; set => m_currentLocalAngles = value; }
    public bool MirrorX { get => m_mirrorX; set => m_mirrorX = value; }
    public bool MirrorY { get => m_mirrorY; set => m_mirrorY = value; }
    public float MaxAngleX { get => m_maxAngleX; set => m_maxAngleX = value; }
    public float MinAngleX { get => m_minAngleX; set => m_minAngleX = value; }
    public bool Clamping { get => m_clamping; set => m_clamping = value; }
    public float ClampValue { get => m_clampValue; set => m_clampValue = value; }
    public bool AvoidingCollisions { get => m_avoidingCollisions; set => m_avoidingCollisions = value; }
    public float CameraCollisionScale { get => m_cameraCollisionScale; set => m_cameraCollisionScale = value; }
    public string[] AvoidCollisionTags { get => m_avoidCollisionTags; set => m_avoidCollisionTags = value; }

    #endregion

    #region Operator Propertys

    public float CurrentTargetDistance
    {
        get
        {
            var targetPosition = m_target.position + m_targetOffset;
            var currentPosition = transform.position;

            var distance = targetPosition - currentPosition;

            var result = distance.magnitude;

            return result;
        }
        set
        {
            var targetPosition = m_target.position + m_targetOffset;
            var currentPosition = transform.position;

            var distance = targetPosition - currentPosition;

            var angle = distance.normalized;

            var result = angle * value;

            transform.position = result;
        }
    }

    #endregion


    void Start()
    {
        Init();

        if (m_target == null)
            return;

        ChangeAngles(Vector2.zero);
        ChangePosition();
    }

    private void Init()
    {
        InitInputMap();


        void InitInputMap()
        {
            var playerInput = GetComponent<PlayerInput>();
            var actions = playerInput.actions;

            CameraMove = actions["CameraMove"];
        }
    }

    void Update()
    {
        CameraOperator();
    }

    private void CameraOperator()
    {
        if (m_target == null)
            return;

        var input = GetInputStick();

        if (input.inputing)
        {
            ChangeAngles(input.inputValue);
        }

        ChangePosition();
    }

    private (bool inputing,Vector2 inputValue) GetInputStick()
    {
        var value = CameraMove.ReadValue<Vector2>();

        (bool inputing, Vector2 inputValue) result =
            (
            inputing :(value.sqrMagnitude > 0),
            inputValue : value
            );

        return result;
    }

    private void ChangeAngles(Vector2 inputValue)
    {
        ChangeParametor(inputValue);
        SetRotation();

        #region local methods

        void ChangeParametor(Vector2 inputValue)
        {
            var resultAngles = m_currentLocalAngles;
            var deltaInput = inputValue * Time.deltaTime;

            var rotateSpeedX = m_mirrorX ? m_rotateSpeedX : -m_rotateSpeedX;
            var rotateSpeedY = m_mirrorY ? -m_rotateSpeedY : m_rotateSpeedY;

            resultAngles.x += deltaInput.y * rotateSpeedX;
            resultAngles.y += deltaInput.x * rotateSpeedY;

            if (resultAngles.x > m_maxAngleX)
            {
                resultAngles.x = m_maxAngleX;
            }
            else if(resultAngles.x < m_minAngleX)
            {
                resultAngles.x = m_minAngleX;
            }

            const float MaxEuler = 360;
            if(resultAngles.y > MaxEuler)
            {
                resultAngles.y -= MaxEuler;
            }
            else if(resultAngles.y < 0)
            {
                resultAngles.y += MaxEuler;
            }

            m_currentLocalAngles = resultAngles;

        }
        void SetRotation()
        {
            var SettingAngle = new Vector3
                (
                    x : m_currentLocalAngles.x,
                    y : m_currentLocalAngles.y,
                    z : 0
                );

            transform.eulerAngles = SettingAngle;
        }

        #endregion
    }

    private void ChangePosition()
    {
        var nextPosition = m_avoidingCollisions ? GetAvoidNextPosition() : GetNextPosition();

        
        if (m_clamping)
        {
            nextPosition = GetClampDistancePosition(nextPosition);
        }

        SetPosition(nextPosition);

        #region local methods

        Vector3 GetNextPosition()
        {
            var back = (transform.forward * -1);
            var targetDistance = back * m_targetToDistance;
            var targetPosition = m_target.transform.position + m_targetOffset;

            var result = targetPosition + targetDistance;

            return result;
        }

        Vector3 GetAvoidNextPosition()
        {
            if (m_avoidCollisionTags == null)
            {
                return GetNextPosition();
            }
            var avoidCollisionsLength = m_avoidCollisionTags.Length;

            var back = (transform.forward * -1);
            var right = transform.right;
            var up = transform.up;

            var targetPosition = m_target.transform.position + m_targetOffset;
            var targetToDistance = m_targetToDistance;

            var ray = new Ray(
                origin: Vector3.zero,
                direction: back
                );
            var rayDistance = m_targetToDistance + m_cameraCollisionScale;

            for (int i = -1;i <= 1; i++)
            {
                var rayWidthOffset = right * i * m_cameraCollisionScale;
                var rayWidthPosition = targetPosition + rayWidthOffset;

                for (int j = -1; j <= 1; j++)
                {
                    var rayHeightOffset = up * j * m_cameraCollisionScale;
                    var rayPosition = rayWidthPosition + rayHeightOffset;
                    ray.origin = rayPosition;

                    var hits = Physics.RaycastAll(ray, rayDistance);
                    if(hits == null)
                    {
                        continue;
                    }

                    var hitsLenght = hits.Length;
                    for(int k = 0;k < hitsLenght; k++)
                    {
                        var hit = hits[k];
                        for (int l = 0; l < avoidCollisionsLength; l++)
                        {
                            var tag = m_avoidCollisionTags[l];
                            if (hit.collider.CompareTag(tag))
                            {
                                var distance = hit.distance - m_cameraCollisionScale;
                                if (targetToDistance > distance)
                                {
                                    targetToDistance = distance;
                                }
                            }
                        }
                    }
                }
            }

            var targetDistance = back * targetToDistance;
            

            var result = targetPosition + targetDistance;

            return result;
        }

        Vector3 GetClampDistancePosition(Vector3 nextPosition)
        {
            var targetPosition = m_target.transform.position + m_targetOffset;
            var currentPosition = transform.position;

            var nextDistance = nextPosition - targetPosition;
            var currentDistance = currentPosition - targetPosition;

            var nextDistanceLenght = nextDistance.magnitude;
            var currentDistanceLenght = currentDistance.magnitude;

            var distanceDistanceLenght = Mathf.Abs(nextDistanceLenght - currentDistanceLenght);

            var result = nextPosition;
            var clampValueDelta = m_clampValue * Time.deltaTime;

            if (distanceDistanceLenght > clampValueDelta)
            {
                var normalizeNextDistance = nextDistance.normalized;

                result -= normalizeNextDistance * (distanceDistanceLenght - clampValueDelta)
                     * ((nextDistanceLenght - currentDistanceLenght) > 0 ? 1 :-1);
            }
            return result;
        }

        void SetPosition(Vector3 nextPosition)
        {
            var result = nextPosition;

            transform.position = result;
        }

        #endregion
    }

}
