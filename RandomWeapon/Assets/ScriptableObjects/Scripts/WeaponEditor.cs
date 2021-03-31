namespace Myspace.Weapon
{
    using UnityEngine;
    using UnityEditor;

    [CustomEditor(typeof(Weapon))]
    internal class WeaponEditor : Editor
    {
        [SerializeField] Weapon weapon = null;
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            var instance = target as Weapon;

            var skillValue = instance.SkillSetValue.ParametersAndRate;

            

            var skillCnt = skillValue.Count;

            if (skillCnt != 0)
            {
                int[] vs = new int[skillCnt];
                skillValue.Keys.CopyTo(vs, 0);

                for(int i = 0; i < skillCnt; i++)
                {
                    GUILayout.BeginHorizontal();
                    var key = vs[i];
                    key = EditorGUILayout.IntField(key);

                    if(key != vs[i])
                    {
                        var vsi = skillValue[vs[i]];
                        skillValue.Remove(vs[i]);
                        skillValue.Add(key, vsi);
                    }

                    GUILayout.Label(" : " + skillValue[vs[i]]);
                    GUILayout.EndHorizontal();
                }
            }
        }
    }
}
