using UnityEngine;
using System.Collections;

namespace LuaFramework
{
    public class MathLib
    {
        public const float PI = Mathf.PI;

        public static float Sin(float value)
        {
            return Mathf.Sin(value);
        }

        public static float Cos(float value)
        {
            return Mathf.Cos(value);
        }

        public static float Lerp(float start, float end, float lerp)
        {
            return Mathf.Lerp(start, end, lerp);
        }

        public static float Exp(float power)
        {
            return Mathf.Exp(power);
        }

        public static float Clamp(float value, float min, float max)
        {
            return Mathf.Clamp(value, min, max);
        }

        public static float Clamp01(float value)
        {
            return Mathf.Clamp01(value);
        }

        public static Transform PhysicsRaycast(Ray ray)
        {
            RaycastHit hitInfo;
            if (Physics.Raycast(ray, out hitInfo))
                return hitInfo.transform;
            return null;
        }
    }
}
