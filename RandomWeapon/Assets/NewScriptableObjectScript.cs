using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CreateAssetMenu(fileName = "NewNewScriptableObjectScript", menuName = "ScriptableObject/NewScriptableObjectScript")]
public class NewScriptableObjectScript : ScriptableObject
{
    [SerializeField] Test m_test = new Test();
    private void OnValidate()
    {
        
    }
}

[CustomEditor(typeof(NewScriptableObjectScript))]
public class NewScriptableObjectScriptEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();


    }
}

[Serializable]
public class Test : UnityEngine.Object
{

}

[CustomEditor(typeof(Test))]
public class TestEditor : Editor
{
    public static Dictionary<Test, Editor> EditorSet = new Dictionary<Test, Editor>(8);
    private void OnEnable()
    {
        EditorSet.Add(target as Test, this);
    }
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();


    }
}
