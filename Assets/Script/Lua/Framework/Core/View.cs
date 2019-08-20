using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace LuaFramework
{
    public class View : Base, IView
    {
        /// <summary>
        /// 注册消息
        /// </summary>
        /// <param name="view"></param>
        /// <param name="messages"></param>
        protected void RegisterMessage(IView view, List<string> messages)
        {
            if (messages == null || messages.Count == 0)
                return;
            Controller.instance.RegisterViewCommand(view, messages.ToArray());
        }

        /// <summary>
        /// 移除消息
        /// </summary>
        /// <param name="view"></param>
        /// <param name="messages"></param>
        protected void RemoveMessage(IView view, List<string> messages)
        {
            if (messages == null || messages.Count == 0)
                return;
            Controller.instance.RemoveViewCommand(view, messages.ToArray());
        }

        public virtual void OnMessage(IMessage message)
        {
        }

        public void OnclickTalkingDataBegin(string _name)
        {
           // TalkingDataPlugin.TrackPageBegin(_name);
        }

        public void OnclickTalkingDataEnd(string _name)
        {
           // TalkingDataPlugin.TrackPageEnd(_name);
        }
        public void TrackEvent(string _name)
        {
           /// TalkingDataPlugin.TrackEvent(_name);
        }
    }
}
