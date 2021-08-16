using System.Reflection;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
namespace RandomWeapons.Editor
{

    [CustomPropertyDrawer(typeof(RandomWeapons.Weapon.WeightValueLayer<>))]
    public class WeightLayerEditor : PropertyDrawer
    {
        string m_propetypath = null;

        SerializedPropertyFieldInfo parametor;
        SerializedPropertyFieldInfo rate;

        private void Init(SerializedProperty property)
        {
            if (m_propetypath != null)
            {
                return;
            }
            rate = new SerializedPropertyFieldInfo(property, fieldInfo, "m_rate");
            parametor = new SerializedPropertyFieldInfo(property, fieldInfo, "m_parameter");

            m_propetypath = property.propertyPath;
        }
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            Init(property);

            var fieldRect = position;
            // インデントされた位置のRectが欲しければこっちを使う
            var indentedFieldRect = EditorGUI.IndentedRect(fieldRect);
            fieldRect.height = EditorGUIUtility.singleLineHeight;

            // ラベルを表示し、ラベルの右側のプロパティを描画すべき領域のpositionを得る
            fieldRect = EditorGUI.PrefixLabel(fieldRect, GUIUtility.GetControlID(FocusType.Passive), label);

            // ここでIndentを0に
            var preIndent = EditorGUI.indentLevel;
            EditorGUI.indentLevel = preIndent - 1;


            var leftRect = fieldRect;

            leftRect.width /= 2;

            var rightRect = leftRect;

            rightRect.x += leftRect.width;

            // var va = parametor.GetFieldValue();


            //EditorGUI.LabelField(fieldRect, parametor.parentObject.ToString());

            EditorGUI.PropertyField(leftRect, rate.property, GUIContent.none);

            EditorGUI.PropertyField(rightRect, parametor.property, GUIContent.none);

            EditorGUI.indentLevel = preIndent;



        }
        /// プロパティの高さを取得する。カスタムによって高さが変わるなら必須
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return base.GetPropertyHeight(property, label) + EditorGUIUtility.singleLineHeight;
        }
    }
}