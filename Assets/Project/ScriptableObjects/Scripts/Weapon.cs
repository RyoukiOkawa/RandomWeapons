namespace RandomWeapons.Weapon
{
    using System;
    using System.Linq;
    using System.Collections.Generic;
    using UnityEngine;


    [CreateAssetMenu(menuName = "ScriptableObject/Weapon", fileName = "newWeapon")]
    public class Weapon : ScriptableObject
    {
        [SerializeField] private string m_name = "";
        [SerializeField] private Texture2D m_image = null;
        [SerializeField] private WeightValueLayerList<Element> m_elementWeight = new WeightValueLayerList<Element>();
        [SerializeField] private WeightValueLayerList<int> m_killWeight = new WeightValueLayerList<int>();
        [SerializeField] private WeaponType m_type = WeaponType.Sword;

        public string Name { get => m_name; internal set => m_name = value; }
        public Texture2D Image { get => m_image; internal set => m_image = value; }
        public WeightValueLayerList<Element> ElementWeight { get => m_elementWeight; internal set => m_elementWeight = value; }
        public WeightValueLayerList<int> SkillWeight { get => m_killWeight; internal set => m_killWeight = value; }
        public WeaponType Type { get => m_type; internal set => m_type = value; }
    }

    public class WeaponParametor
    {

        WeaponParametor(Weapon weapon)
        {
            Weapon = weapon;
        }

        public Weapon Weapon { get; private set; }

        public int Attack { get; private set; }
        public Element Element { get; private set; }
    }


    public enum WeaponType
    {
        Hand,
        Sword
    }
}


#region Editor Only WeaponEditor

#if UNITY_EDITOR

namespace RandomWeapons.Editor
{



    using UnityEditor;
    using UnityEngine;
    using RandomWeapons.Weapon;

    [CustomEditor(typeof(Weapon))]
    internal class WeaponEditor : MyCustomEditor
    {
        WeightLayerChanger elementChager = new WeightLayerChanger();
        WeightLayerChanger SkillChager = new WeightLayerChanger();

        public override void OnInspectorGUI()
        {
            serializedObject.Update();


            var instance = target as Weapon;

            GUILayout.BeginHorizontal();


            GUILayout.BeginVertical();

            GUILayout.Label("武器の名前");
            instance.Name = EditorGUILayout.TextField(instance.Name);

            instance.Type = (WeaponType)EditorGUILayout.EnumPopup("装備モード",instance.Type);

            
            GUILayout.EndVertical();
            GUILayout.BeginVertical();
            
            instance.Image = EditorGUILayout.ObjectField("武器画像", instance.Image, typeof(Texture2D), true, GUILayout.Height(100),GUILayout.ExpandWidth(true)) as Texture2D;
            
            GUILayout.EndVertical();


            GUILayout.EndHorizontal();

            //EditorGUILayout.PropertyField(serializedObject.FindProperty("m_killWeight"));


            //var s = EditorGUILayout.ObjectField("説明文", new Object(), typeof(ScriptableObject), true) as ScriptableObject;



            instance.ElementWeight = WeightLayerGUIObject("属性のウェイト", instance.ElementWeight, elementChager);

            instance.SkillWeight = WeightLayerGUIInt("スキル数", instance.SkillWeight, SkillChager, 0, 5);


            var ele =serializedObject.FindProperty("m_elementWeight");

            EditorGUILayout.PropertyField(ele);



            GUILayout.Space(20);

            //if (GUILayout.Button("Save Asset"))
            //{
            //    serializedObject.ApplyModifiedProperties();
            //    EditorUtility.SetDirty(instance);
            //    AssetDatabase.SaveAssets();
            //}
        }
    }

}


#endif

#endregion