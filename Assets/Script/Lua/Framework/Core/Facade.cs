using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

namespace LuaFramework
{
    /// <summary>
    /// 事件命令
    /// </summary>
    public class ControllerCommand : ICommand
    {
        public virtual void Execute(IMessage message)
        {
        }
    }

    public class Facade
    {
        protected IController m_Controller;
        protected GameObject m_GameManager;

        //系统管理器对象
        protected static Dictionary<string, object> m_Managers = new Dictionary<string, object>();

        protected Facade()
        {
            this.InitFramework();
        }

        #region Command
        public virtual void RegisterCommand(string commandName, Type commandType)
        {
            m_Controller.RegisterCommand(commandName, commandType);
        }

        public virtual void RemoveCommand(string commandName)
        {
            m_Controller.RemoveCommand(commandName);
        }

        public virtual bool HasCommand(string commandName)
        {
            return m_Controller.HasCommand(commandName);
        }

        public void RegisterMultiCommand(Type commandType, params string[] commandNames)
        {
            int count = commandNames.Length;
            for (int i = 0; i < count; i++)
                this.RegisterCommand(commandNames[i], commandType);
        }

        public void RemoveMultiCommand(params string[] commandName)
        {
            int count = commandName.Length;
            for (int i = 0; i < count; i++)
                this.RemoveCommand(commandName[i]);
        }

        public void SendMessageCommand(string message, object body = null)
        {
            m_Controller.ExecuteCommand(new Message(message, body));
        }
        #endregion

        #region Manager
        /// <summary>
        /// 添加管理器
        /// </summary>
        /// <param name="typeName"></param>
        /// <param name="obj"></param>
        public void AddManager(string typeName, object obj)
        {
            if (!m_Managers.ContainsKey(typeName))
                m_Managers.Add(typeName, obj);
        }

        /// <summary>
        /// 添加Unity对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="typeName"></param>
        /// <returns></returns>
        public T AddManager<T>(string typeName) where T : Component
        {
            object result = null;
            m_Managers.TryGetValue(typeName, out result);
            if (result != null)
                return (T)result;
            Component c = m_GameManager.AddComponent<T>();
            m_Managers.Add(typeName, c);
            return default(T);
        }

        /// <summary>
        /// 删除管理器
        /// </summary>
        /// <param name="typeName"></param>
        public void RemoveManager(string typeName)
        {
            if (!m_Managers.ContainsKey(typeName))
                return;

            object manager = null;
            m_Managers.TryGetValue(typeName, out manager);
            Type type = manager.GetType();
            if (type.IsSubclassOf(typeof(MonoBehaviour)))
                GameObject.Destroy((Component)manager);
            m_Managers.Remove(typeName);
        }

        /// <summary>
        /// 获取系统管理器
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="typeName"></param>
        /// <returns></returns>
        public T GetManager<T>(string typeName) where T : class
        {
            if (!m_Managers.ContainsKey(typeName))
                return default(T);
            object manager = null;
            m_Managers.TryGetValue(typeName, out manager);
            return (T)manager;
        }
        #endregion

        protected virtual void InitFramework()
        {
            if (m_Controller == null)
                m_Controller = Controller.instance;

            if (m_GameManager == null)
            {
                m_GameManager = new GameObject();
                m_GameManager.name = "GameManager";
                m_GameManager.transform.position = Vector3.zero;
                m_GameManager.transform.localScale = Vector3.one;
            }
        }
    }
}
