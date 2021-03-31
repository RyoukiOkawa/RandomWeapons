using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        // スキルのセットできる数とそれに対する重み
        public WeightLayer<int> SkillSetValue { get; internal set; } = new WeightLayer<int>();
        // 耐久値
        [Header("耐久値")] [SerializeField] MaxAndMin<int> m_Endurance;
        [Header("攻撃力")] [SerializeField] MaxAndMin<int> m_AttackPoint;
        [Header("属性")] [SerializeField] WeightLayer<Attribute> m_attributeWeight;
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

    [Serializable]
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
    }

}
