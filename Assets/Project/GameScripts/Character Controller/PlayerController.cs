using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using RandomWeapons.Director;
using RandomWeapons.Animations;
using static RandomWeapons.Animations.MyStateMachineBehaviour;


namespace RandomWeapons.Character
{
    [DisallowMultipleComponent]
    [AddComponentMenu(menuName: "MyComponet/PlayerController")]
    [RequireComponent(typeof(MyRigidbody))]
    public class PlayerController : MonoBehaviour,IMyStarter,IMyUpdater,IMyFixedUpdater,IUseInput
    {
        private Animator m_animator;
        private MyRigidbody m_rigidbody;
        private Transform m_transform;
        private GameObject m_gameObject;

        [SerializeField, Min(0.01f)] float m_moveSpeeds = 1;
        [SerializeField, Min(0)] float m_jumpForce = 10;

        private PlayerAttackState[] m_attackStates = null;
        private int m_currentWeaponLayer;


        #region Input Maps

        private InputAction m_attackNormalAction;
        private InputAction m_attackSpecialAction;
        private InputAction m_moveAction;
        private InputAction m_jumpAction;
        private InputAction m_weapons1Action;
        private InputAction m_weapons2Action;
        private InputAction m_weapons3Action;
        private InputAction m_weapons4Action;
        private InputAction m_guardAction;

        #endregion

        void IUseInput.InitInput(PlayerInput input)
        {
            var map = input.actions;
            map.Enable();

            m_attackNormalAction = map["AttackNormal"];
            m_attackSpecialAction = map["AttackSpecial"];
            m_moveAction = map["Move"];
            m_jumpAction = map["Jump"];
            m_weapons1Action = map["Weapons1"];
            m_weapons2Action = map["Weapons2"];
            m_weapons3Action = map["Weapons3"];
            m_weapons4Action = map["Weapons4"];
            m_guardAction = map["Guard"];
        }

        void IMyStarter.MyStart()
        {

            InitSetAnimators();
            InitProperty();

            #region local methods

            void InitSetAnimators()
            {
                m_animator = GetComponent<Animator>();
                m_attackStates = m_animator.GetBehaviours<PlayerAttackState>();
            }
            void InitProperty()
            {
                m_rigidbody = GetComponent<MyRigidbody>();
                m_gameObject = gameObject;
                m_transform = transform;
            }

            #endregion
        }

        // Update is called once per frame
        void IMyUpdater.MyUpdate()
        {
            OperationCharacter();
        }

        void IMyFixedUpdater.MyFixedUpdate()
        {
            FixedOperationCharacter();
        }

        private void OperationCharacter()
        {
            WeaponChange();
            Attack();
            Jump();
            m_rigidbody.Move(Time.deltaTime);
        }
        private void FixedOperationCharacter()
        {
            MoveCharcter();
        }

        private MethodsActions WeaponChange()
        {
            MethodsActions methodsActions = new MethodsActions()
            {
                Actioning = false
            };

            var input = InputCheck();

            if (input.push)
            {
                if (GetStateChange())
                {
                    methodsActions.Actioning = ChangeAnimatorParametor(input.pushNumber);
                }
            }

            return methodsActions;


            #region local methods

            bool ChangeAnimatorParametor(int nextLayer)
            {
                bool changeing = false;
                var currentLayer = m_currentWeaponLayer;
                if(currentLayer != nextLayer)
                {
                    m_animator.SetLayerWeight(nextLayer, 1);
                    m_animator.SetLayerWeight(currentLayer, 0);

                    m_currentWeaponLayer = nextLayer;
                    changeing = true;
                }
                return changeing;
            }
            bool GetStateChange()
            {
                var state = GetCurrentStateMachine(m_currentWeaponLayer, m_attackStates);
                if(state is null)
                {
                    return true;
                }

                var result = state.GetStateAttack();

                return result;
            }
            (bool push,int pushNumber) InputCheck()
            {
                (bool push, int pushNumber) result = (true,0);
                if (m_weapons1Action.triggered)
                {
                    result.pushNumber = 0;
                }
                else if (m_weapons2Action.triggered)
                {
                    result.pushNumber = 1;
                }
                else if (m_weapons3Action.triggered)
                {
                    result.pushNumber = 2;
                }
                //else if (Weapons4.triggered)
                //{
                //      result.pushNumber = 3;
                //}
                else
                {
                    result.push = false;
                }
                return result;
            }

            #endregion
        }

        private MethodsActions Attack()
        {
            var result = new MethodsActions()
            {
                Actioning = false
            };

            if (GetInputSpecialAttack())
            {
                if (GetStateNextAttack())
                {
                    SetAnimatorParametor(2);
                    result.Actioning = true;
                }
            }
            else if (GetInputNormalAttack())
            {
                if (GetStateNextAttack())
                {
                    SetAnimatorParametor(1);
                    result.Actioning = true;
                }
            }


            return result;

            #region local methods

            bool GetInputNormalAttack()
            {
                var result = m_attackNormalAction.triggered;
                return result;
            }

            bool GetInputSpecialAttack()
            {
                var result = m_attackSpecialAction.triggered;
                return result;
            }

            bool GetStateNextAttack()
            {
                var result = false;

                if(TryGetCurrentStateMachine(m_currentWeaponLayer,m_attackStates,out var attack))
                {
                    result = attack.GetStateAttack();
                }
                else
                {
                    result = true;
                }

                return result;
            }
            void SetAnimatorParametor(int AttackType)
            {
                m_animator.SetTrigger("AttackTrigger");
                m_animator.SetInteger("AttackType", AttackType);
            }

            #endregion
        }

        private MethodsActions MoveCharcter()
        {
            MethodsActions methodsActions = new MethodsActions()
            {
                Actioning = false
            };

            if (IsMoveing())
            {
                var input = GetInput();

                if (input.inputing)
                {
                    ChangeAngle(input.inputValue);
                }
                ChangeForce(input.inputValue);
                ChangeAnimatorParametor(input.inputValue);

                methodsActions.Actioning = true;
            }
            return methodsActions;


            #region local methods

            bool IsMoveing()
            {
                var transitionInfo = m_animator.GetCurrentAnimatorStateInfo(m_currentWeaponLayer);

                var result = transitionInfo.IsName("BaseStanceTree");

                return result;
            }

            (bool inputing,Vector2 inputValue) GetInput()
            {
                (bool inputing, Vector2 inputValue) result =
                    (
                        false,
                        m_moveAction.ReadValue<Vector2>()
                    );
                if(result.inputValue.sqrMagnitude > 0)
                {
                    result.inputing = true;
                }

                return result;
            }

            void ChangeAngle(Vector2 input)
            {
                var cameraTransform = Camera.main.transform;
                var cameraAngleY = cameraTransform.localEulerAngles.y;


                var angles = m_transform.localEulerAngles;
                var inputAngle = GetAngle(input);


                var resultAngles = angles;
                resultAngles.y = cameraAngleY + inputAngle;

                m_transform.localEulerAngles = resultAngles; 


                float GetAngle(Vector2 values)
                {
                    var tan = Mathf.Atan2(values.x, values.y);
                    var angle = tan * Mathf.Rad2Deg;

                    return angle;
                }
            }

            void ChangeForce(Vector2 inpuValue)
            {
                var velocity = m_rigidbody.Velocity;
                var velocityY = velocity.y;

                float MoveSpeed = m_moveSpeeds;

                velocity = m_transform.forward * MoveSpeed * inpuValue.sqrMagnitude;
                velocity.y = velocityY;

                m_rigidbody.Velocity = velocity;

            }

            void ChangeAnimatorParametor(Vector2 inputValue)
            {
                m_animator.SetFloat("MoveSpeed", inputValue.sqrMagnitude);
            }

            #endregion
        }

        private MethodsActions Jump()
        {
            var result = new MethodsActions()
            {
                Actioning = false
            };

            var inputing = GetInput();

            if (inputing && GetJumpingFlag())
            {
                SetJumpForce();

                result.Actioning = true;
            }

            return result;

            #region local methods

            bool GetInput()
            {
                var result = m_jumpAction.triggered;

                return result;
            }
            bool GetJumpingFlag()
            {
                var result = true;

                if (m_rigidbody.IsGround(out var _) == false)
                {
                    result = false;
                }
                else if (TryGetCurrentStateMachine(m_currentWeaponLayer, m_attackStates,out var attack))
                {
                    result = false;
                }
                return result;
            }

            void SetJumpForce()
            {
                var jumpAngle = m_transform.up;
                var addForceValue = jumpAngle * m_jumpForce;


                m_rigidbody.AddForce(addForceValue);
            }

            #endregion
        }

        private struct MethodsActions
        {
            public bool Actioning;
        }
    }
}
