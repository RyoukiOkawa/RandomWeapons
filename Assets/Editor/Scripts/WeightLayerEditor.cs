using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomPropertyDrawer(typeof(Myspace.Weapon.WeightLayer<>))]
public class WeightLayerEditor : PropertyDrawer
{
    string m_propetypath = null;

    SerializedProperty parametor;
    SerializedProperty rate;



    private void Init(SerializedProperty property)
    {
        if (m_propetypath != null)
        {
            return;
        }
        parametor = property.FindPropertyRelative("m_parameter");
        rate = property.FindPropertyRelative("m_rate");

        m_propetypath = property.propertyPath;
    }
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        Init(property);

        var fieldRect = position;
        // インデントされた位置のRectが欲しければこっちを使う
        var indentedFieldRect = EditorGUI.IndentedRect(fieldRect);
        fieldRect.height = EditorGUIUtility.singleLineHeight;


        // Prefab化した後プロパティに変更を加えた際に太字にしたりする機能を加えるためPropertyScopeを使う
        using (new EditorGUI.PropertyScope(fieldRect, label, property))
        {
            // ラベルを表示し、ラベルの右側のプロパティを描画すべき領域のpositionを得る
            fieldRect = EditorGUI.PrefixLabel(fieldRect, GUIUtility.GetControlID(FocusType.Passive), label);

            // ここでIndentを0に
            var preIndent = EditorGUI.indentLevel;
            EditorGUI.indentLevel = preIndent -1;


            var leftRect = fieldRect;

            leftRect.width /= 2;

            var rightRect = leftRect;

            rightRect.x += leftRect.width;

            EditorGUI.PropertyField(leftRect, rate, GUIContent.none);

            EditorGUI.PropertyField(rightRect,parametor, GUIContent.none);

            EditorGUI.indentLevel = preIndent;
        }


    }
    /// プロパティの高さを取得する。カスタムによって高さが変わるなら必須
    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        return base.GetPropertyHeight(property, label) + EditorGUIUtility.singleLineHeight;
    }
}
