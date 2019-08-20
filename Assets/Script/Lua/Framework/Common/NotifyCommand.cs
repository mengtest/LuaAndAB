using UnityEngine;
using System.Collections;

public class NotifyCommand
{
   
    /// <summary>
    /// Controller层消息通知
    /// 在 启动时 绑定的事件触发机制//
    /// 只有集成Controller的类 才可以 执行对应的接口事件//
    /// </summary>
    public const string STARTUP = "Startup";    //启动框架

    public const string DISPATCH_MESSAGE = "DispatchMessage";   //派发信息

    /// <summary>
    /// View层消息通知
    /// 主资源更新进度
    /// </summary>
    public const string MAIN_RESOURCE_UPDATE_PROGRESS = "MainResourceUpdateProgress";   //主资源更新进度
    /// <summary>
    /// 主资源更新完毕
    /// 避免资源更新没有完成，执行的lua脚本，导致报错//
    /// </summary>
    public const string MAIN_RESOURCE_UPDATE_FINISH = "MainResourceUpdateFinish";       //主资源更新完毕
}
