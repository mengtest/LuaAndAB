using System;


namespace LuaFramework
{
    /// <summary>
    /// 用于音乐的加载//
    /// </summary>
    public interface IMusicLoader
    {
        /// <summary>
        /// 加载音乐资源
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        UnityEngine.AudioClip LoadUrl(string url);

        /// <summary>
        /// 卸载指定的音频//
        /// </summary>
        /// <param name="_clip"></param>
        void UnLoadUrl(UnityEngine.AudioClip _clip);

    }
}
