using UnityEngine;
using System.Collections;
using LuaInterface;
using System.IO;
using System.Collections.Generic;

namespace LuaFramework
{
    public class LuaManager : Manager
    {
        protected LuaState m_LuaState;
        protected LuaLoader m_LuaLoader;
        protected LuaLooper m_LuaLooper = null;

        protected override void Init()
        {
            base.Init();

            m_LuaLoader = new LuaLoader();
            //初始化路径问题
            getResourcePath();

            Reload();

        }

        public void Reload()
        {
            Debug.LogError("00_Reload");
            if (m_LuaState != null)
            {
                m_LuaState.Dispose();
                m_LuaState = null;
            }

            if (m_LuaLooper != null)
                m_LuaLooper.Destroy();

            m_LuaState = new LuaState();
            this.OpenLibs();
            m_LuaState.LuaSetTop(0);

            LuaBinder.Bind(m_LuaState);
            LuaCoroutine.Register(m_LuaState, this);

            m_LuaLoader.Restart();

            m_LuaState.AddSearchPath(Application.dataPath + "/Lua");


        }

        public void InitStart()
        {
            this.InitLuaPath();
            this.InitLuaBundle();
            m_LuaState.Start();
            //this.StartMain();
            this.StartLooper();
        }

        public void DoFile2(string filename)
        {
            //if (LuaConst.USE_AB&& ResourceManager.ScriptAB == null)
            //{
            //    //避免 切换场景时 直接 //
            //    GlobalUIManager.Instance.StartCoroutine(ResourceManager.StartAB(() =>
            //    {
            //        m_LuaState.DoFile(filename);
            //    }));
            //}
            //else
            {
                m_LuaState.DoFile(filename);
            }
        }

        /// <summary>
        /// 多次调用时，只会执行第一次的载入//
        /// 如果需要重新载入，请执行m_LuaState.ReLoad  luaManager.Reload();  建议不熟悉的话，不要调用
        /// </summary>
        /// <param name="filename"></param>
        public void DoFile(string filename)
        {
            //避免已经添加了路径的脚本，报错//
            if (filename.EndsWith(".lua"))
            {
                if (luaResPathDic.ContainsKey(filename))
                {
                    m_LuaState.DoFile(luaResPathDic[filename]);//如果是完整的路径，是查询不到的
                }
                else
                {
                    m_LuaState.DoFile(filename);
                }
            }
            else
            {
                m_LuaState.DoFile(SearchLuaPath(filename));
            }
        }

        public string SearchLuaPath(string luaName)
        {
            //本地Lua脚本的位置//luaDir
            if (LuaConst.USE_AB == false)
            {
                //检索的文件夹//
                return findPath(luaName);
            }
            else
            {
                return luaName + ".lua";
            }
        }
#if UNITY_EDITOR_
        List<string> _paths = new List<string>() { LuaConst.luaDir, Application.dataPath + "/Lua", LuaConst.toluaDir };
#else
        List<string> _paths = new List<string>() { };
#endif
        private string findPath(string luaName)
        {
            //检索的文件夹//
            string _DirPath = LuaConst.luaDir;

            // List<string> _paths = new List<string>() { LuaConst.luaDir, Application.dataPath + "/Lua", LuaConst.toluaDir };
            for (int m = 0; m < _paths.Count; m++)
            {
                _DirPath = _paths[m];

                DirectoryInfo dir = new DirectoryInfo(_DirPath);
                DirectoryInfo[] _dirs = dir.GetDirectories();
                for (int i = 0; i < _dirs.Length; i++)
                {
                    string _name = getFileByDir(_dirs[i], luaName + ".lua");
                    if (_name.Length > 0)
                    {
                        return _name;
                    }
                }
            }
            //在发布手机上，非AB包时，执行
            string _luaPath = luaName + ".lua";
            if (luaResPathDic.ContainsKey(_luaPath))
            {
                Debug.LogError("luaResPathDic[_luaPath]:" + luaResPathDic[_luaPath]);
                return luaResPathDic[_luaPath];
            }
            return luaName + ".lua";//防止在发布手机上时，找不到脚本
        }

        private Dictionary<string, string> luaResPathDic = new Dictionary<string, string>();
        private string[] split = (new string[] { "|", @"|" });
        //读取resource下的路径//
        public void getResourcePath()
        {
            if (luaResPathDic.Count > 0)
                return;
            TextAsset _infoList = Resources.Load("luaList") as TextAsset;
            if (_infoList != null)
            {
                string[] _list = _infoList.text.Trim().Split(new string[] { "\n" }, System.StringSplitOptions.None);
                for (int i = 0; i < _list.Length; i++)
                {
                    string[] _count = _list[i].Split(split, System.StringSplitOptions.None);
                    string _key = _count[0];
                    //print(_key);
                    string _value = _count[1].Trim();
                    if (!luaResPathDic.ContainsKey(_key))
                    {
                        luaResPathDic.Add(_key, _value);
                    }
                }
            }
        }

        private string getFileByDir(DirectoryInfo dir, string _luaName)
        {
            FileInfo[] fi = dir.GetFiles();
            for (int i = 0; i < fi.Length; i++)
            {
                if (!fi[i].Name.EndsWith(".lua"))
                    continue;
                else
                {
                    if (fi[i].Name.Equals(_luaName))
                    {
                        return fi[i].FullName;
                    }
                }
            }

            DirectoryInfo[] _dirs = dir.GetDirectories();
            for (int i = 0; i < _dirs.Length; i++)
            {
                string _name = getFileByDir(_dirs[i], _luaName + ".lua");
                if (_name.Length > 0)
                {
                    return _name;
                }
            }

            return "";
        }


        /// <summary>
        /// 由于升级lua，所以之前的版本稍作修改//
        /// args 参数列表，识别有误//
        /// --by zhouzhongnian 20190820--
        /// //获取的参数是列表，需要在Lua脚本中，使用数组来获得对应的参数呢
        /// </summary>
        /// <param name="name"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        public object[] CallFunction(string name, params object[] args)
        {
#if !UNITY_2018
            LuaFunction func = m_LuaState.GetFunction(name);
            if (func != null)
            {
               // for (int i = 0; i < args.Length; i++)
                {
                    func.Call(args);
                }
            }
            return null;
#else
            LuaFunction func = m_LuaState.GetFunction(name);
            if (func != null)
            {
                //根据参数的数量，设置call调用的泛型参数列表(func.Call只有)//
                // object[] argObj = new object[args.Length];
                // for (int i = 0; i < args.Length; i++)
                {
                    //func.Call(args);
                    setCallArgs(func, args);
                }
            }
            return null;
#endif
        }

        /// <summary>
        /// 识别不了参数列表，提供不了泛型参数//
        /// </summary>
        /// <param name="func"></param>
        /// <param name="args"></param>
        private void setCallArgs(LuaFunction func, params object[] args)
        {
            int argsLength = args.Length;
            switch (argsLength)
            {
                case 0:
                default:
                    Debug.Log("CallArgs is null !");
                    break;
                case 1:
                    func.Call(args[0]);
                    break;
                case 2:
                    func.Call(args[0], args[1]);
                    break;
                case 3:
                    func.Call(args[0], args[1], args[2]);
                    break;
                case 4:
                    func.Call(args[0], args[1], args[2], args[3]);
                    break;
                case 5:
                    func.Call(args[0], args[1], args[2], args[3], args[4]);
                    break;
                case 6:
                    func.Call(args[0], args[1], args[2], args[3], args[4], args[5]);
                    break;
                case 7:
                    func.Call(args[0], args[1], args[2], args[3], args[4], args[5], args[6]);
                    break;
                case 8:
                    func.Call(args[0], args[1], args[2], args[3], args[4], args[5], args[6], args[7]);
                    break;
                case 9:
                    func.Call(args[0], args[1], args[2], args[3], args[4], args[5], args[6], args[7], args[8]);
                    break;
            }
        }


        protected void InitLuaPath()
        {
            //            if (LuaConst.DEBUG_MODE)
            //            {
            //                m_LuaState.AddSearchPath(Globe.SCRIPTS_ROOT + "/Lua");
            //                m_LuaState.AddSearchPath(Globe.SCRIPTS_ROOT + "/ToLua/Lua");
            //            }
            //            else
            //            {
            //                //m_LuaState.AddSearchPath(Util.DataPath() + "lua");
            //#if UNITY_EDITOR
            //                //避免编辑模式下，直接报错//
            //                m_LuaState.AddSearchPath(Globe.SCRIPTS_ROOT + "/Lua");
            //                m_LuaState.AddSearchPath(Globe.SCRIPTS_ROOT + "/ToLua/Lua");

            //                //将lua文件拷贝至指定的文件夹下(在正式运行的情况下)//
            //                string _path = Util.DataPath();
            //#else
            //                m_LuaState.AddSearchPath(Util.DataPath() + "Lua");
            //#endif
            //            }
        }

        protected void InitLuaBundle()
        {
            // m_LuaLoader.onLoadScriptFunction = resourceManager.LoadScript;
        }

        void StartLooper()
        {
            if (m_LuaLooper == null)
                m_LuaLooper = this.gameObject.AddComponent<LuaLooper>();
            //m_LuaLooper.state = m_LuaState;
            m_LuaLooper.luaState = m_LuaState;
        }

        /// <summary>
        /// 初始化加载第三方库
        /// </summary>
        protected void OpenLibs()
        {
            m_LuaState.OpenLibs(LuaDLL.luaopen_pb);

            //m_LuaState.OpenLibs(LuaDLL.luaopen_sproto_core);
            //m_LuaState.OpenLibs(LuaDLL.luaopen_protobuf_c);


            m_LuaState.OpenLibs(LuaDLL.luaopen_lpeg);
            m_LuaState.OpenLibs(LuaDLL.luaopen_bit);
            m_LuaState.OpenLibs(LuaDLL.luaopen_socket_core);

            this.OpenCJson();
        }

        protected void OpenCJson()
        {
            m_LuaState.LuaGetField(LuaIndexes.LUA_REGISTRYINDEX, "_LOADED");
            m_LuaState.OpenLibs(LuaDLL.luaopen_cjson);
            m_LuaState.LuaSetField(-2, "cjson");

            m_LuaState.OpenLibs(LuaDLL.luaopen_cjson_safe);
            m_LuaState.LuaSetField(-2, "cjson.safe");
        }
    }
}
