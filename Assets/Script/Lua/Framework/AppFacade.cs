using UnityEngine;
using System.Collections;
using LuaFramework;

public class AppFacade : LuaFramework.Facade
{
    protected static AppFacade s_Instance;

    protected bool m_IsInitialized = false;

    AppFacade()
        : base()
    {
    }

    protected override void InitFramework()
    {
        base.InitFramework();

        //this.RegisterCommand(NotifyCommand.STARTUP, typeof(StartupCommand));
    }

    /// <summary>
    /// 启动框架
    /// </summary>
    public void StartUp()
    {
        this.SendMessageCommand(NotifyCommand.STARTUP);
        this.RemoveMultiCommand(NotifyCommand.STARTUP);
    }

    /// <summary>
    /// 初始化一些单例变量
    /// </summary>
    public void AddManager()
    {
        if (AppFacade.instance.GetManager<LuaManager>(ManagerName.LUA) == null)
        {
            //初始化管理器
            AppFacade.instance.AddManager<LuaManager>(ManagerName.LUA);
            AppFacade.instance.AddManager<SoundManager>(ManagerName.SOUND);
            //AppFacade.instance.AddManager<TimerManager>(ManagerName.Timer);
            //AppFacade.instance.AddManager<NetworkManager>(ManagerName.Network);
            AppFacade.instance.AddManager<ResourceManager>(ManagerName.RESOURCE);
            //AppFacade.instance.AddManager<ThreadManager>(ManagerName.Thread);
            //AppFacade.instance.AddManager<ObjectPoolManager>(ManagerName.ObjectPool);
            AppFacade.instance.AddManager<GameManager>(ManagerName.GAME);
        }
    }



    public static AppFacade instance
    {
        get
        {
            if (s_Instance == null)
                s_Instance = new AppFacade();
            return s_Instance;
        }
    }

    public bool isInitialized { set { m_IsInitialized = value; } get { return m_IsInitialized; } }
}
