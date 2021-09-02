using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
[RequireComponent(typeof(CharacterController))]
[AddComponentMenu(menuName: "MyComponet/MyRigidbody")]
public class MyRigidbody : MonoBehaviour
{
    private Transform m_transform;

    [SerializeField] Vector3 m_groundPointOffset = Vector3.zero;
    [SerializeField] float m_collisionScale = 5;
    [SerializeField,Min(0)] float m_groundLower = 0.1f;
    [SerializeField] string[] m_groundTags = null;
    [SerializeField,Range(0,85)] float m_groundAngle = 0;
    [SerializeField] Vector3 m_velocity = Vector3.zero;
    private float m_rayDistance = 0;

    public float CollisionScale
    {
        get => m_collisionScale;
        set
        {
            if(value < 0)
            {
                value = 0;
            }


            m_collisionScale = value;
            SetRayDistance();
        }
    }

    public float GroundAngle
    {
        get => m_groundAngle;
        set
        {
            if(value < 0)
            {
                value = 0;
            }
            else if(value > 85)
            {
                value = 85;
            }

            m_groundAngle = value;
            SetRayDistance();
        }
    }

    private void SetRayDistance()
    {
        var result = 0f;

        var collisionScale = m_collisionScale;
        var groundAngle = m_groundAngle;


        var collisionDiameter = collisionScale * 2;

        // h= a * tan(Angle)

        var rad = groundAngle * Mathf.Deg2Rad;
        var tan = Mathf.Tan(rad);

        result = collisionDiameter * tan;

        m_rayDistance = result;
    }

    public Vector3 GroundPoint
        => m_transform.position + m_groundPointOffset;

    private CharacterController m_characterController;

    public CharacterController CharacterController { get => m_characterController; }
    public Vector3 Velocity { get => m_velocity; set => m_velocity = value; }
    public bool UseGravity { get; set; } = true;
    public bool IsKinematic { get; set; } = false;
    public bool GroundedMode { get; set; } = false;

    [SerializeField] MyRigidbodyScaleScriptableObject m_myRigidbodyScale = null;

    private I_MyRigidbody I_MyRigidbody = null;

    private void Awake()
    {
        m_transform = transform;
        m_characterController = GetComponent<CharacterController>();
        TryGetComponent(out this.I_MyRigidbody);
        IsKinematic = false;
        SetRayDistance();
    }
    private void LateUpdate()
    {
       // Move(MathFree.TimeMix);
    }
    public void Move(float timeScale)
    {
        Vector3 velocity = this.m_velocity;

        if (m_characterController == null)
        {
            Debug.LogError(transform.gameObject.name + " Is Can Not Find CharacterController");
            return;
        }
        if (IsKinematic)
        {
            return;
        }
        if (Time.timeScale == 0)
        {
            return;
        }

        velocity = OnGravity(velocity);
        
        m_characterController.Move(velocity * timeScale);

        var isGround = IsGround(out var _);

        velocity = OnSimpleFriction(velocity, isGround);

        this.m_velocity = velocity;

        #region local methods

        Vector3 OnGravity(Vector3 force)
        {
            MyRigidbodyScale myRigidbodyScale = m_myRigidbodyScale != null ?
                m_myRigidbodyScale.RigidbodyScale : MyRigidbodyScale.Default();

            var result = force;

            if (UseGravity)
            {
                result.y += myRigidbodyScale.GravityScale * timeScale;
            }

            return result;
        }

        Vector3 OnSimpleFriction(Vector3 force,bool isGround)
        {
            MyRigidbodyScale myRigidbodyScale = m_myRigidbodyScale != null ?
                m_myRigidbodyScale.RigidbodyScale : MyRigidbodyScale.Default();
            var result = force;

            if (isGround)
            {
                result.y = 0;
            }

            // 摩擦の計算
            float frictionValue = isGround
                ? myRigidbodyScale.Friction : myRigidbodyScale.AirFriction;

            var frictionDelta = frictionValue * timeScale;

            var noGravityVelocity = new Vector2(x: force.x, y: force.z);

            if(noGravityVelocity.sqrMagnitude > frictionValue * frictionDelta)
            {
                var normalized = noGravityVelocity.normalized;
                var frictionVector = normalized * frictionDelta;

                noGravityVelocity -= frictionVector; 
            }
            else
            {
                noGravityVelocity = Vector2.zero;
            }

            result.x = noGravityVelocity.x;
            result.z = noGravityVelocity.y;

            return result;
        }

        #endregion

    }

    public bool IsGround(out RaycastsLister groundInfo)
    {
        var myRigidbodyScale = m_myRigidbodyScale != null ?
            m_myRigidbodyScale.RigidbodyScale : MyRigidbodyScale.Default();
        groundInfo = null;
        var result = false;
        var velocity = this.m_velocity;

        if(velocity.y <= 0)
        {
            if(RayCheck(out groundInfo))
            {
                result = true;
            }
        }

        return result;

        #region local methods

        bool RayCheck(out RaycastsLister groundInfo)
        {
            groundInfo = null;
            if(m_groundTags == null)
            {
                return false;
            }

            var tagsLenght = m_groundTags.Length;

            var up = m_transform.up;
            var down = -up;
            var forward = m_transform.forward;
            var right = m_transform.right;

            var rayDistance = m_rayDistance + m_collisionScale;
            var groundPoint = GroundPoint + up * m_collisionScale;
            var groundLower = m_groundLower + m_collisionScale;

            var ray = new Ray(origin : groundPoint,direction : down);

            

            List<RaycastHit> hitList = new List<RaycastHit>(8);

            for(int i = -1; i <= 1; i++)
            {
                var rayRightOffset = right * i;

                for(int j = -1;j <= 1; j++)
                {
                    var rayForwardOffset = forward * j;
                    var rayOrigin = groundPoint + rayForwardOffset + rayRightOffset;

                    ray.origin = rayOrigin;

                    if (Physics.Raycast(ray,out var hit, rayDistance))
                    {
                        for(int k = 0;k < tagsLenght; k++)
                        {
                            var tag = m_groundTags[k];

                            if (hit.collider.CompareTag(tag))
                            {
                                if(result == false && hit.distance - CollisionScale <= groundLower)
                                {
                                    result = true;
                                }

                                hitList.Add(hit);
                                break;
                            }
                        }
                    }
                }
            }

            if (result == true)
            {
                groundInfo = new RaycastsLister(hitList);
                Debug.Log("hit");
            }

            return result;

        }

        #endregion

    }

    public void AddForce(Vector3 power)
    {
        m_velocity += power;
    }
    public void AddForce(float x, float y, float z)
    {
        var power = new Vector3(x, y, z);
        m_velocity += power;
    }
    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        I_MyRigidbody?.OnControllerColliderHit_MyRigid(hit);
    }
}

public class RaycastsLister
{
    public Vector3 anglesVariance = Vector3.zero;
    public Vector3 anglesAverage = Vector3.zero;

    public float distancesVariance = 0f;
    public float distancesAverage = 0f;

    public List<RaycastHit> raycastHits = null;

    public RaycastsLister(List<RaycastHit> raycastList)
    {
        raycastHits = raycastList;

        if (raycastList != null)
        {
            var listLength = raycastList.Count;
            var distancesSum = 0f;
            var anglesSum = Vector3.zero;

            for (int i = 0;i < listLength; i++)
            {
                var hit = raycastList[i];

                var distance = hit.distance;
                distancesSum += distance;

                var angle = hit.normal;
                anglesSum += angle;
            }

            this.distancesAverage = distancesSum / listLength;
            this.anglesAverage = anglesSum / listLength;



            var distancesVarianceSum = 0f;
            var anglesVarianceSum = Vector3.zero;

            for (int i = 0; i < listLength; i++)
            {
                var hit = raycastList[i];

                var distance = hit.distance;
                var distanceDeviation = distance - this.distancesAverage;
                var distanceVariance = distanceDeviation * distanceDeviation;

                distancesVarianceSum += distanceVariance;


                var angle = hit.normal;
                var angleDeviation = angle - this.anglesAverage;
                var angleVariance = Vector3.Scale(angleDeviation, angleDeviation);

                anglesVarianceSum += angleVariance;
            }

            this.distancesVariance = distancesVarianceSum / listLength;
            this.anglesVariance = anglesVarianceSum / listLength;
        }
    }
}

public interface I_MyRigidbody
{
    void OnControllerColliderHit_MyRigid(ControllerColliderHit hit);
}
[System.Serializable]
public struct MyRigidbodyScale
{
    [Header("重力の大きさ")][SerializeField] float m_gravityScale;
    [Header("摩擦力")][SerializeField] float m_friction;
    [Header("空気抵抗")][SerializeField] float m_airFriction;

    public float GravityScale { get => m_gravityScale; }
    public float Friction { get => m_friction; }
    public float AirFriction { get => m_airFriction; }


    public static MyRigidbodyScale Default()
    {
        return new MyRigidbodyScale()
        {
            m_gravityScale = -9.8f,
            m_friction = 40f,
            m_airFriction = 4f
        }; 
    }
}