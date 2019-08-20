using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace LuaFramework
{
    public class  Base : MonoBehaviour
    {
        protected AppFacade m_Facade;

        protected LuaManager m_LuaManager;
        protected SoundManager m_SoundManager;
        protected ResourceManager m_ResourceManager;
        protected ThreadManager m_ThreadManager;

        protected GameManager m_GameManager;

        void Awake()
        {
            //CDebug.Log("Awake");
            this.Init();
        }

        void Update()
        {
            this.Tick(Time.deltaTime);
        }

        void OnDestroy()
        {
            this.Term();
        }

        protected virtual void Init()
        {
        }

        protected virtual void Term()
        {
        }

        protected virtual void Tick(float time)
        {
        }

        protected AppFacade facade
        {
            get
            {
                if (m_Facade == null)
                    m_Facade = AppFacade.instance;
                return m_Facade;
            }
        }

        protected LuaManager luaManager
        {
            get
            {
                if (m_LuaManager == null)
                    m_LuaManager = facade.GetManager<LuaManager>(ManagerName.LUA);
                return m_LuaManager;
            }
        }

        protected SoundManager soundManager
        {
            get
            {
                if (m_SoundManager == null)
                    m_SoundManager = facade.GetManager<SoundManager>(ManagerName.SOUND);
                return m_SoundManager;
            }
        }

        protected ResourceManager resourceManager
        {
            get
            {
                if (m_ResourceManager == null)
                    m_ResourceManager = facade.GetManager<ResourceManager>(ManagerName.RESOURCE);
                return m_ResourceManager;
            }
        }

        protected GameManager gameManager
        {
            get
            {
                if (m_GameManager == null)
                    m_GameManager = facade.GetManager<GameManager>(ManagerName.GAME);
                return m_GameManager;
            }
        }
    }
}
