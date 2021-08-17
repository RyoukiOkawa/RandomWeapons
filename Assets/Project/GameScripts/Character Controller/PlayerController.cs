using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using RandomWeapons.Animations;

#if UNITY_EDITOR
using UnityEditor;
using RandomWeapons.Editor;
#endif

namespace RandomWeapons.Character
{
    [RequireComponent(typeof(Rigidbody))]
    public class PlayerController : MonoBehaviour
    {
        private Animator m_animator;
        private Rigidbody m_rigidbody;
        private Transform m_transform;
        private GameObject m_gameObject;



        private PlayerAttackState[] m_attackStates = null;
        private int m_currentWeaponLayer;


        #region Input Maps

        private InputAction AttackNormal;
        private InputAction AttackSpecial;
        private InputAction Move;
        private InputAction Jump;
        private InputAction Weapons1;
        private InputAction Weapons2;
        private InputAction Weapons3;
        private InputAction Weapons4;
        private InputAction Guard;

        #endregion

        #region GUI

#if UNITY_EDITOR

        bool previewGUI = false;

        private void OnGUI()
        {
            Rect rect = new Rect(0, 0, 200, EditorGUIUtility.singleLineHeight);

            previewGUI = GUI.Toggle(rect, previewGUI, "GUI");
            if (previewGUI)
            {
                rect = GetNextLineRect(rect);
                EditorGUI.Vector2Field(rect, "Move : ", Move.ReadValue<Vector2>());
            }
        }

        public Rect GetNextLineRect(Rect rect, float ySpace = 0, float? height = null)
        {
            var result = rect;

            result.y += rect.height + ySpace;

            result.height =
                (height is float hei)
                ? hei
                : EditorGUIUtility.singleLineHeight;

            return result;
        }

#endif

        #endregion

        // Start is called before the first frame update
        void Start()
        {
            InitInputMap();
            InitSetAnimators();
            InitProperty();

            #region local methods

            void InitInputMap()
            {
                var input = GetComponent<PlayerInput>();
                var map = input.actions;

                AttackNormal = map["AttackNormal"];
                AttackSpecial = map["AttackSpecial"];
                Move = map["Move"];
                Jump = map["Jump"];
                Weapons1 = map["Weapons1"];
                Weapons2 = map["Weapons2"];
                Weapons3 = map["Weapons3"];
                Weapons4 = map["Weapons4"];
                Guard = map["Guard"];
            }
            void InitSetAnimators()
            {
                m_animator = GetComponent<Animator>();
                m_attackStates = m_animator.GetBehaviours<PlayerAttackState>();
            }
            void InitProperty()
            {
                m_rigidbody = GetComponent<Rigidbody>();
                m_gameObject = gameObject;
                m_transform = transform;
            }

            #endregion
        }

        // Update is called once per frame
        void Update()
        {
            OperationCharacter();
        }

        private void OperationCharacter()
        {
            WeaponChange();
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
                var state = MyStateMachineBehaviour.GetCurrentStateMachine(m_currentWeaponLayer, m_attackStates);
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
                if (Weapons1.triggered)
                {
                    result.pushNumber = 0;
                }
                else if (Weapons2.triggered)
                {
                    result.pushNumber = 1;
                }
                else if (Weapons3.triggered)
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

            return result;

            #region local methods

            bool GetInputNormalAttack()
            {
                var result = AttackNormal.triggered;

                return result;
            }

            bool GetInputSpecialAttack()
            {
                var result = AttackSpecial.triggered;

                return result;
            }

            #endregion
        }

        private MethodsActions MoveCharcter()
        {
            MethodsActions methodsActions = new MethodsActions()
            {
                Actioning = false
            };

            var input = GetInput();
            ChangeForce(input.inputValue);
            ChangeAnimatorParametor(input.inputValue);

            return methodsActions;


            #region local methods

            (bool inputing,Vector2 inputValue) GetInput()
            {
                (bool inputing, Vector2 inputValue) result =
                    (
                        false,
                        Move.ReadValue<Vector2>()
                    );
                if(result.inputValue.sqrMagnitude > 1)
                {
                    result.inputing = true;
                }

                return result;
            }
            void ChangeForce(Vector2 inpuValue)
            {

            }
            void ChangeAnimatorParametor(Vector2 inputValue)
            {
                m_animator.SetFloat("MoveSpeed", inputValue.sqrMagnitude);
            }

            #endregion
        }

        private struct MethodsActions
        {
            public bool Actioning;
        }
    }
}
