using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameMainCameraController : MonoBehaviour
{
    [SerializeField] Transform m_target;
    [SerializeField] Vector3 m_targetOffset = Vector3.zero;
    [SerializeField, Min(0)] float m_lenght = 10;
    [SerializeField] float m_rotateSpeedX = 100;
    [SerializeField] float m_rotateSpeedY = 200;
    [SerializeField] Vector2 m_currentAngles = Vector2.zero;
    [SerializeField] bool m_mirrorX = false;
    [SerializeField] bool m_mirrorY = false;
    [SerializeField] float m_maxAngleX = 75;
    [SerializeField] float m_minAngleX = -75;


    private InputAction CameraMove;


    // Start is called before the first frame update
    void Start()
    {
        Init();
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

    // Update is called once per frame
    void Update()
    {
        CameraOperator();
    }

    private void CameraOperator()
    {
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
            var resultAngles = m_currentAngles;
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

            m_currentAngles = resultAngles;

        }
        void SetRotation()
        {
            var SettingAngle = new Vector3
                (
                    x : m_currentAngles.x,
                    y : m_currentAngles.y,
                    z : 0
                );

            transform.eulerAngles = SettingAngle;
        }

        #endregion
    }

    private void ChangePosition()
    {
        var nextPosition = GetNextPosition();
        SetPosition(nextPosition);

        #region local methods

        Vector3 GetNextPosition()
        {
            var back = (transform.forward * -1);
            var targetDistanse = back * m_lenght;
            var targetPosition = m_target.transform.position + m_targetOffset;

            var result = targetPosition + targetDistanse;

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
