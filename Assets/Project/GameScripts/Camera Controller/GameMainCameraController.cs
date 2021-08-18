using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameMainCameraController : MonoBehaviour
{
    [SerializeField] Transform m_target;
    [SerializeField] Vector3 m_targetOffset = Vector3.zero;
    [SerializeField, Min(0)] float m_lenght = 0;
    [SerializeField] float m_rotateSpeed = 10;
    [SerializeField] Vector2 m_currentAngles = Vector2.zero;
    [SerializeField] float m_maxAngleX = 0;
    [SerializeField] float m_minAngleX = 0;


    private InputAction CameraMove;


    // Start is called before the first frame update
    void Start()
    {
        Init();
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
        var input = GetInput();

        if (input.inputing)
        {
            ChangeAngles(input.inputValue);
        }

        SetPosition();
    }

    private (bool inputing,Vector2 inputValue) GetInput()
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


        void ChangeParametor(Vector2 inputValue)
        {
            var resultAngles = m_currentAngles;

            resultAngles.x += inputValue.y * m_rotateSpeed;
            resultAngles.y += inputValue.x * m_rotateSpeed;

            if(resultAngles.x > m_maxAngleX)
            {
                resultAngles.x = m_maxAngleX;
            }
            else if(resultAngles.x < m_minAngleX)
            {
                resultAngles.x = m_minAngleX;
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
    }

    private void SetPosition()
    {

    }
}
