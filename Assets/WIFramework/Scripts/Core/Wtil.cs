using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using WIFramework.UI;
using WIFramework.Core;
using WIFramework.Core.Manager;
using UnityEditor;

namespace WIFramework.Util
{
    public static class Wtil
    {
#if UNITY_EDITOR
        [MenuItem("Tools/CreateWIManager #'")]
        public static void CreateWIManager()
        {
            var wiManager = GameObject.FindObjectOfType<WIManager>();
            if (wiManager != null)
            {
                Debug.Log("Already WIManager");
                return;
            }

            wiManager = GameObject.Instantiate(Resources.Load<WIManager>("WIFramework/WIManager"));
            wiManager.gameObject.name = "WIManager";
        }
#endif
        public static T2[] ArrayConvertor<T, T2>(T[] origin) where T : WIBehaviour where T2 : WIBehaviour
        {
            T2[] result = new T2[origin.Length];
            for (int i = 0; i < result.Length; ++i)
            {
                result[i] = origin[i] as T2;
            }
            return result;
        }

        public static T2 PanelTypeConvertor<T, T2>(T origin) where T : WIBehaviour where T2 : WIBehaviour
        {
            return origin as T2;
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