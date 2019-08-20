using UnityEngine;
using System.Collections;
using LuaInterface;

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
    }
}
