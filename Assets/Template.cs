using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Template : MonoBehaviour
{
    internal string Name = "自由の女神";
    [SerializeField] List<A<int>> a;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}


[System.Serializable]
public class A<T>
{
    public B<T> b;


    public string Text;
}
[System.Serializable]
public class B<T>
{
    public T t;
}

namespace Myspace.Editor{
    using UnityEditor;

    [CustomPropertyDrawer(typeof(A<>))]
    public class AEditor : PropertyDrawer
    {





        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            var indent = EditorGUI.indentLevel;
            EditorGUI.indentLevel = 0;

            var a = property.FindPropertyRelative("b");

            var t = property.FindPropertyRelative("Text");

            EditorGUILayout.PropertyField(a);
            EditorGUILayout.PropertyField(t);

            EditorGUI.indentLevel = indent;
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return base.GetPropertyHeight(property, label) + EditorGUIUtility.singleLineHeight;
        }
    }

    [CustomPropertyDrawer(typeof(B<>))]
    public class BEditor : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            var a = property.FindPropertyRelative("t");

            EditorGUILayout.PropertyField(a);
        }
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return base.GetPropertyHeight(property, label) + EditorGUIUtility.singleLineHeight;
        }
    }
}



#region Editor Inspector Expansion

# if UNITY_EDITOR
namespace Myspace.Editor
{
    using UnityEngine;
    using UnityEditor;
    using UnityEngineInternal;
    using UnityEditorInternal;

    [CustomEditor(typeof(Template))]
    public class TemplatEditor : Editor
    {
        private bool ChangeNow = false;
        string ChangeText = "";
        private ReorderableList reorderableList;

        private void OnEnable()
        {
            //SerializedObjectからdata(Addressbook.data)を取得し、ReordableListを作成する
            reorderableList = new ReorderableList(serializedObject, serializedObject.FindProperty("a"));
            reorderableList.drawElementCallback += (Rect rect, int index, bool selected, bool focused) =>
            {
                SerializedProperty property = reorderableList.serializedProperty.GetArrayElementAtIndex(index);
                // PropertyFieldを使ってよしなにプロパティの描画を行う（PersonDataはPropertyDrawerを使っているのでそちらに移譲されます）
                EditorGUI.PropertyField(rect, property, GUIContent.none);
            };
            reorderableList.drawHeaderCallback += rect =>
            {
                EditorGUI.LabelField(rect, "Addressbok:");
            };
        }

        public override void OnInspectorGUI()
        {
            var tar = target as Template;
            base.OnInspectorGUI();


            reorderableList.DoLayoutList();


            GUILayout.Label(tar.Name);


            if (ChangeNow)
            {
                ChangeText = GUILayout.TextArea(ChangeText);

                if (GUILayout.RepeatButton("変更確定"))
                {
                    tar.Name = ChangeText;
                    ChangeNow = false;
                }
            }


            if (ChangeNow ? GUILayout.Button("変更取り消し") : GUILayout.Button("変更開始"))
            {

                ChangeText = tar.Name;
                ChangeNow = !ChangeNow;
            }


        }

    }
}
#endif

#endregion