using System.Collections.Generic;
using UnityEngine;

namespace Myspace
{
    [CreateAssetMenu(menuName = "ScriptableObject/Attribute")]
    public class Attribute : ScriptableObject
    {
        [Header("イメージ画像")][SerializeField] Texture2D m_attributeImage = null;
        [Header("攻撃のとき")][SerializeField] AttributeLayer m_attackAttribute = null;

        public AttributeLayer AttackAttribute { get => m_attackAttribute; }
        public Texture2D AttributeImage { get => m_attributeImage; }
    }
    public enum Compatibility
    {
        // 特になし
        None = 0,
        // いまいち
        Weak,
        // 有効
        Effectiveness,
    }

    [System.Serializable]
    public class AttributeLayer
    {
        [Header("得意属性")] [SerializeField] List<Attribute> m_effectivenessAttributes = null;
        [Header("苦手属性")] [SerializeField] List<Attribute> m_weakAttributes = null;

        public Attribute[] EffectivenessAttributes { get => m_effectivenessAttributes.ToArray(); }
        public Attribute[] WeakAttributes { get => m_weakAttributes.ToArray(); }

        public Compatibility GetCompatibility(Attribute attribute)
        {
            if (m_effectivenessAttributes != null && m_effectivenessAttributes.Contains(attribute))
            {
                return Compatibility.Effectiveness;
            }
            if (m_weakAttributes != null && m_weakAttributes.Contains(attribute))
            {
                return Compatibility.Weak;
            }
            return Compatibility.None;
        }
    }
}
