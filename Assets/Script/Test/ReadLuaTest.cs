using LuaInterface;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;

public class ReadLuaTest : MonoBehaviour
{

    [SerializeField]
    private LuaItemTest m_item;

    // Start is called before the first frame update
    void Start()
    {
        CreateItem("获取Lua的AB包内容", () =>
        {
            Debug.Log("获取Lua的AB包内容");
            readAB();
        });

        CreateItem("读取 define", () =>
        {
            Debug.Log("define");

            byte[] _bts = getLua("define");
            Debug.Log(_bts.Length);

        });
        CreateItem("创建Lua的C#脚本", () =>
        {
            Debug.LogError("3--------------------------");
            MainBehaviourLua _luax = (new GameObject("_luax")).AddComponent<MainBehaviourLua>();
            _luax.Set("LuaTest");
        });

        CreateItem("创建Lua脚本,获取组件", () =>
        {
            //Debug.Log("3");
            MainBehaviourLua _luax = (new GameObject("_luax")).AddComponent<MainBehaviourLua>();
            _luax.Set("LuaTest");
        });


        CreateItem("创建Lua脚本,父子关系的体现", () =>
        {
            //Debug.Log("3");
            MainBehaviourLua _luax = (new GameObject("_luax")).AddComponent<MainBehaviourLua>();
            _luax.Set("LuaTest");//这里填写的是Lua脚本的名字就可以咯，（本地脚本使用脚本查找方式，找到完整路径下的脚本，如果是AB包中，直接传入名称即可）
        });

        m_item.gameObject.SetActive(false);
    }


    void CreateItem(string _text, System.Action _click)
    {
        LuaItemTest _item = LuaItemTest.Instantiate(m_item) as LuaItemTest;
        _item.transform.parent = m_item.transform.parent;
        _item.Set(_text, _click);
    }

    // Update is called once per frame
    void Update()
    {

    }

    void readAB()
    {
        StartCoroutine(readABIE());

    }

    // public static Dictionary<string, byte[]> m_script = new Dictionary<string, byte[]>();
    IEnumerator readABIE()
    {
        //
        //string _url = "https://aiways-aichizhixingapp.oss-cn-shanghai.aliyuncs.com/ar-user-manual/Test/lua.data"; //Application.streamingAssetsPath + "/AB/Android/lua.data";

        string _url = Application.streamingAssetsPath + "/AB/Android/lua.data";
        string _savePath = Application.persistentDataPath + "/lua.data";
        //下载部分
        UnityWebRequest _web = new UnityWebRequest(_url);
        //初始化下载//
        _web.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        _web.SendWebRequest();
        while (_web.isDone)
        {
            yield return 0;
        }
        //
        while (!_web.downloadHandler.isDone)
        {
            yield return 0;
        }

        //将下载的内容写入，本地路径中
        File.WriteAllBytes(_savePath, _web.downloadHandler.data);
        yield return 0;
        Debug.LogError("下载完成！");

        AssetBundleCreateRequest _req = AssetBundle.LoadFromFileAsync(_savePath);

        while (_req.isDone)
        {
            yield return 0;
        }

        AssetBundle ScriptAB = _req.assetBundle;
        int ii = 0;
        //读取／／
        UnityEngine.Object[] _objList = ScriptAB.LoadAllAssets();
        for (int i = 0; i < _objList.Length; i++)
        {
            string _Keyname = _objList[i].name;
            if (!LuaState.m_script.ContainsKey(_Keyname))
            {
                // Debug.LogError(_Keyname);
                byte[] _byteList = UnityEngine.Object.Instantiate(_objList[i] as TextAsset).bytes;// (_objList[i] as TextAsset).bytes;
                LuaState.m_script.Add(_Keyname, _byteList);
            }
            else
            {
                LuaState.m_script[_Keyname] = UnityEngine.Object.Instantiate(_objList[i] as TextAsset).bytes;
            }

            //AppFacade.instance.GetManager<LuaManager>(ManagerName.LUA).DoFile2(_Keyname);

            ii++;
            Resources.UnloadAsset(_objList[i]);
            if (ii > 10)
            {
                ii = 0;
                yield return null;
            }


        }

        yield return 0;
        ScriptAB.Unload(false);
        _req = null;

        Debug.LogError("m_script:" + LuaState.m_script.Count);
        Debug.LogError("脚本读取完成");
    }

    private string[] split = (new string[] { "/", @"\" });
    /// <summary>
    /// 根据文件名字，读取Lua中的字节数据
    /// </summary>
    /// <param name="fileName"></param>
    /// <returns></returns>
    public byte[] getLua(string fileName)
    {
        string[] _list = fileName.Split(split, StringSplitOptions.None);
        string _name = _list[_list.Length - 1];

        if (!_name.EndsWith(".lua"))
            _name += ".lua";
        if (LuaState.m_script.ContainsKey(_name))
        {
            return LuaState.m_script[_name];
        }
        else
        {

        }
        return null;
    }


}
