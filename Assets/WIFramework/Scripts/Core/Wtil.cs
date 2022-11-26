using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace WIFramework
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
        /// <summary>
        /// 하위의 모든(Deactive포함) child를 순회하며 T Type의 컴포넌트가 붙어있는지 검사하고, 있다면 container를 통해 내보낸다.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="root"></param>
        /// <param name="container"></param>
        public static void Search<T>(this Transform root, ref List<T> container)
        {
            if (root.TryGetComponents<T>(out var value))
            {
                container.AddRange(value);
            }
            var childCount = root.childCount;
            for (int i = 0; i < childCount; ++i)
            {
                Search(root.GetChild(i), ref container);
            }
        }
        public static bool TryGetComponents<T>(this Transform root, out T[] result)
        {
            result = root.GetComponents<T>();
            return result.Length > 0;
        }
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