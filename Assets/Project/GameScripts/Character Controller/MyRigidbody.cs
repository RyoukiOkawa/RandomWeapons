using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
[RequireComponent(typeof(CharacterController))]
[AddComponentMenu(menuName: "MyComponet")]
public class MyRigidbody : MonoBehaviour
{
    private Transform m_transform;



    [SerializeField] Vector3 m_groundPointOffset = Vector3.zero;
    [SerializeField] float m_collisionScale = 5;
    [SerializeField] string m_groundTags = null;
    [SerializeField,Range(0,85)] float m_groundAngle = 0;
    private float m_rayDistance = 0;

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
        }
    }

    private float GetRayDistance()
    {
        var result = 0f;

        var collisionScale = m_collisionScale;
        var groundAngle = m_groundAngle;


        var collisionDiameter = collisionScale * 2;

        // h= a * tan(Angle)


        return result;
    }

    public Vector3 GroundPoint
        => m_transform.position + m_groundPointOffset;

    public CharacterController CharacterController { get; private set; }
    public Vector3 Velocity { get; set; } = Vector3.zero;
    public bool UseGravity { get; set; } = true;
    public bool IsKinematic { get; set; } = false;
    public bool GroundedMode { get; set; } = false;
    public bool GroundTouch { get; private set; }

    [SerializeField] MyRigidbodyScaleScriptableObject m_myRigidbodyScale = null;

    private I_MyRigidbody I_MyRigidbody = null;

    private void Awake()
    {
        m_transform = transform;
        CharacterController = GetComponent<CharacterController>();
        TryGetComponent(out this.I_MyRigidbody);
        IsKinematic = false;
    }
    private void LateUpdate()
    {
       // Move(MathFree.TimeMix);
    }
    public void Move(float timeScale)
    {
        MyRigidbodyScale myRigidbodyScale = m_myRigidbodyScale != null ?
            m_myRigidbodyScale.RigidbodyScale : MyRigidbodyScale.Default();

        Vector3 velocity = Velocity;

        if (CharacterController == null)
        {
            Debug.LogError(transform.gameObject.name + " Is Can Not Find CharacterController");
            return;
        }
        if (IsKinematic)
            return;
        if (Time.timeScale == 0)
            return;

        if (UseGravity)
        {
            velocity.y += myRigidbodyScale.GravityScale * timeScale;
        }
        Vector3 oldPosition = transform.position;
        if (GroundedMode && GroundTouch)
        {
            var vel = velocity * 100;
            CharacterController.SimpleMove(vel * timeScale);
        }
        else
        {
            CharacterController.Move(velocity * timeScale);
        }
        Vector3 newposition = transform.position;
        if (CharacterController.isGrounded)
        {
            GroundTouch = true;
            velocity.y = 0;
        }
        else
        {
            GroundTouch = Mathf.Abs(oldPosition.y - newposition.y) <= 0.0001 * timeScale ? true : false;
        }

        // 摩擦の計算
        float Value = GroundTouch
            ? myRigidbodyScale.Friction : myRigidbodyScale.AirResistance;

        Value *= timeScale;
        if (Mathf.Abs(velocity.x) < Value)
            velocity.x = 0;
        else
            velocity.x += velocity.x > 0 ? -Value : Value;
        if (Mathf.Abs(velocity.z) < Value)
            velocity.z = 0;
        else
            velocity.z += velocity.z > 0 ? -Value : Value;


        Velocity = velocity;


        //Vector3 OnFriction(Vector3 force)
        //{

        //}
    }

    public bool IsGround(out float angle)
    {
        angle = 0;
        var result = false;
        var velocity = this.Velocity;

        if(velocity.y <= 0)
        {

        }

        return result;

        bool RayCheck(out float angle)
        {
            angle = 0;
            if(m_groundTags == null)
            {
                return false;
            }

            var tagsLenght = m_groundTags.Length;

            var up = m_transform.up;
            var down = -up;
            var forward = m_transform.forward;
            var right = m_transform.right;

            var groundPoint = GroundPoint;
            var ray = new Ray(origin : groundPoint,direction : down);
            var rayDistance = 0.1f;

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
                        for(int k = 0;j< tagsLenght; k++)
                        {
                            var tag = m_groundTags[k];

                            
                        }
                    }
                }
            }

            return result;

        }
    }

    public void AddForce(Vector3 power)
    {
        Velocity += power;
    }
    public void AddForce(float x, float y, float z)
    {
        var power = new Vector3(x, y, z);
        Velocity += power;
    }
    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        I_MyRigidbody?.OnControllerColliderHit_MyRigid(hit);
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
    [Header("空気抵抗")][SerializeField] float m_airResistance;

    public float GravityScale { get => m_gravityScale; }
    public float Friction { get => m_friction; }
    public float AirResistance { get => m_airResistance; }


    public static MyRigidbodyScale Default()
    {
        return new MyRigidbodyScale()
        {
            m_gravityScale = -9.8f,
            m_friction = 40f,
            m_airResistance = 4f
        }; 
    }
}
public interface IMyRigidbodyScale
{
    MyRigidbodyScale GetRigidbodyScale();
}