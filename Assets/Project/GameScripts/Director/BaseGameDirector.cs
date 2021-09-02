using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace RandomWeapons.Director
{
    [RequireComponent(typeof(PlayerInput))]
    public abstract class BaseGameDirector : MonoBehaviour
    {
        private static BaseGameDirector m_gameDirector;
        public static BaseGameDirector GameDirector => m_gameDirector;

        protected List<IMyUpdater> m_myUpdaters = new List<IMyUpdater>();
        protected List<IMyFixedUpdater> m_myFixedUpdaters = new List<IMyFixedUpdater>();
        protected List<IMyLateUpdater> m_myLateUpdaters = new List<IMyLateUpdater>();

        protected PlayerInput m_playerInput = null;
        public PlayerInput PlayerInput => m_playerInput;

        virtual protected void Awake()
        {
            m_gameDirector = this;
        }

        virtual protected void Start()
        {
            this.m_playerInput = GetComponent<PlayerInput>();
            this.FindsSceneAddListAll();
        }

        protected void FindsSceneAddListAll()
        {
            var monos = GameObject.FindObjectsOfType<MonoBehaviour>();

            if (monos == null)
                return;

            var monoLenght = monos.Length;

            for(int i = 0;i < monoLenght;i++)
            {
                var mono = monos[i];

                var controls = mono.GetComponents<IControlsDircter>();

                if(controls == null)
                {
                    continue;
                }

                var controlsLenght = controls.Length;

                for(int j = 0;j < controlsLenght; j++)
                {
                    var control = controls[j];
                    if(control is IMyStarter starter)
                    {
                        starter.MyStart();
                    }
                    if(control is IUseInput useInput)
                    {
                        useInput.InitInput(m_playerInput);
                    }
                    this.AddControlsList(control);
                }
            }
        }

        // Update is called once per frame
        virtual protected void Update()
        {
            var updaterCount = m_myUpdaters.Count;

            for(int i = 0;i < updaterCount; i++)
            {
                var updater = m_myUpdaters[i];
                if(updater == null)
                {
                    m_myUpdaters.RemoveAt(i);
                    i--;
                    updaterCount--;
                    continue;
                }

                updater.MyUpdate();
            }
        }

        virtual protected void FixedUpdate()
        {
            var fixedUpdaterCount = m_myFixedUpdaters.Count;

            for (int i = 0; i < fixedUpdaterCount; i++)
            {
                var fixedUpdater = m_myFixedUpdaters[i];
                if (fixedUpdater == null)
                {
                    m_myFixedUpdaters.RemoveAt(i);
                    i--;
                    fixedUpdaterCount--;
                    continue;
                }

                fixedUpdater.MyFixedUpdate();
            }
        }

        virtual protected void LateUpdate()
        {
            var lateUpdaterCount = m_myLateUpdaters.Count;

            for (int i = 0; i < lateUpdaterCount; i++)
            {
                var lateUpdater = m_myLateUpdaters[i];
                if (lateUpdater == null)
                {
                    m_myLateUpdaters.RemoveAt(i);
                    i--;
                    lateUpdaterCount--;
                    continue;
                }

                lateUpdater.MyLateUpdate();
            }
        }

        #region set list methods

        public C InstantiateAndTryAddControlsList<C>(C c) where C : Component
        {
            var result = Instantiate(c);

            if(result is IMyStarter starter)
            {
                starter.MyStart();
            }
            if(result is IUseInput useInput)
            {
                useInput.InitInput(m_playerInput);
            }

            return result;
        }

        public void DestroyAndTryRemoveControlsList(Component component)
        {
            var controls = component.GetComponents<IControlsDircter>();

            if(controls != null)
            {
                this.ReMoveControlsList(controls);
            }

            Destroy(component);
        }

        public void AddControlsList(IControlsDircter[] controls)
        {
            var lenght = controls.Length;

            for(int i = 0;i < lenght; i++)
            {
                var control = controls[i];
                this.AddControlsList(control);
            }
        }

        public void AddControlsList(IControlsDircter control)
        {
            if (control is IMyUpdater updater)
            {
                if (this.m_myUpdaters.Contains(updater) == false)
                {
                    this.m_myUpdaters.Add(updater);
                }
            }

            if (control is IMyLateUpdater lateUpdater)
            {
                if (this.m_myLateUpdaters.Contains(lateUpdater) == false)
                {
                    this.m_myLateUpdaters.Add(lateUpdater);
                }
            }

            if (control is IMyFixedUpdater fixedUpdater)
            {
                if (this.m_myFixedUpdaters.Contains(fixedUpdater) == false)
                {
                    this.m_myFixedUpdaters.Add(fixedUpdater);
                }
            }
        }

        public void ReMoveControlsList(IControlsDircter[] controls)
        {
            var lenght = controls.Length;

            for (int i = 0; i < lenght; i++)
            {
                var control = controls[i];
                this.ReMoveControlsList(control);
            }
        }

        public void ReMoveControlsList(IControlsDircter control)
        {
            if ((control is IMyUpdater updater) && this.m_myUpdaters.Contains(updater))
            {
                this.m_myUpdaters.Remove(updater);
            }

            if ((control is IMyLateUpdater myLateUpdater) && this.m_myLateUpdaters.Contains(myLateUpdater))
            {
                this.m_myLateUpdaters.Remove(myLateUpdater);
            }

            if ((control is IMyFixedUpdater fixedUpdater) && this.m_myFixedUpdaters.Contains(fixedUpdater))
            {
                this.m_myFixedUpdaters.Remove(fixedUpdater);
            }
        }

        #endregion
    }

    #region interface

    public interface IControlsDircter
    {

    }

    public interface IMyStarter : IControlsDircter
    {
        void MyStart();
    }

    public interface IMyUpdater : IControlsDircter
    {
        void MyUpdate();
    }

    public interface IMyLateUpdater : IControlsDircter
    {
        void MyLateUpdate();
    }

    public interface IMyFixedUpdater : IControlsDircter
    {
        void MyFixedUpdate();
    }

    public interface IUseInput : IControlsDircter
    {
        void InitInput(PlayerInput input);
    }

    #endregion
}