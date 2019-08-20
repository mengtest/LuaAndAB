using LuaFramework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 需要将所有的素材加载完成之后，执行该脚本
/// </summary>
public class MainUILua : View
{
    [SerializeField]
    public string luaName = "MainSceneUI";
    //Awake
    protected override void Init()
    {
        base.Init();
   
        if (!facade.isInitialized)
        {
            ////注册信息
            //List<string> messages = new List<string>();
            //messages.Add(NotifyCommand.MAIN_RESOURCE_UPDATE_FINISH);
            //this.RemoveMessage(this, messages);
            //this.RegisterMessage(this, messages);

            //初始化--只初始化一次//
            /**************************/
            //AppFacade.instance.StartUp();
          
            AppFacade.instance.AddManager();

           // luaManager.Reload();



            luaManager.InitStart();
            /**************************/

            this.StartCoroutine(this.StartupInitLua());
        }
        else
        {
            //luaManager.Reload();
            //luaManager.InitStart();

            //this.StartCoroutine(this.StartupInitLua());
        }
    }


    public override void OnMessage(IMessage message)
    {

        Debug.LogError("OnMessage");

        string name = message.name;
        object body = message.body;
        switch (name)
        {
            case NotifyCommand.MAIN_RESOURCE_UPDATE_FINISH:
                {
                    facade.isInitialized = true;
                    this.StartCoroutine(this.StartupInitLua());
                }
                break;
        }
    }

    [ContextMenu("重新加载lua脚本")]
    public void ReLoadLua()
    {
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
        luaManager.DoFile("UI/" + luaName + ".lua");
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
