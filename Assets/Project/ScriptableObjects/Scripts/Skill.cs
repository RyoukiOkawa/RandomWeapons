namespace RandomWeapons.SkillActions
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using RandomWeapons.Weapon;


    [CreateAssetMenu(menuName = "ScriptableObject/SkillAction",fileName = "newSkill")]
    public class Skill : ScriptableObject
    {
        //[Header("スキル名")] [SerializeField] string m_name = "";
        //[Header("スキルの内容（ベースステータス）")] [SerializeReference] IStatusSkill[] m_statusSkills = null;
        //[Header("スキルの内容（武器ステータス）")] [SerializeReference]  IWeaponSkill[] m_weaponSkills = null;
        //[Header("スキルの説明")] [TextArea(10, 10)][SerializeField] string m_explanation = "";


        //public string Name { get => m_name; internal set => m_name = value; }
        //public IStatusSkill[] StatusSkills { get => m_statusSkills; internal set => m_statusSkills = value; }
        //public IWeaponSkill[] WeaponSkill { get => m_weaponSkills;internal set => m_weaponSkills = value; }
        //public string Explanation { get => m_explanation; internal set => m_explanation = value; }
        
    }

    public enum ParametorMode
    {
        None,
        Add,
        Multiply,
    }
    public enum Parametor
    {
        None,
        StatusAttack,
        StatusDiffence,
        StatusSpeed,
    }

    [Serializable]
    public class SkillAction
    {
        //[SerializeField] ParametorMode Mode = ParametorMode.None;
        //[SerializeField] Parametor parametor = Parametor.None;
        //[Header("値（掛け算時はこれの1/100）")][SerializeField] int value = 100;

        //public CharacterStatus Apply(CharacterStatus status)
        //{
            
        //}
    }

    [Serializable]
    public class AddWeaponParameter
    {
        [SerializeField] CharacterStatus m_status;
        public CharacterStatus Actions(CharacterStatus status)
        {
            status += m_status;

            return status;
        }
    }
}