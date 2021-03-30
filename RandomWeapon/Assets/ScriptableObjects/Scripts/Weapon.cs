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
        [Header("武器の名前")] [SerializeField] string m_name = "";
        [Header("スキル数")] [SerializeField] MaxAndMin<int> m_skill;
        [Header("耐久値")] [SerializeField] MaxAndMin<int> m_Endurance;
        [Header("攻撃力")] [SerializeField] MaxAndMin<int> m_AttackPoint;
        [Header("属性")] [SerializeField] AttributeWeight[] m_attributeWeight = null;
        #region Editor Only
#if UNITY_EDITOR
        private void Awake()
        {
            m_skill = new MaxAndMin<int>(0,5);
            m_Endurance = new MaxAndMin<int>(0,1000);
            m_AttackPoint = new MaxAndMin<int>(20, 1000);
        }

#endif
        #endregion

        private void OnValidate()
        {
            m_skill.Format();
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
    public class AttributeWeight
    {
        [Header("属性")][SerializeField] Attribute m_attribute;
        [Header("属性の重み")][Range(1,100)] [SerializeField] int m_weight;
        [Header("属性の値")][SerializeField] MaxAndMin<int> m_attributeValue;

        AttributeWeight()
        {
            m_attribute = null;
            m_weight = 1;
            m_attributeValue = new MaxAndMin<int>(20, 500);
        }

        public int Weight { get => m_weight; }
        public Attribute Attribute { get => m_attribute; }
        public MaxAndMin<int> AttributeValue { get => m_attributeValue; set => m_attributeValue = value; }
    }

    [Serializable]
    public class SkillAction
    {
        
    }
}
