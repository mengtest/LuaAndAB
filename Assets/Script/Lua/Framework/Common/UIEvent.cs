using UnityEngine;
using System.Collections;
using LuaInterface;

namespace LuaFramework
{
    public class UIEvent : MonoBehaviour
    {
        public static void AddButtonClick(GameObject obj, LuaFunction func)
        {
            UnityEngine.UI.Button button = obj.GetComponent<UnityEngine.UI.Button>();
            if (button == null)
                return;
            button.onClick.AddListener(() =>
            {
                func.Call<GameObject>(obj);
            });
        }

        public static void ClearButtonClick(GameObject obj)
        {
            UnityEngine.UI.Button button = obj.GetComponent<UnityEngine.UI.Button>();
            if (button == null)
                return;
            button.onClick.RemoveAllListeners();
        }

        public static void ClearToggleClick(GameObject obj)
        {
            UnityEngine.UI.Toggle toggle = obj.GetComponent<UnityEngine.UI.Toggle>();
            if (toggle == null)
                return;
            toggle.onValueChanged.RemoveAllListeners();
        }

        public static void AddToggleClick(GameObject obj, LuaFunction func)
        {
            UnityEngine.UI.Toggle button = obj.GetComponent<UnityEngine.UI.Toggle>();
            if (button == null)
                return;
            button.onValueChanged.AddListener((isbool) =>
            {
                func.Call<bool, GameObject>(isbool, obj);
            });
        }

        public static void AddButtonClickOnStudyEvent(GameObject obj, LuaFunction func)
        {
            UnityEngine.UI.Button button = obj.GetComponent<UnityEngine.UI.Button>();
            if (button == null)
                return;
            button.onClick.AddListener(() =>
            {
                func.Call<GameObject>(obj);
                //EventPatch.Trigger<int>(ProtocolManager.StudyBack,0);
                //if (UserInfo.myData != null) ProtocolManager.sendSaveStudyTrip(UserInfo.myData.token, 2, 1);
            });
        }

        static System.Action<int> _Back = null;
        public static void AddBack(int obj, LuaFunction func)
        {
            _Back = null;
            _Back = (id) =>
            {
                func.Call<int>(obj);
                //EventPatch.RemoveEventListener<int>("Escape", _Back);
            };
           // EventPatch.AddEventListener<int>("Escape", _Back);
        }

        public static void RemoveBack(int obj, LuaFunction func)
        {
            //EventPatch.AddEventListener<int>("Escape", _Back);
        }

        #region  /*lua事件*/
        /// <summary>
        /// 添加lua事件//
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="func"></param>
        public static void AddLuaEvent(string obj, LuaFunction func)
        {
           // EventPatch.AddEventListenerLua<string>(obj, func);
        }

        /// <summary>
        /// 删除lua事件
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="func"></param>
        public static void RemoveLuaEvent(string obj, LuaFunction func)
        {
           // EventPatch.RemoveEventListenerLua<string>(obj, func);
            //EventPatch.RemoveEventListener(obj);
        }

        /// <summary>
        /// 执行lua事件//
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="_args"></param>
        public static void TriggerLuaEvent(string obj, string _args)
        {
           // EventPatch.TriggerStepLua<string>(obj, _args);
        }

        #endregion


        public static void AddEvent(string obj, LuaFunction func)
        {
            System.Action<string> _addAct = null;
           // CDebug.LogError("AddEvent");

            _addAct = (id) =>
            {
                func.Call<string>(obj);
            };
            //EventPatch.AddEventListener<string>(obj, _addAct);
        }

        public static void RemoveEvent(string obj, LuaFunction func)
        {
           // CDebug.LogError("RemoveEvent");
            System.Action<string> _removeAct = null;

            _removeAct = (id) =>
            {
                func.Call<string>(obj);
            };
           // EventPatch.RemoveEventListener(obj);
        }

        ///执行在C#中添加的事件分发,在C#中运行//
        public static void TriggerEvent(string obj, LuaFunction func, LuaFunction _hiden)
        {
            //if (EventPatch.GetEventListener(obj) > 0)
            //{
            //    //GlobalUIManager.Instance.showWaitting(false);
            //    EventPatch.Trigger<LuaFunction>(obj, func);
            //    //GlobalUIManager.Instance.showWaitting(false);
            //    if (_hiden != null)
            //        _hiden.Call<System.Action>(() =>
            //      {
            //          GlobalUIManager.Instance.showWaitting(false);
            //      });

            //}
        }
    }
}
