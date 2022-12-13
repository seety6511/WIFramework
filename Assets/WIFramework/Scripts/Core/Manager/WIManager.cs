using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using WIFramework.Util;

namespace WIFramework
{
    //[DefaultExecutionOrder(int.MinValue)]
    public static class WIManager
    {
        static Dictionary<WIBehaviour, GameObject> wiTable = new Dictionary<WIBehaviour, GameObject>();
        static Dictionary<int, WIBehaviour> singleTable = new Dictionary<int, WIBehaviour>();
        internal static HashSet<IGetKey> getKeyActors = new HashSet<IGetKey>();
        internal static HashSet<IGetKeyUp> getKeyUpActors = new HashSet<IGetKeyUp>();
        internal static HashSet<IGetKeyDown> getKeyDownActors = new HashSet<IGetKeyDown>();
        static WIManager()
        {
            Debug.Log($"WISystem Awake");
            wiTable.Clear();
            singleTable.Clear();
            getKeyActors.Clear();
            getKeyUpActors.Clear();
            getKeyDownActors.Clear();
        }

        public static void Unregist(WIBehaviour wi)
        {
            wiTable.Remove(wi);
            if (wi is ISingle sb)
                singleTable.Remove(wi.GetHashCode());
            if (wi is IKeyboardActor)
            {
                if (wi is IGetKey gk)
                    getKeyActors.Remove(gk);
               
                if (wi is IGetKeyUp gu)
                    getKeyUpActors.Remove(gu);
                
                if (wi is IGetKeyDown gd)
                    getKeyDownActors.Remove(gd);
            }
        }
        static void MoveToTrash(WIBehaviour target, WIBehaviour origin)
        {
            var trash = target.gameObject.TryAddComponent<TrashBehaviour>();
            trash.originBehaviour = origin; 
            wiTable.Remove(target);
            GameObject.Destroy(target);
        }

        static bool RegistSingleBehaviour(WIBehaviour wi)
        {
            var hashCode = wi.GetType().GetHashCode();
            //Debug.Log($"Regist Single {wi}");
            if (singleTable.ContainsKey(hashCode))
            {
                //Debug.Log($"SameCode! Origin={singleTable[hashCode].gameObject.name}, New={wi.gameObject.name}");
                if (singleTable[hashCode].GetHashCode()!= wi.GetHashCode())
                {
                    //Debug.Log($"Move To Trash");
                    MoveToTrash(wi, singleTable[hashCode]);
                    return false;
                }
                return true;
            }

            singleTable.Add(hashCode, wi);
            if(diWaitingTable.TryGetValue(wi.GetType(), out var watingList))
            {
                //Debug.Log($"Find Wating Table");
                foreach(var w in watingList)
                {
                    var injectTargets = w.GetType().GetAllFields();
                    
                    foreach(var t in injectTargets)
                    {
                        if (t.FieldType == wi.GetType())
                        {
                            //Debug.Log($"Find Missing");
                            t.SetValue(w, wi);
                        }
                    }
                }
            }
            return true;
        }

        public static void Regist(WIBehaviour wi)
        {
            if (wiTable.ContainsKey(wi))
                return;

            if (wi is ISingle)
            {
                if (!RegistSingleBehaviour(wi))
                    return;
            }
            wiTable.Add(wi, wi.gameObject);

            if(wi is IKeyboardActor)
            {
                if (wi is IGetKey gk)
                    getKeyActors.Add(gk);

                if (wi is IGetKeyUp gu)
                    getKeyUpActors.Add(gu);

                if (wi is IGetKeyDown gd)
                    getKeyDownActors.Add(gd);
            }
            Injection(wi);
            wi.Initialize();
        }
        static Dictionary<Type, List<WIBehaviour>> diWaitingTable = new Dictionary<Type, List<WIBehaviour>>();
        static void Injection(WIBehaviour wi)
        {
            var wiType = wi.GetType();
            var targetFields = wiType.GetAllFields();
            
            List<Component> childs = new List<Component>();
            wi.transform.Search(ref childs);
            
            var uiElements = childs
                .Where(c => c.GetType().IsSubclassOf(typeof(UIBehaviour)))
                .ToArray();
            var childTransforms = childs
                .Where(c => c.GetType().IsSubclassOf(typeof(Transform)) || c.GetType().Equals(typeof(Transform)))
                .ToArray();

            foreach (var f in targetFields)
            {
                if (f.FieldType.GetInterface(nameof(ISingle)) != null)
                {
                    //Debug.Log($"{wi} in ISingle.");
                    if (singleTable.TryGetValue(f.FieldType.GetHashCode(), out var value))
                    {
                        //Debug.Log($"Inject {value}");
                        f.SetValue(wi, value);
                    }
                    else
                    {
                        if (!diWaitingTable.ContainsKey(f.FieldType))
                        {
                            diWaitingTable.Add(f.FieldType, new List<WIBehaviour>());
                        }

                        diWaitingTable[f.FieldType].Add(wi);
                        //Debug.Log($"Inject Waiting {wi}");
                    }

                    continue;
                }

                if (f.FieldType.IsSubclassOf(typeof(UIBehaviour)))
                {
                    InjectNameAndType(uiElements, f, wi);
                    continue;
                }

                if (f.FieldType.IsSubclassOf(typeof(Transform)) || f.FieldType.Equals(typeof(Transform)))
                {
                    InjectNameAndType(childTransforms, f, wi);
                    continue;
                }
            }
        }
        static void InjectNameAndType<T>(T[] arr, FieldInfo info, WIBehaviour target) where T : Component
        {
            foreach (var a in arr)
            {
                if (a.name.Equals(info.Name))
                {
                    if (a.GetType().Equals(info.FieldType))
                    {
                        info.SetValue(target, a);
                        return;
                    }
                }
            }
        }
        static void Awake()
        {
            var roots = SceneManager.GetActiveScene().GetRootGameObjects();
            var tempList = new List<WIBehaviour>();
            for(int i = 0; i < roots.Length; ++i)
            {
                roots[i].transform.Search(ref tempList);
            }

            foreach(var c in tempList)
            {
                Regist(c);
            }

            Debug.Log($"Active WI : {wiTable.Count}");
        }

    }
}