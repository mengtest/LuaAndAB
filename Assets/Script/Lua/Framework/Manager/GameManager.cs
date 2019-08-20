using UnityEngine;
using System.IO;
using System.Collections;
using System.Collections.Generic;

namespace LuaFramework
{
    public class GameManager : Manager
    {
        protected override void Init()
        {
            DontDestroyOnLoad(gameObject);
        }
    }
}
