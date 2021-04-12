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

[Serializable]
public struct CharacterStatus
{
    public int hp;
    public int attack;
    public int diffence;
    public int speed;
    public int EXP;


    public static CharacterStatus operator +(CharacterStatus a,CharacterStatus b)
    {
        var c = new CharacterStatus()
        {
            hp = a.hp + b.hp,
            attack = a.attack + b.attack,
            diffence = a.diffence + b.diffence,
            speed = a.speed + b.speed,
            EXP = a.EXP + b.EXP
        };

        return c;
    }
    public static CharacterStatus operator -(CharacterStatus a, CharacterStatus b)
    {
        var c = new CharacterStatus()
        {
            hp = a.hp - b.hp,
            attack = a.attack - b.attack,
            diffence = a.diffence - b.diffence,
            speed = a.speed - b.speed,
            EXP = a.EXP - b.EXP
        };

        return c;
    }
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

        m_test = EditorGUILayout.BeginToggleGroup("Test", m_test);

        m_level = EditorGUILayout.IntField(m_level);
        if (m_level <= 0)
            m_level = 1;
        else if (m_level > 100)
            m_level = 100;

        m_level = (int)GUILayout.HorizontalSlider(m_level, 1, 100);

        GUILayout.Space(7);

        EditorGUILayout.BeginVertical();
        EditorGUILayout.LabelField("Hp       : " + m_instance.m_hpCurve.Evaluate(m_level));
        EditorGUILayout.LabelField("Attack   : " + m_instance.m_atttackCurve.Evaluate(m_level));
        EditorGUILayout.LabelField("Diffence : " + m_instance.m_diffenceCurve.Evaluate(m_level));
        EditorGUILayout.LabelField("Speed    : " + m_instance.m_speedCurve.Evaluate(m_level));
        EditorGUILayout.LabelField("EXP      : " + m_instance.m_EXPCurve.Evaluate(m_level));
        EditorGUILayout.EndVertical();

        EditorGUILayout.EndToggleGroup();


        m_instance.m_atttackCurve = EditorGUILayout.CurveField(m_instance.m_atttackCurve);

        if (GUILayout.Button("Go"))
        {
            m_instance.m_atttackCurve = m_instance.m_atttackCurve.Format(0, 100);
        }
    }

    
}
public static partial class EX
{
    public static AnimationCurve Format(this AnimationCurve curve, float min, float max)
    {
        if(curve == null)
        {
            Debug.LogError("curve is null");
        }


        Keyframe[] keyframes = curve.keys;
        
        for(int i = 0;i< (keyframes.Length -1); i++)
        {
            if (keyframes[i].value < min)
            {
                if (keyframes[i].outTangent < 0)
                {
                    keyframes[i].outTangent = 0;
                }
                if (keyframes[i + 1].value < min)
                {
                    if (keyframes[i + 1].inTangent > 0)
                        keyframes[i + 1].inWeight = 0;
                }
                keyframes[i].value = min;
            }
            
        }
        return curve = new AnimationCurve(keyframes);

    }
}

#endif