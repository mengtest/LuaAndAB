using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;

public class LuaABEditor : Editor
{
    static string savePath = "ScriptAB";
    static string saveVar = "data";
    static string _tar = "";

    [MenuItem("资源打包/lua 脚本打包/---(Resources_lua)Android---", false, 101)]
    public static void BuildScriptAB()
    {
        BuildScriptTar(BuildTarget.Android);
    }

    [MenuItem("资源打包/lua 脚本打包/---(Resources_lua)IOS---", false, 101)]
    public static void BuildScriptABIOS()
    {
        BuildScriptTar(BuildTarget.iOS);
    }


    public static void BuildScriptTar(BuildTarget _tar)
    {
        ToLuaMenu.CopyLuaFilesToResAct(() => {
            string path = Application.dataPath + "/Resources/lua";  //AssetDatabase.GUIDToAssetPath(strs[0]);Scripts  Resources
            if (!Directory.Exists(path))
            {
                Debug.LogError("请选择文件夹，来打包！");
                //创建文件夹
                //Directory.CreateDirectory(path);
            }
            else
            {
                Debug.LogError(path);
                //打包该文件夹//
                AssetBuildleEditor.BuildPathByTar(path, savePath, saveVar, _tar);
            }
        });

    }

}
