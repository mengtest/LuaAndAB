using UnityEngine;
using System;
using System.IO;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text.RegularExpressions;
using System.Reflection;
using LuaInterface;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace LuaFramework
{
    public class Util
    {
        public static int Int(object o)
        {
            return Convert.ToInt32(o);
        }

        public static float Float(object o)
        {
            return (float)Math.Round(Convert.ToSingle(o), 2);
        }

        public static long Long(object o)
        {
            return Convert.ToInt64(o);
        }

        public static int Random(int min, int max)
        {
            return UnityEngine.Random.Range(min, max);
        }

        public static float Random(float min, float max)
        {
            return UnityEngine.Random.Range(min, max);
        }

        public static int RandomInteger(int min, int max)
        {
            return UnityEngine.Random.Range(min, max);
        }

        public static string Uid(string uid)
        {
            int position = uid.LastIndexOf('_');
            return uid.Remove(0, position + 1);
        }

        public static long GetTime()
        {
            TimeSpan ts = new TimeSpan(DateTime.UtcNow.Ticks - new DateTime(1970, 1, 1, 0, 0, 0).Ticks);
            return (long)ts.TotalMilliseconds;
        }

        /// <summary>
        /// 搜索子物体组件-GameObject版
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="go"></param>
        /// <param name="subnode"></param>
        /// <returns></returns>
        public static T Get<T>(GameObject go, string subnode) where T : Component
        {
            if (go != null)
            {
                Transform sub = go.transform.Find(subnode);
                if (sub != null) return sub.GetComponent<T>();
            }
            return null;
        }

        /// <summary>
        /// 搜索子物体组件-Transform版
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="go"></param>
        /// <param name="subnode"></param>
        /// <returns></returns>
        public static T Get<T>(Transform go, string subnode) where T : Component
        {
            if (go != null)
            {
                Transform sub = go.Find(subnode);
                if (sub != null) return sub.GetComponent<T>();
            }
            return null;
        }

        public static void DestroyAllChildObject(Transform tran)
        {
            List<Transform> childs = new List<Transform>();
            foreach (Transform child in tran)
            {
                childs.Add(child);
            }
            int count = childs.Count;
            for (int i = 0; i < count; i++)
            {
                UnityEngine.Object.Destroy(childs[i].gameObject);
            }
        }

        /// <summary>
        /// 搜索子物体组件-Component版
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="go"></param>
        /// <param name="subnode"></param>
        /// <returns></returns>
        public static T Get<T>(Component go, string subnode) where T : Component
        {
            return go.transform.Find(subnode).GetComponent<T>();
        }

        /// <summary>
        /// 添加组件
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="go"></param>
        /// <returns></returns>
        public static T Add<T>(GameObject go) where T : Component
        {
            if (go != null)
            {
                T[] ts = go.GetComponents<T>();
                for (int i = 0; i < ts.Length; i++)
                {
                    if (ts[i] != null)
                        GameObject.Destroy(ts[i]);
                }
                return go.gameObject.AddComponent<T>();
            }
            return null;
        }

        /// <summary>
        /// 添加组件
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="go"></param>
        /// <returns></returns>
        public static T Add<T>(Transform go) where T : Component
        {
            return Add<T>(go.gameObject);
        }

        /// <summary>
        /// 查找子对象
        /// </summary>
        /// <param name="go"></param>
        /// <param name="subnode"></param>
        /// <returns></returns>
        public static GameObject Child(GameObject go, string subnode)
        {
            return Child(go.transform, subnode);
        }

        /// <summary>
        /// 查找子对象
        /// </summary>
        /// <param name="go"></param>
        /// <param name="subnode"></param>
        /// <returns></returns>
        public static GameObject Child(Transform go, string subnode)
        {
            Transform tran = go.Find(subnode);
            if (tran == null) return null;
            return tran.gameObject;
        }

        /// <summary>
        /// 取平级对象
        /// </summary>
        /// <param name="go"></param>
        /// <param name="subnode"></param>
        /// <returns></returns>
        public static GameObject Peer(GameObject go, string subnode)
        {
            return Peer(go.transform, subnode);
        }

        /// <summary>
        /// 取平级对象
        /// </summary>
        /// <param name="go"></param>
        /// <param name="subnode"></param>
        /// <returns></returns>
        public static GameObject Peer(Transform go, string subnode)
        {
            Transform tran = go.parent.Find(subnode);
            if (tran == null) return null;
            return tran.gameObject;
        }

        /// <summary>
        /// 计算字符串的MD5值
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static string MD5(string source)
        {
            MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
            byte[] data = System.Text.Encoding.UTF8.GetBytes(source);
            byte[] md5Data = md5.ComputeHash(data, 0, data.Length);
            md5.Clear();

            string destString = "";
            for (int i = 0; i < md5Data.Length; i++)
                destString += System.Convert.ToString(md5Data[i], 16).PadLeft(2, '0');
            destString = destString.PadLeft(32, '0');
            return destString;
        }

        /// <summary>
        /// 计算文件的MD5值
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        public static string MD5File(string file)
        {
            try
            {
                FileStream fs = new FileStream(file, FileMode.Open);
                System.Security.Cryptography.MD5 md5 = new System.Security.Cryptography.MD5CryptoServiceProvider();
                byte[] retVal = md5.ComputeHash(fs);
                fs.Close();

                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < retVal.Length; i++)
                {
                    sb.Append(retVal[i].ToString("x2"));
                }
                return sb.ToString();
            }
            catch (Exception ex)
            {
                throw new Exception("MD5File() fail, error:" + ex.Message);
            }
        }

        /// <summary>
        /// 清除所有子节点
        /// </summary>
        /// <param name="go"></param>
        public static void ClearChild(Transform go)
        {
            if (go == null) return;
            for (int i = go.childCount - 1; i >= 0; i--)
                GameObject.Destroy(go.GetChild(i).gameObject);
        }
        /// <summary>
        /// 清理内存
        /// </summary>
        public static void ClearMemory()
        {
            //GC.Collect(); Resources.UnloadUnusedAssets();
            //LuaManager mgr = AppFacade.Instance.GetManager<LuaManager>(ManagerName.Lua);
            //if (mgr != null) mgr.LuaGC();
        }

        /// <summary>
        /// 在手机端 可以访问到存放至本地的信息//
        /// </summary>
        public static string DataPath()
        {
            if (Application.isMobilePlatform)
                return Application.persistentDataPath + "/Data/";
            return "";// Application.persistentDataPath + "/" + LuaConst.OS_DIRECTORY + "/Data/";
        }

        /// <summary>
        /// 应用程序内容路径
        /// </summary>
        /// <returns></returns>
        public static string AppContentPath()
        {
            string path;
           // if (LuaConst.USE_ASSETS)
                path = Application.persistentDataPath + "/Assets/";

          //  else
            {
                path = Application.streamingAssetsPath +
#if UNITY_ANDROID || UNITY_EDITOR_WIN
 "/Assets/" + RuntimePlatform.Android + "/";

#elif UNITY_IPHONE || UNITY_EDITOR_OSX
                "/Assets/" + RuntimePlatform.IPhonePlayer + "/";
           
#endif
            }
            return path;
        }

        /// <summary>
        /// 添加lua单机事件
        /// </summary>
        /// <param name="go"></param>
        /// <param name="luafuc"></param>
        public static void AddClick(GameObject go, System.Object luafuc)
        {
            //UIEventListener.Get(go).onClick += delegate(GameObject o)
            //{
            //    LuaInterface.LuaFunction func = (LuaInterface.LuaFunction)luafuc;
            //    func.Call();
            //};
        }

        public static void Log(string str)
        {
            Debug.Log(str);
        }

        public static void LogWarning(string str)
        {
            Debug.LogWarning(str);
        }

        public static void LogError(string str)
        {
            Debug.LogError(str);
        }

        public static void CallbackLuaFunction(LuaFunction callback)
        {
            if (callback != null)
                callback.Call();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="go"></param>
        /// <param name="assembly"></param>
        /// <param name="classname"></param>
        /// <returns></returns>
        public static Component GetComponent(GameObject obj, string assembly, string classname)
        {
            Assembly asmb = Assembly.Load(assembly);
            Type t = asmb.GetType(assembly + "." + classname);
            return obj.GetComponent(t);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="go"></param>
        /// <param name="assembly"></param>
        /// <param name="classname"></param>
        /// <returns></returns>
        public static Component AddComponent(GameObject go, string assembly, string classname)
        {
            Assembly asmb = Assembly.Load(assembly);
            Type t = asmb.GetType(assembly + "." + classname);
            return go.AddComponent(t);
        }

       

        /// <summary>
        /// 判空 Lua中需要调用的地方调用//
        /// </summary>
        /// <param name="_obj"></param>
        /// <returns></returns>
        public static bool IsNull(UnityEngine.Object _obj)
        {
            if (_obj == null || _obj.ToString().Equals("null") || _obj.name.Equals(null) || _obj.name.Equals("null"))
            {
                _obj = null;
                return true;
            }
            return false;
        }

        /// <summary>
        /// 判不为空 Lua中需要调用的地方调用//
        /// </summary>
        /// <param name="_obj"></param>
        /// <returns></returns>
        public static bool NotNull(UnityEngine.Object _obj)
        {
            bool isNull = IsNull(_obj);
            if (isNull)
                return false;
            else
                return true;
        }

        public static void LoadAsset(int type, string assetPath, LuaFunction callback)
        {
            ResourceManager resourceManager = AppFacade.instance.GetManager<ResourceManager>(ManagerName.RESOURCE);
            if (resourceManager == null)
                callback.Call();
            else
                resourceManager.LoadAsset(type, assetPath, () => { callback.Call(); });
        }

        public static void UnloadAsset(int type)
        {
            ResourceManager resourceManager = AppFacade.instance.GetManager<ResourceManager>(ManagerName.RESOURCE);
            if (resourceManager != null)
                resourceManager.UnloadAsset(type);
        }

        public static void LoadScene(string asset, string name, LuaFunction callback)
        {
            ResourceManager resourceManager = AppFacade.instance.GetManager<ResourceManager>(ManagerName.RESOURCE);
            if (resourceManager == null)
                // callback.Call(null);
                callback.Call();
            else
                resourceManager.LoadScene(asset, name, (obj) => { callback.Call(obj); });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="assembly"></param>
        /// <param name="classname"></param>
        /// <returns></returns>
        public static UnityEngine.Object LoadResource(string name, string assembly, string classname)
        {
            Assembly asmb = Assembly.Load(assembly);
            Type type = asmb.GetType(assembly + "." + classname);
            ResourceManager resourceManager = AppFacade.instance.GetManager<ResourceManager>(ManagerName.RESOURCE);
            if (resourceManager == null)
                return Resources.Load(name, type);
            return resourceManager.LoadResource(name, type);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="assembly"></param>
        /// <param name="classname"></param>
        /// <returns></returns>
        public static UnityEngine.Object LoadAB(string name, string assembly, string classname, LuaFunction _callBack, string _ABID)
        {
            //Assembly asmb = Assembly.Load(assembly);
            //Type type = asmb.GetType(assembly + "." + classname);

            //if (LuaConst.USE_AB)
            //{
            //    ResourceManager resourceManager = AppFacade.instance.GetManager<ResourceManager>(ManagerName.RESOURCE);
            //    if (resourceManager != null)
            //    {
            //        string _url = "";
            //        if (_ABID.Length <= 0)
            //        {
            //            if (!name.EndsWith(".data"))
            //            {
            //                name += ".data";
            //            }

            //            string[] _list = name.Split(ResourceManager.split, StringSplitOptions.None);
            //            string _objName = _list[_list.Length - 1];
            //            _url = "file:///" + Application.streamingAssetsPath + "/SceneAB/" + _objName.ToLower();
            //        }
            //        else
            //        {
            //            int _abid = int.Parse(_ABID);
            //            if (ABServerManager.Instance.m_myABList.ContainsKey(_abid))
            //            {
            //                _url = ABServerManager.Instance.m_myABList[_abid].downloadUrl;
            //            }
            //        }

            //        Debug.LogError(_url);
            //        resourceManager.AddABIE(name, resourceManager.LoadAB(name, _url, type, (_obj) =>
            //        {
            //            if (_callBack != null)
            //            {
            //                _callBack.Call(_obj);
            //            }
            //        })
            //        );
            //    }
            //}
            //else
            //{
            //    ResourceManager resourceManager = AppFacade.instance.GetManager<ResourceManager>(ManagerName.RESOURCE);
            //    if (resourceManager != null)
            //    {
            //        resourceManager.AddABIE(name, resourceManager.LoadRes(name, type, (_obj) =>
            //        {
            //            if (_callBack != null)
            //            {
            //                _callBack.Call(_obj);
            //            }
            //        })
            //        );
            //    }
            //}
            return null;
        }

        public static AssetBundle GetAB(string _web, string _asset, LuaFunction _callBack)
        {
            //ResourceManager resourceManager = AppFacade.instance.GetManager<ResourceManager>(ManagerName.RESOURCE);
            //if (resourceManager != null)
            //{
            //    if (!LuaConst.USE_AB)
            //    {
            //        string _stream = "file:///" + Application.streamingAssetsPath + "/" + _web.ToLower();
            //        Debug.LogError(_stream);
            //        resourceManager.StartCoroutine(resourceManager.GetABToLua(_stream, _asset, _callBack));
            //    }
            //    else
            //    {
            //        Debug.LogError(_web);
            //        string Url = "";
            //        int _abid = int.Parse(_web);
            //        if (ABServerManager.Instance.m_myABList.ContainsKey(_abid))
            //        {
            //            Url = ABServerManager.Instance.m_myABList[_abid].downloadUrl;
            //            Debug.LogError("Url:" + Url);
            //            resourceManager.StartCoroutine(resourceManager.GetABToLua(Url, _asset, _callBack));
            //        }
            //        else
            //        {
            //            Debug.LogError("_abid is null");
            //            UnityEngine.Object _obj = null;
            //            _callBack.Call(_obj);

            //        }
            //    }
            //}
            //else
            //{
            //    UnityEngine.Object _obj = null;
            //    _callBack.Call(_obj);
            //}
            return null;
        }

        /// <summary>
        /// 执行Lua方法
        /// </summary>
        /// <param name="module"></param>
        /// <param name="func"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        public static object[] CallMethod(string module, string func, params object[] args)
        {
            LuaManager luaManager = AppFacade.instance.GetManager<LuaManager>(ManagerName.LUA);
            if (luaManager == null) return null;
            return luaManager.CallFunction(module + "." + func, args);
        }

        public static void Callback(System.Action func)
        {
            if (func != null)
                func();
        }

#if UNITY_EDITOR
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        static int CheckRuntimeFile()
        {
            if (!Application.isEditor)
                return 0;

            string streamPath = Application.dataPath + "/StreamingAssets/";
            if (!Directory.Exists(streamPath))
                return -1;

            string[] files = Directory.GetFiles(streamPath);
            if (files.Length == 0)
                return -1;

            //if (!File.Exists(streamPath + "files.txt"))
            //    return -1;

            string sourcePath = "";// Globe.SCRIPTS_ROOT + "/ToLua/Source/Generate/";
            if (!Directory.Exists(sourcePath))
                return -2;

            files = Directory.GetFiles(sourcePath);
            if (files.Length == 0)
                return -2;
            return 0;
        }
#endif

        public static bool CheckEnvironment()
        {
#if UNITY_EDITOR
            int resultId = Util.CheckRuntimeFile();
            if (resultId == -1)
            {
                Debug.LogError("没有找到框架所需要的资源，单击LuaFramework菜单下Build xxx Resource生成！！");
                EditorApplication.isPlaying = false;
                return false;
            }
            else if (resultId == -2)
            {
                Debug.LogError("没有找到Wrap脚本缓存，单击Lua菜单下Gen Lua Wrap Files生成脚本！！");
                EditorApplication.isPlaying = false;
                return false;
            }
#endif
            return true;
        }


        //这个函数能够是拿到在编辑状态SetActive为false的控件的，并且递归下去拿 add by ouhaoge 2016.12.22
        public static GameObject Control(GameObject go, string name)
        {
            if (go == null)
                return null;

            //return Control(name, mWndObject);
            return GetControl(name, go);
        }

        public static GameObject GetControl(string name, GameObject parent)
        {

            if (parent == null)
                return null;
            try
            {
                return ReturnControlX(name, parent);
            }
            catch
            {
                Debug.LogError("err:  " + name + " " + parent);
            }
            return null;
        }

        public static GameObject ReturnControlX(string name, GameObject parent)
        {
            Transform t = FindInChild(parent.transform, name);
            if (t == null) return null;
            return t.gameObject;
        }

        public static Transform FindInChild(Transform t, string name)
        {
            if (t == null) return null;
            Transform o = t.Find(name);
            if (o != null)
                return o;
            else
            {
                foreach (Transform ct in t)
                {
                    Transform no = FindInChild(ct, name);
                    if (no != null) return no;
                }
            }
            return null;
        }

        public static GameObject FindGameObject(string gameName)
        {
            return GameObject.Find(gameName);
        }

        public static void Destory(GameObject _go)
        {
            Destory(_go);
        }

        //加载扫描二维码的插件//
        public static void LoadPrefab(string obj, LuaFunction func)
        {
           // PanelManager.Instance.LoadPanelByName<QRcode>(obj).Init().m_onCloseAct = () => { func.Call<string>(obj); }; //执行lua脚本中的事件//
        }

        public static bool IsIpadDevice()
        {
            float proportion = GetSceneProportion();
            //if (proportion < MainSceneUILua.m_Ipad)
            //{
            //    return true;
            //}
            //else
            //{
            //    return false;
            //}
            return false;
        }

        public static bool IsIphoneX()
        {
            float proportion = GetSceneProportion();
            //if (proportion > MainSceneUILua.m_IphoneX)
            //{
            //    return true;
            //}
            //else
            {
                return false;
            }
        }

        public static float GetSceneProportion()
        {
            float proportion = (float)Screen.width / Screen.height;
            return proportion;
        }

        public static int LoadedSceneName()
        {
            //string _name = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;
            //AllSceneType _type = AllSceneType.FarmType;
            //switch (_name)
            //{
            //    case "LuaFarm_optimized":
            //        _type = AllSceneType.FarmType;
            //        break;
            //    case "Dersert_optimized":
            //        _type = AllSceneType.DersertType;
            //        break;
            //    default:
            //        _type = AllSceneType.FarmType;
            //        break;
            //}
            //return (int)_type;
            return 0;
        }

        /// <summary>
        /// 激活码购买的商店 id 
        /// 与服务器对应农场 11000 沙漠 21000
        /// </summary>
        /// <returns></returns>
        public static int LoadedSceneShopID()
        {
            string _name = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;
            int _type =11000;
            switch (_name)
            {
                case "LuaFarm_optimized":
                    _type = 11000;
                    break;
                case "Dersert_optimized":
                    _type = 21000;
                    break;
                default:
                    _type = 11000;
                    break;
            }
            return _type;
        }


        public static void DebugObj(UnityEngine.Object _obj)
        {
            Debug.LogError(_obj);
        }


        #region /*加载场景的预制件*/

        #region /*UI 预制件加载*/
        /// <summary>
        /// 加载UI
        /// （在跳转场景后 执行加载UI的回调函数）
        /// (便于在Lua中调用)
        /// 加入场景跳转，是为了，更好的释放内存  也可以直接加载AB//
        /// </summary>
        /// <param name="_mon"></param>
        /// <param name="ABID"></param>
        /// <param name="ResourcesPath"></param>
        /// <param name="Streaming"></param>
        /// <param name="_callBack"></param>
        public static void LoadUIScene(MonoBehaviour _mon, int ABID, string ResourcesPath, string Streaming, LuaFunction _callBack)
        {
           // AssetbundleEngine.instance.DisposeByCallBack(_mon, () =>
            {
                if (LuaConst.USE_AB == false)
                {
                    UnityEngine.Object _obj = UnityEngine.Object.Instantiate(Resources.Load(ResourcesPath));
                    if (_callBack != null)
                    {
                        _callBack.Call(_obj);
                    }
                }
                else
                {
                    LoadByABID(ABID, (_ui) =>
                     {
                         Debug.LogError(_ui);
                         Debug.LogError("ui is ok");
                         if (_ui == null)
                         {
                             _mon.StartCoroutine(Load(Streaming, _callBack));
                         }
                         else
                         {
                             if (_callBack != null)
                             {
                                 _callBack.Call(_ui);
                             }
                         }
                     });
                }
            }
            //);
        }
        private static void LoadByABID(int ABID, System.Action<GameObject> _callBack)
        {
            //if (ABServerManager.Instance.m_myABList.ContainsKey(ABID))
            //{
            //    string _url = ABServerManager.Instance.m_myABList[ABID].downloadUrl;

            //    AssetbundleEngine.instance.LoadAssetRefF<GameObject>(GlobalUIManager.Instance, _url, (pro) =>
            //    {

            //        if (LoadingDownLoadView.Instance == null)
            //        {
            //            GlobalUIManager.Instance.LoadABLua();
            //        }
            //        else
            //        {
            //            if (pro >= 0.98f)
            //            {
            //                LoadingDownLoadView.Instance.SetVisual(false);
            //                LoadingDownLoadView.Instance.SetType(LoadingDownLoadView.DownType.load,false);
            //                LoadingDownLoadView.Instance.SetTarProgress(0f);

            //            }
            //            else
            //            {
            //                LoadingDownLoadView.Instance.SetVisual(true);
            //                LoadingDownLoadView.Instance.SetType(LoadingDownLoadView.DownType.DownLoad, false);
            //                LoadingDownLoadView.Instance.SetTarProgress(pro);
            //            }
            //        }

            //    }, (_load) =>
            //    {

            //        //Debug.LogError("_load:" + _load);

            //        if (Time.time - GlobalUIManager.Instance.nowTime > Globe.MaxWaitting)
            //        {

            //            //  GlobalUIManager.Instance.WaitAct(() =>
            //            {
            //                if (_load < 0.9f)
            //                {
            //                    if (LoadingDownLoadView.Instance == null)
            //                    {
            //                        GlobalUIManager.Instance.LoadABLua();
            //                        LoadingDownLoadView.Instance.SetVisual(true);
            //                        LoadingDownLoadView.Instance.SetType(LoadingDownLoadView.DownType.load, false);
            //                        LoadingDownLoadView.Instance.SetTarProgress(_load);
            //                    }
            //                    else
            //                    {

            //                        LoadingDownLoadView.Instance.SetVisual(true);
            //                        LoadingDownLoadView.Instance.SetType(LoadingDownLoadView.DownType.load, false);
            //                        LoadingDownLoadView.Instance.SetTarProgress(_load);


            //                    }
            //                }
            //                else
            //                {
            //                    if (LoadingDownLoadView.Instance != null)
            //                    {
            //                        LoadingDownLoadView.Instance.SetVisual(false);
            //                        LoadingDownLoadView.Instance.SetType(LoadingDownLoadView.DownType.load, false);
            //                        LoadingDownLoadView.Instance.Close();
            //                    }
            //                }
            //            }
            //        }
            //        //, _waittingTime);

            //    }, (tt) =>
            //    {
            //        //  GlobalUIManager.Instance.WaitAct(() =>
            //        {
            //            if (LoadingDownLoadView.Instance == null)
            //            {
            //                // GlobalUIManager.Instance.LoadABLua();
            //            }
            //            else
            //            {
            //                LoadingDownLoadView.Instance.SetVisual(false); LoadingDownLoadView.Instance.Close();
            //            }
            //        }
            //        //, _waittingTime);

            //        Debug.LogError("AssetbundleEngine load is ok");

            //        GlobalUIManager.Instance.GotoABLoading(false,()=> {
            //            if (_callBack != null)
            //            {
            //                _callBack.Invoke(tt);
            //            }
            //        });
            //    });
            //}
            //else
            //{
            //    if (_callBack != null)
            //    {
            //        _callBack.Invoke(null);
            //    }
            //    GlobalUIManager.Instance.showTips("没有ABID");
            //}
        }
        private static IEnumerator Load(string _abName, LuaFunction _callBack)
        {
            AssetBundleCreateRequest _req = AssetBundle.LoadFromFileAsync(Application.streamingAssetsPath + "/" + _abName);

            Debug.LogError(Application.streamingAssetsPath + "/" + _abName);

            while (!_req.isDone)
            {
                yield return 0;
            }

            yield return 0;
            AssetBundle _ab = _req.assetBundle;

            Debug.LogError("_ad "+ _ab);

            UnityEngine.Object[] _all = _ab.LoadAllAssets();
            yield return 0;
            Debug.LogError("_all " + _all.Length);

            UnityEngine.Object _go = UnityEngine.Object.Instantiate(_all[0]);// _all[0];// UnityEngine.Object.Instantiate(_all[0]);
            if (_callBack != null)
            {
                _callBack.Call(_go);
            }
            yield return 0;
            _ab.Unload(false);
            yield return 0;

            yield return 0;

        }
        #endregion







        private static float _waittingTime = 2f;

        public static int LoadSceneGame(string _sceneName, LuaFunction _Act)
        {
            //CClearSceneData.ClearSceneData();
            //Debug.LogError("LoadSceneGame");

            //// //GameObject _go = Resources.Load(_sceneName) as GameObject;
            ////// GameObject _lua = UnityEngine.Object.Instantiate(_go);// Resources.Load<MainSceneUILua>(_sceneName) as MainSceneUILua
            //// UnityEngine.Object Preb = Resources.Load(_sceneName, typeof(GameObject));
            //// //用加载得到的资源对象，实例化游戏对象，实现游戏物体的动态加载
            //// GameObject sphere = GameObject.Instantiate(Preb) as GameObject;


            ////判断有没有_sceneName该资源，如果有的话，直接读取，说明是测试包，如果没有，需要从服务器下载//
            //if (LuaConst.USE_AB == true)
            //{
            //    int _key = -100;
            //    if (int.TryParse(_sceneName, out _key) == true)
            //    {
            //        _key = int.Parse(_sceneName);
            //    }

            //    Debug.Log("_key:" + _key);
            //    //表示从网络下载 (传递过来的是ID)//
            //    if (ABServerManager.Instance.m_myABList.ContainsKey(_key))
            //    {
            //        GlobalUIManager.Instance.GotoABLoading(true);

            //        ///ab包不需要 转圈//
            //        GlobalUIManager.Instance.showLoginWait(false);

            //        // PanelManager.Instance.getTransfromByABURL(_ab.m_myABList[10001].downloadUrl);
            //        string _url = ABServerManager.Instance.m_myABList[_key].downloadUrl;// "http://192.168.3.28:85" + "/" + _abName + ".mmp3";

            //        //GlobalUIManager.Instance.WaitAct(() =>
            //        //{
            //        //    if (LoadingDownLoadView.Instance == null)
            //        //    {
            //        //        GlobalUIManager.Instance.LoadABLua();
            //        //        LoadingDownLoadView.Instance.SetType(LoadingDownLoadView.DownType.load);
            //        //        LoadingDownLoadView.Instance.SetVisual(true);
            //        //        LoadingDownLoadView.Instance.SetTarProgress(0);
            //        //    }
            //        //    else
            //        //    {
            //        //        LoadingDownLoadView.Instance.SetVisual(true);
            //        //        LoadingDownLoadView.Instance.SetType(LoadingDownLoadView.DownType.DownLoad);
            //        //        LoadingDownLoadView.Instance.SetTarProgress(0);
            //        //    }
            //        //}, 0.75f);

            //        //if (Time.time - GlobalUIManager.Instance.nowTime > 3f)
            //        //{

            //        //    GlobalUIManager.Instance.nowTime = -1f;
            //        //}
            //        //else
            //        {
            //            GlobalUIManager.Instance.nowTime = Time.time;
            //        }

            //        Debug.LogError(_url);

            //        AssetbundleEngine.instance.LoadAssetRefF<GameObject>(GlobalUIManager.Instance, _url, (pro) =>
            //        {

            //            if (LoadingDownLoadView.Instance == null)
            //            {
            //                GlobalUIManager.Instance.LoadABLua();
            //            }
            //            else
            //            {
            //                if (pro >= 0.98f)
            //                {
            //                    LoadingDownLoadView.Instance.SetVisual(false);
            //                    LoadingDownLoadView.Instance.SetType(LoadingDownLoadView.DownType.load);
            //                    LoadingDownLoadView.Instance.SetTarProgress(0f);

            //                }
            //                else
            //                {
            //                    LoadingDownLoadView.Instance.SetVisual(true);
            //                    LoadingDownLoadView.Instance.SetType(LoadingDownLoadView.DownType.DownLoad);
            //                    LoadingDownLoadView.Instance.SetTarProgress(pro);
            //                }
            //            }

            //        }, (_load) =>
            //        {

            //            //Debug.LogError("_load:" + _load);

            //            if (Time.time - GlobalUIManager.Instance.nowTime > Globe.MaxWaitting)
            //            {

            //                //  GlobalUIManager.Instance.WaitAct(() =>
            //                {
            //                    if (_load < 0.9f)
            //                    {
            //                        if (LoadingDownLoadView.Instance == null)
            //                        {
            //                            GlobalUIManager.Instance.LoadABLua();
            //                            LoadingDownLoadView.Instance.SetVisual(true);
            //                            LoadingDownLoadView.Instance.SetType(LoadingDownLoadView.DownType.load);
            //                            LoadingDownLoadView.Instance.SetTarProgress(_load);
            //                        }
            //                        else
            //                        {

            //                            LoadingDownLoadView.Instance.SetVisual(true);
            //                            LoadingDownLoadView.Instance.SetType(LoadingDownLoadView.DownType.load);
            //                            LoadingDownLoadView.Instance.SetTarProgress(_load);


            //                        }
            //                    }
            //                    else
            //                    {
            //                        if (LoadingDownLoadView.Instance != null)
            //                        {
            //                            LoadingDownLoadView.Instance.SetVisual(false);
            //                            LoadingDownLoadView.Instance.SetType(LoadingDownLoadView.DownType.load);
            //                            LoadingDownLoadView.Instance.Close();
            //                        }
            //                    }
            //                }
            //            }
            //            //, _waittingTime);

            //        }, (tt) =>
            //        {
            //            //  GlobalUIManager.Instance.WaitAct(() =>
            //            {

            //            }
            //            //, _waittingTime);

            //            Debug.LogError("AssetbundleEngine load is ok");

            //            //Resources.UnloadUnusedAssets();

            //            AssetbundleEngine.instance.DisposeByCallBack(GlobalUIManager.Instance, () => {
                           
            //                if (_Act != null)
            //                {
            //                    _Act.Call(tt);
            //                }

            //                if (LoadingDownLoadView.Instance == null)
            //                {
            //                    // GlobalUIManager.Instance.LoadABLua();
            //                }
            //                else
            //                {
            //                    LoadingDownLoadView.Instance.SetVisual(false); LoadingDownLoadView.Instance.Close();
            //                }
            //                GlobalUIManager.Instance.GotoABLoading(false);

            //            });

            //            //GlobalUIManager.Instance.WaitAct(() => { }, 01f);

                       
            //        });

            //    }
            //    else
            //    {
            //        GlobalUIManager.Instance.showTips("网络下载资源出错了,读取本地资源咯！");
            //        //读取本地//
            //        string _prefabName = "ScenePrefab/" + _sceneName;
            //        Debug.Log("-------------------************************-------------------:" + _prefabName);
            //        GlobalUIManager.Instance.StartCoroutine(Load(new string[] { _prefabName }, _Act));
            //    }
            //}
            //else
            //{
            //    string _prefabName = "ScenePrefab/" + _sceneName;
            //    Debug.Log("-------------------************************-------------------:" + _prefabName);
            //    GlobalUIManager.Instance.StartCoroutine(Load(new string[] { _prefabName }, _Act));
            //}




            // Resources.UnloadUnusedAssets();
            return 0;
        }


        public static int LoadSceneGame(string _sceneName)
        {

            Debug.LogError("LoadSceneGame");

            // //GameObject _go = Resources.Load(_sceneName) as GameObject;
            //// GameObject _lua = UnityEngine.Object.Instantiate(_go);// Resources.Load<MainSceneUILua>(_sceneName) as MainSceneUILua
            // UnityEngine.Object Preb = Resources.Load(_sceneName, typeof(GameObject));
            // //用加载得到的资源对象，实例化游戏对象，实现游戏物体的动态加载
            // GameObject sphere = GameObject.Instantiate(Preb) as GameObject;
            string _prefabName = "ScenePrefab/" + _sceneName;
            Debug.Log("-------------------************************-------------------:" + _prefabName);
            //GlobalUIManager.Instance.StartCoroutine(Load(new string[] { _prefabName }, null));



            // Resources.UnloadUnusedAssets();
            return 0;
        }

        /// <summary>
        /// 加载AR预制件
        /// lua 脚本中调用
        /// </summary>
        /// <param name="_sceneName"></param>
        /// <param name="_Act"></param>
        /// <returns></returns>
        public static int LoadARGame(string _sceneName, LuaFunction _Act)
        {
            //CClearSceneData.ClearSceneData();
            //if (LuaConst.USE_AB == true)
            //{
            //    int _key = -100;
            //    if (int.TryParse(_sceneName, out _key) == true)
            //    {
            //        _key = int.Parse(_sceneName);
            //    }

            //    //表示从网络下载 (传递过来的是ID)//
            //    if (ABServerManager.Instance.m_myABList.ContainsKey(_key))
            //    {
            //        // PanelManager.Instance.getTransfromByABURL(_ab.m_myABList[10001].downloadUrl);
            //        string _url = ABServerManager.Instance.m_myABList[_key].downloadUrl;// "http://192.168.3.28:85" + "/" + _abName + ".mmp3";
            //                                                                            //Debug.LogError(_url);

            //        bool _islocal = AssetbundleEngine.instance.isLocal(_url);

            //        GlobalUIManager.Instance.nowTime = Time.time;

            //        //如果是本地
            //        if (!_islocal)
            //        {
            //            if (LoadingDownLoadView.Instance == null)
            //            {
            //                GlobalUIManager.Instance.LoadABLua();
            //                LoadingDownLoadView.Instance.SetVisual(true);
            //                LoadingDownLoadView.Instance.SetTarProgress(0);
            //            }
            //            else
            //            {
            //                LoadingDownLoadView.Instance.SetVisual(true);
            //                LoadingDownLoadView.Instance.SetType(LoadingDownLoadView.DownType.DownLoad);
            //                LoadingDownLoadView.Instance.SetTarProgress(0);
            //            }
            //        }

            //        AssetbundleEngine.instance.LoadAssetRefF<GameObject>(GlobalUIManager.Instance, _url, (pro) =>
            //        {
            //            if (!_islocal)
            //            {
            //                if (LoadingDownLoadView.Instance == null)
            //                {
            //                    GlobalUIManager.Instance.LoadABLua();
            //                    LoadingDownLoadView.Instance.SetVisual(true);
            //                    LoadingDownLoadView.Instance.SetType(LoadingDownLoadView.DownType.DownLoad);
            //                    LoadingDownLoadView.Instance.SetTarProgress(pro);
            //                }
            //                else
            //                {
            //                    LoadingDownLoadView.Instance.SetVisual(true);
            //                    LoadingDownLoadView.Instance.SetType(LoadingDownLoadView.DownType.DownLoad);
            //                    LoadingDownLoadView.Instance.SetTarProgress(pro);
            //                }
            //            }

            //        }, (_load) =>
            //        {
            //            if (Time.time - GlobalUIManager.Instance.nowTime > Globe.MaxWaitting)
            //            {
            //                if (LoadingDownLoadView.Instance == null)
            //                {
            //                    GlobalUIManager.Instance.LoadABLua();
            //                    LoadingDownLoadView.Instance.SetVisual(true);
            //                    LoadingDownLoadView.Instance.SetType(LoadingDownLoadView.DownType.load);
            //                    LoadingDownLoadView.Instance.SetTarProgress(_load);
            //                }
            //                else
            //                {
            //                    LoadingDownLoadView.Instance.SetVisual(true);
            //                    LoadingDownLoadView.Instance.SetType(LoadingDownLoadView.DownType.load);
            //                    LoadingDownLoadView.Instance.SetTarProgress(_load);
            //                }
            //            }

            //        }, (tt) =>
            //        {
            //            if (LoadingDownLoadView.Instance == null)
            //            {
            //                //GlobalUIManager.Instance.LoadABLua();
            //            }
            //            else
            //            {
            //                LoadingDownLoadView.Instance.SetVisual(false);
            //                LoadingDownLoadView.Instance.Close();
            //            }

            //            Debug.LogError("AssetbundleEngine load is ok");
            //            if (_Act != null)
            //            {
            //                _Act.Call(tt);
            //            }
            //        });

            //    }
            //    else
            //    {
            //        //GlobalUIManager.Instance.showTips("网络下载资源出错了,读取本地资源咯！");
            //        Debug.LogError("网络下载资源出错了,读取本地资源咯！");

            //        string _prefabName = "SceneAR/" + _sceneName;
            //        Debug.Log("-------------------************************-------------------:" + _prefabName);
            //        if (GlobalUIManager.Instance == null)
            //        {
            //            Debug.LogError("请从Log场景开始加载~");
            //        }
            //        else
            //        {
            //            GlobalUIManager.Instance.StartCoroutine(Load(new string[] { _prefabName }, _Act));
            //        }
            //    }
            //}
            //else
            //{
            //    string _prefabName = "SceneAR/" + _sceneName;
            //    Debug.Log("-------------------************************-------------------:" + _prefabName);
            //    if (GlobalUIManager.Instance == null)
            //    {
            //        Debug.LogError("请从Log场景开始加载~");
            //    }
            //    else
            //    {
            //        GlobalUIManager.Instance.StartCoroutine(Load(new string[] { _prefabName }, _Act));
            //    }
            //}

            // Resources.UnloadUnusedAssets();
            return 0;
        }

        //给加载的预制件添加手动旋转的功能


        static Dictionary<string, GameObject> LoadGameList = new Dictionary<string, GameObject>();

        // 使用协程异步加载 
        static IEnumerator Load(string[] arr, LuaFunction _act)
        {
            foreach (string str in arr)
            {
                bool _boo = false;
                if (LoadGameList.ContainsKey(str))
                {
                    if (LoadGameList[str].gameObject.name == "null")
                    {
                        LoadGameList.Remove(str);
                    }
                    else
                    {
                        UnityEngine.Object.Destroy(LoadGameList[str]);
                        LoadGameList.Remove(str);
                    }
                    _boo = true;
                }

                if (_boo)
                {
                    //所有的都加载完毕后，执行对应的lua脚本//
                    if (_act != null)
                    {
                        _act.Call(LoadGameList[str]);
                    }
                    continue;
                }
                else
                {
                    ResourceRequest rr = Resources.LoadAsync(str);
                    yield return rr;
                    while (rr.progress < 1)
                    {
                        Debug.Log("rr.progress:" + rr.progress);
                        yield return 0;
                    }
                    if (rr.asset == null)
                    {
                        //所有的都加载完毕后，执行对应的lua脚本//
                        if (_act != null)
                        {
                            _act.Call();
                        }
                        yield break;
                    }
                    GameObject _go = UnityEngine.Object.Instantiate(rr.asset) as GameObject;
                    _go.name = rr.asset.name;
                    if (!LoadGameList.ContainsKey(_go.name)) LoadGameList.Add(_go.name, _go);

                    //所有的都加载完毕后，执行对应的lua脚本//
                    if (_act != null)
                    {
                        _act.Call(_go);
                    }
                }
            }

        }


        public static int UnLoadSceneGame(UnityEngine.Object _obj)
        {
            if (LoadGameList.ContainsKey(_obj.name))
            {
                LoadGameList.Remove(_obj.name);
            }
            UnityEngine.Object.Destroy(_obj);
            return 0;
        }

        public static void UnAllLoadSceneGame()
        {
            LoadGameList.Clear();
        }

        public static GameObject LoadPanel(string _name)
        {
            return null;
           // return PanelManager.Instance.LoadPanelByName(_name);
        }
        public static void DestoryPanel(string _name)
        {
           // PanelManager.Instance.ClosePanelByName(_name);
        }

        public static void LogDebug(GameObject _go)
        {
            Debug.Log(_go);

            //_go.GetComponentsInChildren(typeof(LuaFramework.EntityBehaviour));

            //EntityBehaviour[] _list = PanelManager.getCompontent<EntityBehaviour>(_go);

            //EntityBehaviour[] _myList = _go.GetComponentsInChildren<EntityBehaviour>();

            //Debug.LogError("_myList:"+_myList.Length);_go.GetComponent
        }

        public static void LogObj(UnityEngine.Object _go)
        {
            Debug.Log(_go);

        }







        #endregion

        #region /*声音控制*/
        public static void LoadMusic(string _soundName, LuaFunction _callBack)
        {
            //暂时读取的是resource文件夹目录下的音频文件//

            ResourceRequest _req = Resources.LoadAsync<AudioClip>(_soundName);


        }




        #endregion

    }
}
