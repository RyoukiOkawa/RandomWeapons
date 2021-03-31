using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Template : MonoBehaviour
{
    internal string Name = "自由の女神";
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

#region Editor Inspector Expansion

# if UNITY_EDITOR
namespace Myspace.Editor
{
    using UnityEditor;

    [CustomEditor(typeof(Template))]
    public class TemplatEditor : Editor
    {
        private bool ChangeNow = false;
        string ChangeText = "";

        public override void OnInspectorGUI()
        {
            var tar = target as Template;
            base.OnInspectorGUI();

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