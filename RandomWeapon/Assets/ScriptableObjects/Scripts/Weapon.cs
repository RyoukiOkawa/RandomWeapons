using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Myspace
{
    [Serializable]
    public struct MaxAndMin<T> where T : struct, IComparable<T>
    {
        [Header("最小値")] [SerializeField] T m_min;
        [Header("最大値")] [SerializeField] T m_max;

        #region Editor Only
#if UNITY_EDITOR
        private T m_maxToMaX;
        private T m_minToMin;

        public MaxAndMin(T MinToMin, T MaxToMax)
        {
            if (MinToMin.CompareTo(MaxToMax) > 0)
            {
                m_maxToMaX = MinToMin;
                m_minToMin = MaxToMax;
            }
            else
            {
                m_maxToMaX = MaxToMax;
                m_minToMin = MinToMin;
            }
            m_min = m_minToMin;
            m_max = m_maxToMaX;
        }
#endif
        #endregion


        public T Min { get => m_min; }
        public T Max { get => m_max; }

        /// <summary>
        /// 最小値を0以上、最大値以下
        /// <para>最大値を0以上最大値の最大値以下</para>
        /// </summary>
        public void Format()
        {
            var @default = new T();
            if (@default.CompareTo(m_min) > 0)
            {
                m_min = @default;
            }
            if (@default.CompareTo(m_max) > 0)
            {
                m_max = @default;
            }
            if (m_maxToMaX.CompareTo(m_max) < 0)
            {
                m_max = m_maxToMaX;
            }
            if (m_min.CompareTo(m_max) > 0)
            {
                m_min = m_max;
            }
        }
    }
}

namespace Myspace.Weapon
{
    [CreateAssetMenu(menuName = "ScriptableObject/Weapon", fileName = "newWeapon")]
    public class Weapon : ScriptableObject
    {
        public string Name { get; internal set; } = "";
        public Texture2D Image { get; internal set; } = null;

        
        // 耐久値
        [Header("耐久値")] [SerializeField] MaxAndMin<int> m_Endurance;
        [Header("攻撃力")] [SerializeField] MaxAndMin<int> m_AttackPoint;
        public WeightLayer<Element> ElementWeight { get; internal set; } = new WeightLayer<Element>();

        // スキルのセットできる数とそれに対する重み
        public WeightLayer<int> SkillWeight { get; internal set; } = new WeightLayer<int>();

        #region Editor Only
#if UNITY_EDITOR
        private void Awake()
        {
            m_Endurance = new MaxAndMin<int>(0,1000);
            m_AttackPoint = new MaxAndMin<int>(20, 1000);
        }

#endif
        #endregion

        private void OnValidate()
        {
            m_Endurance.Format();
            m_AttackPoint.Format();
        }
    }


    public enum SkillMode
    {
        Add,
        Multiply,
    }

    [Serializable]
    public class SkillAction
    {
        
    }

    public class WeightLayer<T>
    {
        internal Dictionary<T, float> ParametersAndRate {get; set; } = new Dictionary<T, float>();

        public float GetRate(T parameter)
        {
            if (ParametersAndRate.ContainsKey(parameter))
            {
                return ParametersAndRate[parameter];
            }
            else
            {
                Debug.Log("parameter is not found");
                return 0;
            }
        }

        public bool TryGetNameAndWeight(out string[] names,float[] weights)
        {
            var count = ParametersAndRate.Count;

            names = new string[count];
            weights = new float[count];

            if(count != 0)
            {
                names = ParametersAndRate.Keys
                    .Select(name => name.ToString())
                    .ToArray();

                ParametersAndRate.Values.CopyTo(weights, 0);

                return true;
            }
            return false;
        }

    }
    public class WeightCurveLayer
    {
        [SerializeField]
        WeightMode m_Mode = WeightMode.Int;


    }
    enum WeightMode
    {
        Int,
        Single
    }
}

#region Editor Only WeaponEditor

#if UNITY_EDITOR

namespace Myspace.Editor
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEditor;
    using Myspace.Weapon;

    [CustomEditor(typeof(Weapon))]
    internal class WeaponEditor : MyCustomEditor
    {
        WeightLayerChanger elementChager = new WeightLayerChanger();
        WeightLayerChanger SkillChager = new WeightLayerChanger();

        private void OnEnable()
        {

        }


        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            var instance = target as Weapon;

            GUILayout.BeginHorizontal();
            GUILayout.BeginVertical();
            GUILayout.Label("武器の名前");
            instance.Name = GUILayout.TextField(instance.Name);
            GUILayout.EndVertical();
            GUILayout.BeginVertical();
            instance.Image = EditorGUILayout.ObjectField("武器画像", instance.Image, typeof(Texture2D), true,GUILayout.Height(100)) as Texture2D;
            GUILayout.EndVertical();
            GUILayout.EndHorizontal();


            

            

            var skillValue = instance.SkillWeight.ParametersAndRate;

            //var s = EditorGUILayout.ObjectField("説明文", new Object(), typeof(ScriptableObject), true) as ScriptableObject;

            var skillCnt = skillValue.Count;

            WeightLayerGUIObject("属性のウェイト", instance.ElementWeight, elementChager);

            WeightLayerGUIInt("スキル数", instance.SkillWeight, SkillChager);

        }
    }
}

#endif

#endregion