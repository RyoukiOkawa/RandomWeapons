using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

#if UNITY_EDITOR
using UnityEditor;
using RandomWeapons.Editor;
#endif

namespace RandomWeapons.Character
{
    [RequireComponent(typeof(Rigidbody))]
    public class PlayerController : MonoBehaviour,IMoveCharacter
    {
        private Animator m_animator;
        private Rigidbody m_rigidbody;
        private Transform m_transform;
        private GameObject m_gameObject;

        Rigidbody IMoveCharacter.Rigidbody => m_rigidbody;
        Transform ICharacter.Transform => m_transform;
        GameObject ICharacter.GameObject => m_gameObject;

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

        private void InitInputMap()
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

            m_rigidbody = GetComponent<Rigidbody>();
            m_gameObject = gameObject;
            m_transform = transform;
            m_animator = GetComponent<Animator>();
            
        }

        // Update is called once per frame
        void Update()
        {
            MoveCharcter();

            if (Weapons1.triggered)
            {
                m_animator.SetLayerWeight(1, 1);
                m_animator.SetLayerWeight(2, 0);
            }

            if (Weapons2.triggered)
            {
                m_animator.SetLayerWeight(1, 0);
                m_animator.SetLayerWeight(2, 1);
            }
        }

        public void MoveCharcter()
        {
            var moveValue = Move.ReadValue<Vector2>();

            m_animator.SetFloat("MoveSpeed", moveValue.sqrMagnitude);
        }

    }
}
