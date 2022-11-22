using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using WIFramework.UI;
using WIFramework.Core;

namespace WIFramework.Util
{
    public static class Wtil
    {
        public static T2[] ArrayConvertor<T, T2>(T[] origin) where T : WIBehaviour where T2 : WIBehaviour
        {
            T2[] result = new T2[origin.Length];
            for (int i = 0; i < result.Length; ++i)
            {
                result[i] = origin[i] as T2;
            }
            return result;
        }

        /// <summary>
        /// 있으면 추가하지 않는다
        /// Do not allow duplicates
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="target"></param>
        /// <returns></returns>
        public static T TryAddComponent<T>(this GameObject target) where T : Component
        {
            if (target.TryGetComponent<T>(out var result))
                return result;

            return target.AddComponent<T>();
        }
    }
}