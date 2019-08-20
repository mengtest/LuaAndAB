using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using Object = UnityEngine.Object;

public partial class AssetBuildleEditor : Editor
{
    static string outpath = "";
    public static string[] split = (new string[] { "/", @"\" });
    public static string[] point = (new string[] { "." });

    //参数1 为要查找的总路径， 参数2 保存路径
    public static void GetDirs(string dirPath, string _ext, ref List<string> dirs)
    {
        foreach (string path in Directory.GetFiles(dirPath))
        {
            //获取所有文件夹中包含后缀为 .prefab 的路径
            if (System.IO.Path.GetExtension(path) == _ext)//".prefab"
            {
                dirs.Add(path.Substring(path.IndexOf("Assets")));
                Debug.Log(path.Substring(path.IndexOf("Assets")).Replace(@"\", "/"));
            }
        }

        if (Directory.GetDirectories(dirPath).Length > 0)  //遍历所有文件夹
        {
            foreach (string path in Directory.GetDirectories(dirPath))
            {
                GetDirs(path, _ext, ref dirs);
            }
        }
    }

    public static void BuildPathByTar(string _dir, string _savepath = "PrefabFile", string _variant = "assets", BuildTarget _tar = BuildTarget.Android)
    {
        string dir = _dir.Substring(_dir.IndexOf("Assets"));

        AssetImporter asset = AssetImporter.GetAtPath(dir);
        string[] _list = dir.Split(split, StringSplitOptions.None);
        //Debug.LogError("_backUrl:" + _list.Length);
        string _picName = _list[_list.Length - 1];
        //Debug.LogError("_picName:" + _list.Length);
        //--存储的本地路径--
        // string _picPath = _picName.Split('.')[0];
        asset.SetAssetBundleNameAndVariant(_picName, _variant);


        string _ABPath = Application.streamingAssetsPath + "/" + _savepath;

        //如果没有该文件夹，则需要手动创建//
        if (!Directory.Exists(_ABPath))
        {
            Directory.CreateDirectory(_ABPath);
        }

        Debug.Log("Android:" + dir);
        Debug.Log(AssetDatabase.GUIDToAssetPath(dir));
        BuildPipeline.BuildAssetBundles(_ABPath, BuildAssetBundleOptions.None, _tar);
        //刷新资源ku//
        AssetDatabase.Refresh();
        //清理设置的打包包名//
        //清理设置的打包包名//
        ResetFolder(dir);
        // ResetSelectFolderFileBundleName();
    }

    public static void ResetFolder(string filePath)
    {
        AssetImporter ai = AssetImporter.GetAtPath(filePath);
        ai.assetBundleName = null;
        AssetDatabase.Refresh();
    }
}
