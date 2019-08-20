using LuaFramework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 在绑定该脚本之后，动态执行，加载对应的Lua脚本
/// </summary>
public class MainBehaviourLua : View
{
    [SerializeField][HideInInspector]
    private string luaName = "MainSceneUI";
    //Awake
    protected override void Init()
    {
        base.Init();
   
        if (!facade.isInitialized)
        {
            AppFacade.instance.AddManager();
            luaManager.InitStart();
            //this.StartCoroutine(this.StartupInitLua()); 外部调用 使用set//
        }
    }

    [ContextMenu("set")]
    public void set()
    {
        this.StartCoroutine(this.StartupInitLua());
    }


    public void Set(string _luaName)
    {
        Debug.Log("_luaName_luaName_luaName_luaName");
        this.luaName = _luaName;
        this.StartCoroutine(this.StartupInitLua());
    }


    //加载lua脚本//
    protected IEnumerator StartupInitLua()
    {
        //  yield return new WaitForSeconds(0.5f);
        //
        yield return 0;
        Debug.Log("luaManager.InitStart()");

        Debug.Log("luaManager.InitStart()");
        //luaManager.Reload();
        Debug.Log("luaManager.Reload();");
        luaManager.InitStart();
        Debug.Log("luaManager.InitStart();");
        luaManager.DoFile(luaName);
        Debug.Log("luaName luaName luaName ");
        luaManager.CallFunction(luaName + ".Init", this);
        Debug.Log("luaManager.InitStart()-------");
        Debug.Log(luaManager);
    }

    //update
    protected override void Tick(float time)
    {
        //luaManager.CallFunction("UIController.Tick", time);
    }
    //ondestory
    protected override void Term()
    {
        //luaManager.CallFunction(luaName + ".Term", 0);
    }

    void OnDisable()
    {
        // UnityEngine.Object.Destroy(this.gameObject);
    }

}
