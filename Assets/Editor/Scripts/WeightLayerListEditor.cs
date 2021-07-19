using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomPropertyDrawer(typeof(Myspace.Weapon.WeightLayerList<>))]
public class WeightLayerListEditor : PropertyDrawer
{
    string m_propetypath = null;
    SerializedProperty weightLayers;



    private void Init(SerializedProperty property)
    {
        if (m_propetypath != null)
        {
            return;
        }


            weightLayers = property.FindPropertyRelative("m_weightLayers");

        m_propetypath = property.propertyPath;
    }
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        Init(property);
        var fieldRect = position;
        // インデントされた位置のRectが欲しければこっちを使う
        var indentedFieldRect = EditorGUI.IndentedRect(fieldRect);
        fieldRect.height = EditorGUIUtility.singleLineHeight;


        var leftRect = fieldRect;

        leftRect.width /= 2;

        var rightRect = leftRect;

        rightRect.x += leftRect.width;

        EditorGUI.PropertyField(position,weightLayers, GUIContent.none);

    }

    /// プロパティの高さを取得する。カスタムによって高さが変わるなら必須
    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        return base.GetPropertyHeight(property, label) + EditorGUIUtility.singleLineHeight * 10;
    }
}
