using UnityEngine;
using System.Collections;
using LuaInterface;
using System;
using System.IO;
using UnityEngine.Networking;

namespace LuaFramework
{
    public class LuaLoader : LuaFileUtils
    {
        void Awake()
        {
//#if UNITY_EDITOR
        //    instance = this;
//#else
             instance = new LuaResLoader(); //this;
//#endif
        }

        public void Restart()
        {
//#if !UNITY_EDITOR
//            instance = this;
//#else
            instance = new LuaResLoader(); //this;
//#endif
        }

        private string[] split = (new string[] { "/", @"\" });
        public static string savePath
        {
            get
            {
                //string _url = "";
                return Application.persistentDataPath;
            }
        }
        //资源下载
        public IEnumerator GetAssetRef<T>(string _url, System.Action<int> _proAct = null,
      System.Action<int> _loadAct = null, System.Action<T> _Act = null, bool _file = false)
        where T : UnityEngine.Object
        {
            yield return 0;
            bool _isLocal = false;
            //1，先判断，当前下载目标是否已经被下载//
            string[] _list = _url.Split(split, StringSplitOptions.None);
            string _objName = _list[_list.Length - 1];
            //--存储的本地路径--
            string _objPath = savePath + "/" + _objName;
            _isLocal = File.Exists(_objPath);
            //2，如果下载，直接使用读取本地AB的方法 就可以
            #region /*本地读取*/
            if (_isLocal)
            {
                if (_proAct != null) _proAct.Invoke(100);
                //创建本地AB包对象//
                AssetBundle _ab = null;
                //文件的读取方式//
                if (_file)
                {
                    //该方式 有可能造成假死状态//
                    byte[] _bt = File.ReadAllBytes(_objPath);
                    yield return null;
                    yield return _bt;
                    try
                    {
                        //从内存读取的方式//
                        _ab = AssetBundle.LoadFromMemory(_bt);
                    }
                    catch (System.Exception _ex)
                    {
                        Debug.LogError(_ex.Message.ToString());
                    }
                }
                else
                {
                    //从内存读取的方式//
                    AssetBundleCreateRequest _create = AssetBundle.LoadFromFileAsync(_objPath);
                    while (!_create.isDone)
                    {
                        yield return null;
                    }
                    yield return _create;
                    _ab = _create.assetBundle;
                }
                yield return null;
                //加载完后，需要实例化ab包中，包含的对象//
                T _dd = default(T);
                if (_ab != null)
                {
                    //不需要后缀名//
                    AssetBundleRequest _request = _ab.LoadAllAssetsAsync(); //_ab.LoadAssetAsync(_objName.Split('.')[0]);

                    #region /*进度UI 显示 加载*/
                    int displayProgress = 0;
                    int toProgress = 0;
                    while (_request.progress < 0.9f)
                    {
                        toProgress = (int)_request.progress * 100;
                        while (displayProgress < toProgress)
                        {
                            ++displayProgress;
                            if (_loadAct != null)
                            {
                                _loadAct(displayProgress);
                            }
                            yield return null;// new WaitForEndOfFrame();
                        }
                        yield return null;
                    }
                    while (!_request.isDone)
                    {
                        toProgress = 99;
                        while (displayProgress < toProgress)
                        {
                            ++displayProgress;
                            if (_loadAct != null)
                            {
                                _loadAct(displayProgress);
                            }
                            yield return null;// new WaitForEndOfFrame();
                        }
                        yield return null;
                    }

                    if (_loadAct != null)
                    {
                        _loadAct(100);
                    }

                    #endregion

                    //可能下载的名称和 包中所包含的资源名称不一致，顾使用该方法//
                   UnityEngine.Object[] _objList = _request.allAssets;
                    for (int i = 0; i < _objList.Length; i++)
                    {
                        if (_objList[i].name.Equals(_objName.Split('.')[0]))
                        {
                            _dd = UnityEngine.Object.Instantiate(_objList[i]) as T;
                            break;
                        }
                    }

                    //需要实例化，不是直接赋值//
                    //_dd = _request.asset as T;
                    if (_dd == null)
                    {
                        if (_request.asset != null)
                        {
                            _dd = UnityEngine.Object.Instantiate(_request.asset) as T;
                        }
                        else
                        {
                            if (_request.allAssets.Length > 1)
                                _dd = UnityEngine.Object.Instantiate(_request.allAssets[0]) as T;
                        }
                    }


                    //加载成功后，回调(需要手动运用成功后，手动释放内存)//
                    if (_Act != null)
                    {
                        _Act(_dd);
                    }
                    //释放对应的内存//
                    _request = null;
                }
                else
                {
                    //_dd = UnityEngine.Object.Instantiate(_ab.mainAsset) as T;
                    if (_Act != null)
                    {
                        _Act(null);
                    }
                }
                _ab.Unload(false);
                _ab = null;
                yield break;
            }
            #endregion

            //3，如果没有被下载时，使用UnityWebRequest 的方法
            else
            {
                if (_url.Length < 3)
                {
                    Debug.LogError("下载地址错误");
                    yield break;
                }
#if UNITY_EDITOR
                Debug.Log(_url);
#endif
                UnityWebRequest _web = new UnityWebRequest(_url);
                //初始化下载//
                _web.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
                _web.SendWebRequest();

                #region /*进度UI 显示 加载*/
                int displayProgress = 0;
                int toProgress = 0;
                while (_web.downloadProgress < 0.9f)
                {
                    toProgress = (int)_web.downloadProgress * 100;
                    while (displayProgress < toProgress)
                    {
                        ++displayProgress;
                        if (_proAct != null)
                        {
                            _proAct(displayProgress);
                        }
                        yield return null;// new WaitForEndOfFrame();
                    }
                    yield return null;
                }
                while (!_web.isDone)
                {
                    toProgress = 99;
                    while (displayProgress < toProgress)
                    {
                        ++displayProgress;
                        if (_proAct != null)
                        {
                            _proAct(displayProgress);
                        }
                        yield return null;// new WaitForEndOfFrame();
                    }
                    yield return null;
                }

                if (_proAct != null)
                {
                    _proAct(100);
                }

                #endregion

                if (_web.downloadHandler.data.Length <= 0)
                {
                    Debug.LogError("下载地址错误！");
                    yield break;
                }
                //4,将下载下来的AB包，保存至，固定目录下，便于再次读取//
                //写入资源//
                File.WriteAllBytes(_objPath, _web.downloadHandler.data);
                yield return null;
                //_web.downloadHandler = null;
                _web.Dispose();
                _web = null;
                yield return null;
                //5,读取下载好的文件//
                //读写入的资源//
                AssetBundleCreateRequest asset = null;
                //根据路径下载对应的ab文件//
                asset = AssetBundle.LoadFromFileAsync(_objPath);

                #region /*进度UI 显示 加载*/
                int displayProgress2 = 0;
                int toProgress2 = 0;
                while (asset.progress < 0.9f)
                {
                    toProgress2 = (int)asset.progress * 100;
                    while (displayProgress2 < toProgress2)
                    {
                        ++displayProgress2;
                        if (_loadAct != null)
                        {
                            _loadAct(displayProgress2);
                        }
                        yield return null;// new WaitForEndOfFrame();
                    }
                    yield return null;
                }
                while (!asset.isDone)
                {
                    toProgress2 = 99;
                    while (displayProgress2 < toProgress2)
                    {
                        ++displayProgress2;
                        if (_loadAct != null)
                        {
                            _loadAct(displayProgress2);
                        }
                        yield return null;// new WaitForEndOfFrame();
                    }
                    yield return null;
                }

                if (_loadAct != null)
                {
                    _loadAct(100);
                }

                #endregion

                //读取下载好的文件//
                T _dd = default(T);
                if (asset.assetBundle != null)
                {
                    {
                        AssetBundleRequest _req = asset.assetBundle.LoadAllAssetsAsync();
                        #region /*进度UI 显示 加载*/
                        int displayProgress3 = 0;
                        int toProgress3 = 0;
                        while (_req.progress < 0.9f)
                        {
                            toProgress3 = (int)asset.progress * 100;
                            while (displayProgress3 < toProgress3)
                            {
                                ++displayProgress3;
                                if (_loadAct != null)
                                {
                                    _loadAct(displayProgress3);
                                }
                                yield return null;// new WaitForEndOfFrame();
                            }
                            yield return null;
                        }
                        while (!_req.isDone)
                        {
                            toProgress3 = 99;
                            while (displayProgress3 < toProgress3)
                            {
                                ++displayProgress3;
                                if (_loadAct != null)
                                {
                                    _loadAct(displayProgress3);
                                }
                                yield return null;// new WaitForEndOfFrame();
                            }
                            yield return null;
                        }

                        if (_loadAct != null)
                        {
                            _loadAct(100);
                        }

                        #endregion
                        Debug.LogError("_req.allAssets:" + _req.allAssets);

                        for (int i = 0; i < _req.allAssets.Length; i++)
                        {
                            if (_req.allAssets[i].name.Equals(_objName.Split('.')[0]))
                            {
                                _dd =UnityEngine.Object.Instantiate(_req.allAssets[i]) as T;
                                break;
                            }
                        }
                        if (_dd == null)
                        {
                            if (_req.allAssets.Length >= 1)
                                _dd = UnityEngine.Object.Instantiate(_req.allAssets[0]) as T;
                        }
                    }
                }

                //回调//
                if (_Act != null)
                {
                    _Act(_dd);
                }
                asset.assetBundle.Unload(false);
                asset = null;
                yield break;
            }
        }

    }
}
