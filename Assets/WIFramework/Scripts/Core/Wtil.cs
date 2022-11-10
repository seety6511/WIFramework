using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using WIFramework.UI;

namespace WIFramework.Util
{
    public static class Wtil
    {
        static Dictionary<Transform, Dictionary<string, List<UIBehaviour>>> childElementTable =
        new Dictionary<Transform, Dictionary<string, List<UIBehaviour>>>(); 

        static Dictionary<Transform, Dictionary<Type, List<UIBase>>> childCustomUITable = 
        new Dictionary<Transform, Dictionary<Type, List<UIBase>>>();

        static Dictionary<Transform, int> childCountTable = new Dictionary<Transform, int>();
        public static bool GetUIElement<T>(this Transform root, string targetName, out T result) where T : UIBehaviour
        {
            result = null;
            if (childElementTable.TryGetValue(root, out var childNameTable))
            {
                if (childNameTable.TryGetValue(targetName, out var uiList))
                {
                    foreach (var t in uiList)
                    {
                        if (t is T)
                        {
                            result = t as T;
                            return true;
                        }
                    }
                }
            }
            if (!childElementTable.ContainsKey(root))
                childElementTable.Add(root, new Dictionary<string, List<UIBehaviour>>());
            var childs = root.GetComponentsInChildren<UIBehaviour>();
            foreach (var c in childs)
            {
                var childName = c.gameObject.name;
                if (!childElementTable[root].TryGetValue(childName, out var childUIList))
                {
                    childElementTable[root].Add(childName, new List<UIBehaviour>());
                }
                childElementTable[root][childName].Add(c);
                if (c is T && c.gameObject.name.Equals(targetName))
                {
                    result = c as T;
                }
            }

            return result != null;
        }
        public static bool GetUIElements<T>(this Transform root, out T[] result) where T : UIBase
        {
            result = null;
            var customType = typeof(T);
            if (!childCountTable.ContainsKey(root))
            {
                childCountTable.Add(root, root.childCount);
            }

            if (!childCustomUITable.ContainsKey(root))
            {
                childCustomUITable.Add(root, new Dictionary<Type, List<UIBase>>());
            }

            if (!childCustomUITable[root].TryGetValue(customType, out var list))
            {
                list = new List<UIBase>();
                childCustomUITable[root].Add(customType, list);
                list.AddRange(root.GetComponentsInChildren<T>());
            }

            result = ArrayConvertor<UIBase, T>(list.ToArray());

            return result != null && result.Length > 0;
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