using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomPropertyDrawer(typeof(RandomWeapons.Weapon.WeightValueLayerList<>))]
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

        position.height = EditorGUI.GetPropertyHeight(weightLayers);

        EditorGUI.indentLevel++;

        for(int i = 0;i < weightLayers.arraySize; i++)
        {
            var s = weightLayers.GetArrayElementAtIndex(i);
            var height = EditorGUI.GetPropertyHeight(s);
            position.height = height;
            EditorGUI.PropertyField(position, s);
            position.y += height;

        }


        EditorGUI.indentLevel--;

    }

    /// プロパティの高さを取得する。カスタムによって高さが変わるなら必須
    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        return base.GetPropertyHeight(property, label) + EditorGUIUtility.singleLineHeight * 10;
    }
}
