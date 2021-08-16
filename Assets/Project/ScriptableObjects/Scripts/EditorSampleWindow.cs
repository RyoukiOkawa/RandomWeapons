using UnityEditor;
using UnityEngine;
public class EditorSampleWindow : EditorWindow
{
    private Editor _editor;
    private ScriptableObject _target;

    private void OnGUI()
    {
        EditorGUI.BeginChangeCheck();
        _target = (ScriptableObject)EditorGUILayout.ObjectField("ScriptableObject", _target, typeof(ScriptableObject),true);
        if (EditorGUI.EndChangeCheck())
        {
            _editor = Editor.CreateEditor(_target);
        }

        if (_editor == null)
            return;

        _editor.OnInspectorGUI();
    }
}
