using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

namespace RandomWeapons.Animations
{

    public abstract class MyStateMachineBehaviour : StateMachineBehaviour
    {
        private AnimatorStateInfo? m_stateInfo = null;
        private int? m_layerIndex = null;

        /// <summary>
        /// 再生中であるか
        /// </summary>
        public bool Playing
        {
            get
            {
                var isNull = (m_stateInfo == null);
                return (isNull == false);
            }
        }
        /// <summary>
        /// 再生されてからの時間
        /// </summary>
        public float Time 
        { 
            get
            {
                if(m_stateInfo != null)
                {
                    var info = (AnimatorStateInfo)m_stateInfo;
                    var result = info.normalizedTime * info.length;
                    return result;
                }
                return 0;
            } 
        }
        /// <summary>
        /// どのアニメーターに属するか
        /// </summary>
        public int? LayerIndex => m_layerIndex;


        public AnimatorStateInfo? StateInfo => m_stateInfo;


        #region Unity's overridden methods

        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            base.OnStateEnter(animator, stateInfo, layerIndex);

            // 変数に値をセット

            m_stateInfo = stateInfo;
            m_layerIndex = layerIndex;
        }

        public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            base.OnStateUpdate(animator, stateInfo, layerIndex);

            m_stateInfo = stateInfo;
            m_layerIndex = layerIndex;
        }

        public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            base.OnStateExit(animator, stateInfo, layerIndex);

            // 変数を初期化

            m_stateInfo = null;
            m_layerIndex = null;
        }

        #endregion


        #region Static methods

        public static State GetCurrentStateMachine<State>(int layerIndex, State[] states) where State : MyStateMachineBehaviour
        {
            State result = null;

            for (int i = 0; i < states.Length; i++)
            {
                State state = states[i];

                if (state.Playing && state.LayerIndex == layerIndex)
                {
                    if (result == null || state.Time < result.Time)
                    {
                        result = state;
                    }
                }
            }

            return result;
        }

        public static bool TryGetCurrentStateMachine<State>(int layerIndex, State[] states,out State hitState) where State : MyStateMachineBehaviour
        {
            hitState = GetCurrentStateMachine(layerIndex, states);
            bool result = (hitState != null);

            return result;
        }


        public static State[] GetCurrentStateMachines<State>(int layerIndex, State[] states) where State : MyStateMachineBehaviour
        {
            List<State> result = new List<State>();

            for (int i = 0; i < states.Length; i++)
            {
                State state = states[i];

                if (state.Playing && state.LayerIndex == layerIndex)
                {
                    result.Add(state);
                }
            }

            return result.ToArray();
        }

        public static bool TryGetCurrentStateMachines<State>(int layerIndex, State[] states,out State[] hitStates) where State : MyStateMachineBehaviour
        {
            hitStates = GetCurrentStateMachines(layerIndex, states);
            var result = (hitStates != null);

            return result;
        }

            #endregion

        }
}
