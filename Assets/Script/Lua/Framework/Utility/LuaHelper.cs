using UnityEngine;
using System;
using System.Reflection;
using System.Collections;
using LuaInterface;

namespace LuaFramework
{
    public class LuaHelper
    {
        public static System.Type GetType(string classname)
        {
            Assembly assembly = Assembly.GetExecutingAssembly();
            System.Type type = null;
            type = assembly.GetType(classname); ;
            if (type == null)
                type = assembly.GetType(classname);
            return type;
        }

        public static void SetTexture(UnityEngine.UI.Image image, UnityEngine.Texture2D texture, Vector2 pivot)
        {
            if (image && texture)
            {
                UnityEngine.Sprite sprite = UnityEngine.Sprite.Create(texture, new Rect(0.0f, 0.0f, texture.width, texture.height), pivot);
                if (sprite) image.sprite = sprite;
            }
        }

        //public static void AddCompleteListener(DG.Tweening.DOTweenPath path, LuaFunction func)
        //{
        //    if(path != null && path.onComplete != null)
        //        path.onComplete.AddListener(() => { func.Call(); });
        //}

        public static SoundManager GetSoundManager()
        {
            return AppFacade.instance.GetManager<SoundManager>(ManagerName.SOUND);
        }
    }
}
