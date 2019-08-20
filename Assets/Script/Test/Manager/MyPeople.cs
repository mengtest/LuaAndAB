using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 用于 Lua中，对Class类的实例化的操作
/// </summary>
public class MyPeople
{

    public string Name = "";

    public void Set(string _name)
    {
        Name = _name;
    }

    public void outPut()
    {
        Debug.LogError("Name:" + Name);
    }

}
