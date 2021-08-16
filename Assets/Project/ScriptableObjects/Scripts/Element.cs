using System.Collections.Generic;
using UnityEngine;

namespace RandomWeapons
{
    [CreateAssetMenu(menuName = "ScriptableObject/Element")]
    public class Element : ScriptableObject
    {
        [Header("イメージ画像")][SerializeField] Texture2D m_attributeImage = null;
        [Header("攻撃のとき")][SerializeField] ElementLayer m_attackAttribute = null;

        public ElementLayer AttackAttribute { get => m_attackAttribute; }
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
    public class ElementLayer
    {
        [Header("得意属性")] [SerializeField] List<Element> m_effectivenessAttributes = null;
        [Header("苦手属性")] [SerializeField] List<Element> m_weakAttributes = null;

        public Element[] EffectivenessAttributes { get => m_effectivenessAttributes.ToArray(); }
        public Element[] WeakAttributes { get => m_weakAttributes.ToArray(); }

        public Compatibility GetCompatibility(Element attribute)
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
