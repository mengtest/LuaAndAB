using System;
using System.Collections;
using System.Collections.Generic;

namespace LuaFramework
{
    /// <summary>
    /// 控制台对象
    /// </summary>
    public class Controller : IController
    {
        protected IDictionary<string, Type> m_Commands;
        protected IDictionary<IView, List<string>> m_ViewCommands;

        protected readonly object m_SyncRoot = new object();

        protected static volatile IController s_Instance = null;
        protected static readonly object m_StaticSyncRoot = new object();

        protected Controller()
        {
            this.InitializeController();
        }

        public virtual void RegisterCommand(string name, Type type)
        {
            lock (m_SyncRoot)
            {
                m_Commands[name] = type;
            }
        }

        public virtual void RemoveCommand(string commandName)
        {
            lock (m_SyncRoot)
            {
                if (m_Commands.ContainsKey(commandName))
                    m_Commands.Remove(commandName);
            }
        }

        public virtual void RegisterViewCommand(IView view, string[] commands)
        {
            lock (m_SyncRoot)
            {
                if (m_ViewCommands.ContainsKey(view))
                {
                    List<string> list = null;
                    if (m_ViewCommands.TryGetValue(view, out list))
                    {
                        for (int i = 0; i < commands.Length; i++)
                        {
                            if (list.Contains(commands[i]))
                                continue;
                            list.Add(commands[i]);
                        }
                    }
                }
                else
                {
                    m_ViewCommands.Add(view, new List<string>(commands));
                }
            }
        }

        public virtual void RemoveViewCommand(IView view, string[] commands)
        {
            lock (m_SyncRoot)
            {
                if (!m_ViewCommands.ContainsKey(view))
                    return;

                List<string> list = null;
                if (m_ViewCommands.TryGetValue(view, out list))
                {
                    for (int i = 0; i < commands.Length; i++)
                    {
                        if (list.Contains(commands[i]))
                            continue;
                        list.Remove(commands[i]);
                    }
                }
            }
        }

        public virtual void ExecuteCommand(IMessage message)
        {
            Type commandType = null;
            List<IView> views = null;
            lock (m_SyncRoot)
            {
                if (m_Commands.ContainsKey(message.name))
                {
                    commandType = m_Commands[message.name];
                }
                else
                {
                    views = new List<IView>();
                    foreach (var de in m_ViewCommands)
                    {
                        if (de.Value.Contains(message.name))
                            views.Add(de.Key);
                    }
                }
            }

            if (commandType != null)
            {
                object commandInstance = Activator.CreateInstance(commandType);
                if (commandInstance is ICommand)
                    ((ICommand)commandInstance).Execute(message);
            }

            if (views != null && views.Count != 0)
            {
                for (int i = 0; i < views.Count; i++)
                    views[i].OnMessage(message);//执行绑定Iview接口的的事件//
            }
        }

        public virtual bool HasCommand(string commandName)
        {
            lock (m_SyncRoot)
            {
                return m_Commands.ContainsKey(commandName);
            }
        }

        protected virtual void InitializeController()
        {
            m_Commands = new Dictionary<string, Type>();
            m_ViewCommands = new Dictionary<IView, List<string>>();
        }

        /// <summary>
        /// 返回实例对象
        /// </summary>
        public static IController instance
        {
            get
            {
                if (Controller.s_Instance == null)
                {
                    lock (m_StaticSyncRoot)
                    {
                        if (Controller.s_Instance == null)
                            Controller.s_Instance = new Controller();
                    }
                }
                return Controller.s_Instance;
            }
        }
    }
}
