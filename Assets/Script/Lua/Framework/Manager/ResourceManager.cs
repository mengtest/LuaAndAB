using UnityEngine;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System;
using Object = UnityEngine.Object;
using UnityEngine.Networking;
using LuaInterface;

namespace LuaFramework
{
    public class ResourceManager : Manager
    {
        public static AssetBundle ScriptAB;

        public static Dictionary<string, byte[]> m_script = new Dictionary<string, byte[]>();

        /// <summary>
        /// 读取AB包的协成,便于按照顺序执行//
        /// 在执行玩第一个后，执行第二个//
        /// </summary>
        public static Dictionary<string, IEnumerator> m_ABIE = new Dictionary<string, IEnumerator>();


        public enum Type
        {
            Config = 0,
            Sound,
            Script,
            Resource,
        }

        protected static readonly string CONFIG_PATH = "Config/";
        protected static readonly string SCRIPT_PATH = "Lua/";
        protected static readonly string ASSETS_PATH = "Z-Resource/";
        /// <summary>
        /// 总的lua脚本名称是固定的--暂时不会改变//
        /// </summary>
        protected readonly string[] ASSETS_FILE = { "lua.data" };// { "Scripts.data" };
        protected static readonly Type[] ASSETS_TYPE = { Type.Script };

        protected Dictionary<Type, Stack<AssetBundle>> m_AssetBundles = new Dictionary<Type, Stack<AssetBundle>>();

        /// <summary>
        /// 在程序启动时，调用 ---先下载，之后 进入场景后，不用继续下载 Lua脚本文件//
        /// </summary>
        protected override void Init()
        {

            base.Init();

            //基础方法绑定//
            /************************************初始化（程序运行时就必须需要的）需要的资源********************************************************/
            //1,配置文件//
          //  BlackHouse.CSVLoader.onLoadConfig = this.LoadConfig;//初始化配置表数据 --根据配置从网络读取//
            //2,声音文件//
            SoundManager.onLoadSound = this.LoadSound;//初始化声音文件  --根据配置从网络读取//
            //3.加载lua脚本文件//
            //if (LuaConst.USE_ASSETS)
            //    this.StartCoroutine(this.StartupLoadAssets());
            //else
            //    this.StartCoroutine(this.StartupFinishUpdate());
            if (LuaConst.USE_AB)
            {
                if (ScriptAB == null)
                {
                    StartCoroutine(StartAB(() =>
                    {
                        this.StartCoroutine(this.StartupFinishUpdate());
                    }));
                }
                else
                {
                    this.StartCoroutine(this.StartupFinishUpdate());
                }
            }
            else
            {
                this.StartCoroutine(this.StartupFinishUpdate());
            }


            //4.初始化预制件 --必须等待Lua脚本加载完成后执行 --暂时无//  //场景中的，或者模块化的UI部分 放在调用时下载//

            /************************************初始化需要的资源******************************************************************************************************/
        }

        protected override void Term()
        {
            //Instance = null;

            base.Term();
        }

        public void LoadAsset(int type, string assetPath, System.Action callback)
        {
            //if (LuaConst.USE_ASSETS)
            //{
            //    this.StartCoroutine(this.LoadAssetBundleAsync((ResourceManager.Type)type, assetPath, callback));
            //    return;
            //}

            if (callback != null)
                callback();
        }

        public void UnloadAsset(int type)
        {
            if (!m_AssetBundles.ContainsKey((ResourceManager.Type)(type)))
                return;

            Stack<AssetBundle> stack = m_AssetBundles[(ResourceManager.Type)(type)];
            if (stack.Count != 0)
            {
                AssetBundle assetBundle = stack.Pop();
                assetBundle.Unload(false);
                assetBundle = null;
            }
        }

        public TextAsset LoadConfig(string filename)
        {
            if (LuaConst.USE_AB)
            {
                filename = filename.Replace('/', '_');

                TextAsset textAsset = null;

                //AssetBundle assetBundle = this.Get(ResourceManager.Type.Config);
                //TextAsset textAsset = assetBundle.LoadAsset<TextAsset>(filename);

                if (textAsset == null)
                {
                    //如果为空，使用最新的读表方式-- 服务器的id方式读取配置表文件//
                    //在程序运行时，就开始下载下来咯//
                    //if()
                    //if (ABServerManager.Instance.m_myABList.ContainsKey(ABServerManager.ConfigABID))
                    //{
                    //    if (AssetbundleEngine.instance.isLocal(ABServerManager.Instance.m_myABList[ABServerManager.ConfigABID].downloadUrl))
                    //    {
                    //        return AssetbundleEngine.instance.LoadLocalAB<TextAsset>(ABServerManager.Instance.m_myABList[ABServerManager.ConfigABID].downloadUrl, filename);
                    //    }
                    //}

                }
                else
                {

                }
                return textAsset;
            }

            return Resources.Load<TextAsset>(CONFIG_PATH + filename);
        }

        public AudioClip LoadSound(string filename)
        {
            // if (LuaConst.USE_ASSETS)
            // {
            //     filename = filename.Replace('/', '_');
            //     AssetBundle assetBundle = this.Get(ResourceManager.Type.Sound);
            //     AudioClip audioClip = assetBundle.LoadAsset<AudioClip>(filename);
            //     return audioClip;
            // }

            ////使用最新的加载音乐//
            //if(LuaConst.USE_AB)
            //{
            //    //判断ABID//





            //}



            // string file = Globe.SOUND_PATH + filename;
            string file = "";
             int local = file.LastIndexOf('.');
            if (local != -1) file = file.Substring(0, local);
            return Resources.Load<AudioClip>(file);
        }

        public TextAsset LoadScript(string filename)
        {
            //if (LuaConst.USE_ASSETS)
            //{
            //    filename = filename.Replace('/', '_');
            //    AssetBundle assetBundle = this.Get(ResourceManager.Type.Script);
            //    TextAsset textAsset = assetBundle.LoadAsset<TextAsset>(filename);
            //    return textAsset;
            //}

            return Resources.Load<TextAsset>(SCRIPT_PATH + filename);
        }

        public void LoadScene(string asset, string name, System.Action<GameObject> callback)
        {
            //if (LuaConst.USE_ASSETS)
            //{
            //    this.StartCoroutine(this.LoadSceneAsync(asset, name, callback));
            //    return;
            //}

            if (callback != null)
                callback(null);
        }

        public Object LoadResource(string filename, System.Type type)
        {
            //if (LuaConst.USE_ASSETS)
            //{
            //    filename = filename.Replace('/', '_');
            //    AssetBundle assetBundle = this.Get(ResourceManager.Type.Resource);
            //    Object obj = assetBundle.LoadAsset(filename, type); //assetBundle.LoadAsset(filename);//assetBundle.LoadAsset(filename, type);
            //    return obj;
            //}

            return Resources.Load(ASSETS_PATH + filename);
            //return Resources.Load(ASSETS_PATH + filename, type);
        }

        protected void Push(ResourceManager.Type type, AssetBundle asset)
        {
            if (asset != null)
            {
                if (!m_AssetBundles.ContainsKey(type))
                    m_AssetBundles.Add(type, new Stack<AssetBundle>());
                m_AssetBundles[type].Push(asset);
            }
        }

        protected void Pop(ResourceManager.Type type)
        {
            if (m_AssetBundles.ContainsKey(type))
                m_AssetBundles[type].Pop();
        }

        protected AssetBundle Get(ResourceManager.Type type)
        {
            if (m_AssetBundles.ContainsKey(type))
                return m_AssetBundles[type].Peek();
            return null;
        }

        protected IEnumerator StartupLoadAssets()
        {
            //加载资源
            for (int i = 0; i < this.ASSETS_FILE.Length; i++)
            {
                bool isLoaded = false;
                this.StartCoroutine(this.LoadAssetBundleAsync(ResourceManager.ASSETS_TYPE[i], this.ASSETS_FILE[i], () => { isLoaded = true; }));
                while (!isLoaded) yield return 0;
            }

            this.StartCoroutine(this.StartupFinishUpdate());
        }

        public static string[] split = (new string[] { "/", @"\" });
        /// <summary>
        /// 程序启动时，运行(logo)//
        /// </summary>
        /// <returns></returns>
        public static IEnumerator StartAB(System.Action _callback)
        {
            //            if (ABServerManager.Instance.m_myABList.ContainsKey(ABServerManager.ScriptABID))
            //            {
            //                if (m_script.Count > 0)
            //                {
            //                    if (_callback != null)
            //                    {
            //                        _callback();
            //                    }
            //                    yield break;
            //                }

            //                string _url = ABServerManager.Instance.m_myABList[ABServerManager.ScriptABID].downloadUrl;

            //                string[] _list = _url.Split(split, StringSplitOptions.None);
            //                string _name = _list[_list.Length - 1];

            //                //if (!_name.EndsWith(".lua"))
            //                //    _name += ".lua";

            //                #region /!streamingAssetsPath/
            //                string file = Application.persistentDataPath + "/" + _name;  //Util.AppContentPath() + assetPath;

            //                AssetBundleCreateRequest assetRequest = null;
            //                assetRequest = AssetBundle.LoadFromFileAsync(file);
            //                if (!assetRequest.isDone)
            //                {
            //                    // AssetBundleCreateRequest assetRequest = AssetBundle.LoadFromMemoryAsync(www.bytes);
            //                    yield return assetRequest;
            //                }
            //                ScriptAB = assetRequest.assetBundle;
            //                #endregion

            //#if UNITY_EDITOR && ZZL
            //                #region /streamingAssetsPath/

            //                 file = Application.streamingAssetsPath + "/ScriptAB/" + "resources_lua.data";  //Util.AppContentPath() + assetPath;

            //                 assetRequest = AssetBundle.LoadFromFileAsync(file);
            //                if (!assetRequest.isDone)
            //                {
            //                    // AssetBundleCreateRequest assetRequest = AssetBundle.LoadFromMemoryAsync(www.bytes);
            //                    yield return assetRequest;
            //                }
            //                ScriptAB = assetRequest.assetBundle;

            //                #endregion
            //#endif




            //                int ii = 0;
            //                //读取／／
            //                UnityEngine.Object[] _objList = ScriptAB.LoadAllAssets();
            //                for (int i = 0; i < _objList.Length; i++)
            //                {
            //                    string _Keyname = _objList[i].name;
            //                    if (!m_script.ContainsKey(_Keyname))
            //                    {
            //                        // Debug.LogError(_Keyname);
            //                        byte[] _byteList = UnityEngine.Object.Instantiate(_objList[i] as TextAsset).bytes;// (_objList[i] as TextAsset).bytes;
            //                        m_script.Add(_Keyname, _byteList);
            //                    }
            //                    else
            //                    {
            //                        m_script[_Keyname] = UnityEngine.Object.Instantiate(_objList[i] as TextAsset).bytes;
            //                    }

            //                    //AppFacade.instance.GetManager<LuaManager>(ManagerName.LUA).DoFile2(_Keyname);

            //                    ii++;
            //                    Resources.UnloadAsset(_objList[i]);
            //                    if (ii > 10)
            //                    {
            //                        ii = 0;
            //                        yield return null;
            //                    }


            //                }

            //                yield return 0;
            //                ScriptAB.Unload(false);

            //                assetRequest = null;
            //            }

            yield return null;
            if (_callback != null)
            {
                _callback();
            }
        }


        protected IEnumerator StartupFinishUpdate()
        {
            yield return 0;
            facade.SendMessageCommand(NotifyCommand.MAIN_RESOURCE_UPDATE_FINISH);
        }

        protected IEnumerator LoadAssetBundleAsync(ResourceManager.Type type, string assetPath, System.Action callback)
        {
            AssetBundle assetBundle = null;
            string file = Application.persistentDataPath + "/" + assetPath;  //Util.AppContentPath() + assetPath;
            if (true)
            {
                // WWW www = new WWW(file);
                // yield return www;
                AssetBundleCreateRequest assetRequest = AssetBundle.LoadFromFileAsync(file);
                if (!assetRequest.isDone)
                {
                    // AssetBundleCreateRequest assetRequest = AssetBundle.LoadFromMemoryAsync(www.bytes);
                    yield return assetRequest;
                }
                assetBundle = assetRequest.assetBundle;
            }
            else
            {
                if (File.Exists(file))
                {
                    FileStream fileStream = new FileStream(file, FileMode.Open, FileAccess.Read);
                    if (fileStream.Length != 0)
                    {
                        byte[] bufferData = new byte[fileStream.Length];
                        fileStream.Read(bufferData, 0, bufferData.Length);

                        if (bufferData != null && bufferData.Length != 0)
                        {
                            AssetBundleCreateRequest assetRequest = AssetBundle.LoadFromMemoryAsync(bufferData);
                            yield return assetRequest;
                            assetBundle = assetRequest.assetBundle;
                        }
                    }
                }
            }

            if (assetBundle != null)
                this.Push(type, assetBundle);

            if (callback != null)
                callback();
        }

        protected IEnumerator LoadSceneAsync(string asset, string name, System.Action<GameObject> callback)
        {
            GameObject sceneObject = null;
            AssetBundle assetBundle = null;
            string file = "";// Util.AppContentPath() + asset;
            if (true)
            {

                AssetBundleCreateRequest assetRequest = AssetBundle.LoadFromFileAsync(file);
                if (!assetRequest.isDone)
                {
                    // AssetBundleCreateRequest assetRequest = AssetBundle.LoadFromMemoryAsync(www.bytes);
                    yield return assetRequest;
                }
                assetBundle = assetRequest.assetBundle;
            }
            else
            {
                if (File.Exists(file))
                {
                    FileStream fileStream = new FileStream(file, FileMode.Open, FileAccess.Read);
                    if (fileStream.Length != 0)
                    {
                        byte[] bufferData = new byte[fileStream.Length];
                        fileStream.Read(bufferData, 0, bufferData.Length);
                        if (bufferData != null && bufferData.Length != 0)
                        {
                            AssetBundleCreateRequest assetRequest = AssetBundle.LoadFromMemoryAsync(bufferData);
                            yield return assetRequest;
                            assetBundle = assetRequest.assetBundle;
                        }
                    }
                }
            }

            if (assetBundle != null)
            {
                Object scenePrefab = assetBundle.LoadAsset(name);
                if (scenePrefab != null) sceneObject = GameObject.Instantiate(scenePrefab) as GameObject;
                assetBundle.Unload(false);
            }

            //
            if (callback != null)
                callback(sceneObject);
        }




        #region /*协成管理器*/
        public void AddABIE(string _name, IEnumerator _ieAct)
        {
            if (!m_ABIE.ContainsKey(_name))
            {
                m_ABIE.Add(_name, _ieAct);
            }

            if (m_ABIE.Count == 1)
            {
                Exec(_name);
                //GlobalUIManager.Instance.StartCoroutine(Exec(_name));
            }
        }

        public void Exec(string _IEName)
        {
           // GlobalUIManager.Instance.StartCoroutine(ExecIE(_IEName));
        }

        private IEnumerator ExecIE(string _IEName)
        {
            yield return null;


            //执行下一个协成//
            if (m_ABIE.Count > 0)
            {
                string _ieKey = "";
                foreach (var item in m_ABIE.Keys)
                {
                    if (_ieKey.Length <= 0)
                    {
                        _ieKey = item;
                        break;
                    }
                }
               // GlobalUIManager.Instance.StartCoroutine(m_ABIE[_ieKey]);
                ///执行完毕后，需要手动删除 协成队列中的成员//
                if (m_ABIE.ContainsKey(_IEName)) m_ABIE.Remove(_IEName);
            }


        }


        public IEnumerator LoadRes(string _IEName, System.Type type, System.Action<Object> _callBack)
        {
            yield return null;

            Object _obj = Resources.Load(_IEName, type);
            if (_obj != null)
            {
                if (_callBack != null)
                {
                    _callBack.Invoke(_obj);
                }
            }
            else
            {
                ResourceRequest _req = Resources.LoadAsync(_IEName, type);
                while (!_req.isDone)
                {
                    yield return null;
                }
                //if(Z-Resource)
                if (_req.asset == null)
                {
                    _req = null;
                    yield return null;
                    _req = Resources.LoadAsync("Z-Resource/" + _IEName, type);
                    while (!_req.isDone)
                    {
                        yield return null;
                    }
                }
                if (_req.asset != null)
                {
                    _obj = Object.Instantiate(_req.asset);
                    yield return null;
                    if (_callBack != null)
                    {
                        _callBack.Invoke(_obj);
                    }
                }
                else
                {

                    if (_callBack != null)
                    {
                        _callBack.Invoke(null);
                    }
                }
            }
            //执行从数组中删除的操作//
            StartCoroutine(ExecIE(_IEName));
        }

        public IEnumerator LoadAB(string _assetName, string _IEName, System.Type type, System.Action<Object> _callBack)
        {
            yield return null;
            //获取名字(服务器的文件名字，md5处理过和原先的资源名不一样)//
            string[] _list = _IEName.Split(split, StringSplitOptions.None);
            string _objName = _list[_list.Length - 1];
            //去掉后缀名//
            string _name = _assetName.Split(split, StringSplitOptions.None)[_assetName.Split(split, StringSplitOptions.None).Length - 1].Split('.')[0]; ; // _objName.Split('.')[0];
            print("_name:" + _name);
            string _objPath = Application.persistentDataPath + "/" + _objName;
            bool _isLocal = File.Exists(_objPath);

            if (_isLocal)
            {

            }
            else
            {
                UnityWebRequest _req = new UnityWebRequest(_IEName);
                _req.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
                _req.SendWebRequest();
                while (!_req.isDone)
                {
                    yield return null;
                }
                File.WriteAllBytes(_objPath, _req.downloadHandler.data);
                yield return null;
                _req.Abort();
                _req.Dispose();
            }


            //读写入的资源//
            AssetBundleCreateRequest asset = null;
            //根据路径下载对应的ab文件//
            asset = AssetBundle.LoadFromFileAsync(_objPath);
            while (!asset.isDone)
            {
                yield return null;
            }

            Object _obj = null;
            if (asset.assetBundle != null)
            {
                AssetBundleRequest _abReq = asset.assetBundle.LoadAllAssetsAsync();
                while (!_abReq.isDone)
                {
                    yield return null;
                }
                Object[] _objList = _abReq.allAssets;
                for (int i = 0; i < _objList.Length; i++)
                {
                    if (_objList[i].name == _name)
                    {
                        _obj = _objList[i];// Object.Instantiate(_objList[i]);
                        break;
                    }
                }
                if (_obj != null)
                {
                    if (_callBack != null)
                    {
                        _callBack.Invoke(_obj);
                    }
                }
                asset.assetBundle.Unload(false);
                asset = null;
            }

            yield return null;

            //执行从数组中删除的操作//
            StartCoroutine(ExecIE(_IEName));
        }


        /// <summary>
        /// C# 外部调用
        /// Lua 使用Util调用LoadAB 的方法调用
        /// </summary>
        /// <param name="name"></param>
        /// <param name="type"></param>
        /// <param name="_callBack"></param>
        /// <param name="_ABID"></param>
        /// <returns></returns>
        public UnityEngine.Object LoadObj(string name, System.Type type, System.Action<UnityEngine.Object> _callBack, string _ABID)
        {
            if (LuaConst.USE_AB == true)
            {
                if (resourceManager != null)
                {
                    string _url = "";
                    if (_ABID.Length <= 0)
                    {
                        if (!name.EndsWith(".data"))
                        {
                            name += ".data";
                        }

                        string[] _list = name.Split(ResourceManager.split, StringSplitOptions.None);
                        string _objName = _list[_list.Length - 1];
                        _url = "file:///" + Application.streamingAssetsPath + "/SceneAB/" + _objName.ToLower();
                    }
                    else
                    {
                        int _abid = int.Parse(_ABID);
                        //if (ABServerManager.Instance.m_myABList.ContainsKey(_abid))
                        //{
                        //    _url = ABServerManager.Instance.m_myABList[_abid].downloadUrl;
                        //}
                    }

                    Debug.LogError(_url);
                    resourceManager.AddABIE(name, resourceManager.LoadAB(name, _url, type, (_obj) =>
                    {
                        if (_callBack != null)
                        {
                            _callBack.Invoke(_obj);
                        }
                    })
                    );
                }
            }
            else
            {
                ResourceManager resourceManager = AppFacade.instance.GetManager<ResourceManager>(ManagerName.RESOURCE);
                if (resourceManager != null)
                {
                    resourceManager.AddABIE(name, resourceManager.LoadRes(name, type, (_obj) =>
                    {
                        if (_callBack != null)
                        {
                            _callBack.Invoke(_obj);
                        }
                    })
                    );
                }
            }
            return null;
        }
        #endregion

        #region AB
        public IEnumerator GetAB(string _IEName, string _assetName, System.Action<Object[]> _callBack)
        {
            yield return null;
            //获取名字(服务器的文件名字，md5处理过和原先的资源名不一样)//
            string[] _list = _IEName.Split(split, StringSplitOptions.None);
            string _objName = _list[_list.Length - 1];
            //去掉后缀名//
            string _name = _assetName.Split(split, StringSplitOptions.None)[_assetName.Split(split, StringSplitOptions.None).Length - 1].Split('.')[0]; ; // _objName.Split('.')[0];
            print("_name:" + _name);
            string _objPath = Application.persistentDataPath + "/" + _objName;
            bool _isLocal = File.Exists(_objPath);

            if (_isLocal)
            {

            }
            else
            {
                UnityWebRequest _req = new UnityWebRequest(_IEName);
                _req.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
                _req.SendWebRequest();
                while (!_req.isDone)
                {
                    yield return null;
                }
                File.WriteAllBytes(_objPath, _req.downloadHandler.data);
                yield return null;
                _req.Abort();
                _req.Dispose();
            }


            //读写入的资源//
            AssetBundleCreateRequest asset = null;
            //根据路径下载对应的ab文件//
            asset = AssetBundle.LoadFromFileAsync(_objPath);
            while (!asset.isDone)
            {
                yield return null;
            }

            Object _obj = null;
            if (asset.assetBundle != null)
            {
                AssetBundleRequest _abReq = asset.assetBundle.LoadAllAssetsAsync();
                while (!_abReq.isDone)
                {
                    yield return null;
                }
                Object[] _objList = _abReq.allAssets;

                if (_obj != null)
                {
                    if (_callBack != null)
                    {
                        _callBack.Invoke(_objList);
                    }
                }
                asset.assetBundle.Unload(false);
                asset = null;
            }

            yield return null;
        }

        public IEnumerator GetABToLua(string _IEName, string _assetName, LuaFunction _callBack)
        {
            yield return null;
            //获取名字(服务器的文件名字，md5处理过和原先的资源名不一样)//
            string[] _list = _IEName.Split(split, StringSplitOptions.None);
            string _objName = _list[_list.Length - 1];
            //去掉后缀名//
            //string _name = _assetName.Split(split, StringSplitOptions.None)[_assetName.Split(split, StringSplitOptions.None).Length - 1].Split('.')[0]; ; // _objName.Split('.')[0];
            //print("_name:" + _name);
            string _objPath = Application.persistentDataPath + "/" + _objName;
            bool _isLocal = File.Exists(_objPath);

            if (_isLocal)
            {

            }
            else
            {
                UnityWebRequest _req = new UnityWebRequest(_IEName);
                _req.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
                _req.SendWebRequest();
                while (!_req.isDone)
                {
                    yield return null;
                }
                File.WriteAllBytes(_objPath, _req.downloadHandler.data);
                yield return null;
                _req.Abort();
                _req.Dispose();
            }


            //读写入的资源//
            AssetBundleCreateRequest asset = null;
            //根据路径下载对应的ab文件//
            asset = AssetBundle.LoadFromFileAsync(_objPath);
            while (!asset.isDone)
            {
                yield return null;
            }

            Object _obj = null;
            if (asset.assetBundle != null)
            {
                //AssetBundleRequest _abReq = asset.assetBundle.LoadAllAssetsAsync();
                //int _count = 0;
                //while (!_abReq.isDone)
                //{
                //    _count++;
                //    if (_count > 20)
                //    {
                //        _count = 0;
                //        yield return null;
                //    }
                //}
                //Object[] _objList = _abReq.allAssets;

                Object[] _objList = asset.assetBundle.LoadAllAssets();

                if (_objList != null)
                {
                    if (_callBack != null)
                    {
                        _callBack.Call(_objList);
                    }
                }
                if (asset != null && asset.assetBundle != null) asset.assetBundle.Unload(false);
                yield return null;
                asset = null;
            }

            yield return null;
        }

        #endregion


    }

}
