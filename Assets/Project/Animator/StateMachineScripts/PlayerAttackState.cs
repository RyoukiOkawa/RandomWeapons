using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace RandomWeapons.Animations 
{

    public class PlayerAttackState : MyStateMachineBehaviour
    {
        [SubclassSelector, SerializeReference] IAttackState m_stateAttack = null;

        public bool GetStateAttack()
        {
            if (m_stateAttack == null)
                return true;

            return m_stateAttack.StateAttack(this);
        }
    }
    public interface IAttackState
    {
        public bool StateAttack(PlayerAttackState state);
    }

    public class NormalizedTimeState : IAttackState
    {
        [SerializeField] float m_normalizedTime = 0;
        public bool StateAttack(PlayerAttackState state)
        {
            var info = state.StateInfo;

            if(info is AnimatorStateInfo stateInfo)
            {
                if (stateInfo.normalizedTime > m_normalizedTime)
                {
                    return true;
                }
            }

            return false;
        }
    }

    public class TimeState : IAttackState
    {
        [SerializeField] float m_time = 0;
        public bool StateAttack(PlayerAttackState state)
        {
            if (state.Time > m_time)
                return true;
            return false;
        }
    }
}