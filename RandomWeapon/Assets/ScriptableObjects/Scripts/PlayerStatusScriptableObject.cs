using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "NewPlayerStatusScriptableObject",menuName = "ScriptableObject/PlayerStatusScriptableObject")]
public class PlayerStatusScriptableObject : ScriptableObject
{
    public AnimationCurve m_hpCurve { internal set; get; }
    public AnimationCurve m_atttackCurve { internal set; get; }
    public AnimationCurve m_diffenceCurve { internal set; get; }
    public AnimationCurve m_speedCurve { internal set; get; }
    public AnimationCurve m_EXPCurve { internal set; get; }

    //    string  OnValidate()
    //    {
    //       var d =  File.ReadAllText(Application.dataPath + "/" + filename.ToString() + ".json");

    //        string filename = "z";

    //#if UNITY_EDITOR
    //       return Application.dataPath + "/"+filename.ToString()+ ".json";
    //#else 
    //       return Application.persistentDataPath + "/" + ".json";
    //#endif
    //        return Application.persistentDataPath;
    //    }
}


public struct CharacterStatus
{
    public int hp;
    public int attack;
    public int diffence;
    public int speed;
    public int EXP;
}

#if UNITY_EDITOR

[CustomEditor(typeof(PlayerStatusScriptableObject))]
public class PlayerStatusScriptableObjectEditor : Editor
{
    PlayerStatusScriptableObject m_instance;
    int m_level = 1;
    bool m_test = false;

    private void OnEnable()
    {
        m_instance = target as PlayerStatusScriptableObject;

        m_instance.m_hpCurve = AnimationCurve.Linear(0, 10, 100, 1000);
        m_instance.m_atttackCurve = AnimationCurve.Linear(0, 10, 100, 1000);
        m_instance.m_diffenceCurve = AnimationCurve.Linear(0, 10, 100, 1000);
        m_instance.m_speedCurve = AnimationCurve.Linear(0, 10, 100, 1000);
        m_instance.m_EXPCurve = AnimationCurve.Linear(0, 10, 100, 1000);
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        m_test = GUILayout.Toggle(m_test, "Test");

        if (m_test)
        {
            m_level = EditorGUILayout.IntField(m_level);
            if (m_level <= 0)
                m_level = 1;
            else if (m_level > 100)
                m_level = 100;


            EditorGUILayout.BeginVertical();
            EditorGUILayout.LabelField("Hp       : " + m_instance.m_hpCurve.Evaluate(m_level));
            EditorGUILayout.LabelField("Attack   : " + m_instance.m_hpCurve.Evaluate(m_level));
            EditorGUILayout.LabelField("Diffence : " + m_instance.m_hpCurve.Evaluate(m_level));
            EditorGUILayout.LabelField("Speed    : " + m_instance.m_hpCurve.Evaluate(m_level));
            EditorGUILayout.LabelField("EXP      : " + m_instance.m_hpCurve.Evaluate(m_level));
            EditorGUILayout.EndVertical();
        }
    }
}

#endif