namespace Myspace.Weapon
{
    #region Editor Only WeaponEditor

#if UNITY_EDITOR
    using UnityEditor;
    using UnityEngine;
    using Myspace.Editor;

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
            serializedObject.Update();

            
            var instance = target as Weapon;

            GUILayout.BeginHorizontal();
            GUILayout.BeginVertical();
            GUILayout.Label("武器の名前");
            var pro = serializedObject.FindProperty("m_name");

            EditorGUILayout.PropertyField(pro);
            instance.Name = GUILayout.TextArea(instance.Name);
            GUILayout.EndVertical();
            GUILayout.BeginVertical();
            instance.Image = EditorGUILayout.ObjectField("武器画像", instance.Image, typeof(Texture2D), true, GUILayout.Height(100)) as Texture2D;
            GUILayout.EndVertical();
            GUILayout.EndHorizontal();


            //var s = EditorGUILayout.ObjectField("説明文", new Object(), typeof(ScriptableObject), true) as ScriptableObject;



            instance.ElementWeight = WeightLayerGUIObject("属性のウェイト", instance.ElementWeight, elementChager);

            instance.SkillWeight = WeightLayerGUIInt("スキル数", instance.SkillWeight, SkillChager);

            if (GUILayout.Button("Save Asset"))
            {
                EditorUtility.SetDirty(instance);
                AssetDatabase.SaveAssets();
            }

            serializedObject.ApplyModifiedProperties();

        }

        private void OnDisable()
        {
            Debug.Log("Lost");
        }
    }


#endif

    #endregion
}