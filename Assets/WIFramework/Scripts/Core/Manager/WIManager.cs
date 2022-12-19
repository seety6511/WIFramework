using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;
using UnityEngine.EventSystems;

namespace WIFramework
{
    public static partial class WIManager
    {
        static Dictionary<MonoBehaviour, GameObject> wiTable = new Dictionary<MonoBehaviour, GameObject>();
        static Dictionary<int, MonoBehaviour> singleTable = new Dictionary<int, MonoBehaviour>();
        internal static HashSet<IGetKey> getKeyActors = new HashSet<IGetKey>();
        internal static HashSet<IGetKeyUp> getKeyUpActors = new HashSet<IGetKeyUp>();
        internal static HashSet<IGetKeyDown> getKeyDownActors = new HashSet<IGetKeyDown>();
        static Dictionary<Type, List<MonoBehaviour>> diWaitingTable = new Dictionary<Type, List<MonoBehaviour>>();
        static WIManager()
        {
            Debug.Log($"WISystem Awake");
            wiTable.Clear();
            singleTable.Clear();
            diWaitingTable.Clear();

            getKeyActors.Clear();
            getKeyUpActors.Clear();
            getKeyDownActors.Clear();
        }
        internal static void Regist(MonoBehaviour mb)
        {
            if (wiTable.ContainsKey(mb))
                return;

            if (mb is ISingle)
            {
                if (!RegistSingleBehaviour(mb))
                    return;
            }
            wiTable.Add(mb, mb.gameObject);

            if(mb is IKeyboardActor)
            {
                if (mb is IGetKey gk)
                    getKeyActors.Add(gk);

                if (mb is IGetKeyUp gu)
                    getKeyUpActors.Add(gu);

                if (mb is IGetKeyDown gd)
                    getKeyDownActors.Add(gd);
            }
            Injection(mb);
            mb.Initialize();
        }
        internal static void Unregist(MonoBehaviour mb)
        {
            wiTable.Remove(mb);
            if (mb is ISingle sb)
                singleTable.Remove(mb.GetHashCode());
            if (mb is IKeyboardActor)
            {
                if (mb is IGetKey gk)
                    getKeyActors.Remove(gk);
               
                if (mb is IGetKeyUp gu)
                    getKeyUpActors.Remove(gu);
                
                if (mb is IGetKeyDown gd)
                    getKeyDownActors.Remove(gd);
            }
        }
        static bool RegistSingleBehaviour(MonoBehaviour mb)
        {
            var hashCode = mb.GetType().GetHashCode();
            //Debug.Log($"Regist Single {wi}");
            if (singleTable.ContainsKey(hashCode))
            {
                //Debug.Log($"SameCode! Origin={singleTable[hashCode].gameObject.name}, New={wi.gameObject.name}");
                if (singleTable[hashCode].GetHashCode()!= mb.GetHashCode())
                {
                    //Debug.Log($"Move To Trash");
                    MoveToTrash(mb, singleTable[hashCode]);
                    return false;
                }
                return true;
            }

            singleTable.Add(hashCode, mb);
            if(diWaitingTable.TryGetValue(mb.GetType(), out var watingList))
            {
                //Debug.Log($"Find Wating Table");
                foreach(var w in watingList)
                {
                    var injectTargets = w.GetType().GetAllFields();
                    
                    foreach(var t in injectTargets)
                    {
                        if (t.FieldType == mb.GetType())
                        {
                            //Debug.Log($"Find Missing");
                            t.SetValue(w, mb);
                        }
                    }
                }
            }
            return true;
        }
        static void Injection(MonoBehaviour mb)
        {
            var wiType = mb.GetType();
            var targetFields = wiType.GetAllFields();

            List<Component> childs = new List<Component>();
            mb.transform.Search(ref childs);

            List<UIBehaviour> uiElements = new List<UIBehaviour>();
            List<Transform> childTransforms = new List<Transform>();
            
            //Filtering 
            foreach(var c in childs)
            {
                var cType = c.GetType();
                if (cType.IsSubclassOf(typeof(UIBehaviour)))
                {
                    uiElements.Add(c as UIBehaviour);
                    continue;
                }

                if (cType.IsSubclassOf(typeof(Transform)) || cType.Equals(typeof(Transform)))
                {
                    childTransforms.Add(c as Transform);
                    continue;
                }
            }

            //Injecting
            foreach (var f in targetFields)
            {
                Label label = (Label)f.GetCustomAttribute(typeof(Label));
                if (label != null)
                {
                    foreach(var c in childs)
                    {
                        if (c.name.Equals(label.name))
                        {
                            if(c.TryGetComponent(label.target, out var value))
                            {
                                f.SetValue(mb, value);
                            }
                        }
                    }
                }

                if (f.FieldType.GetInterface(nameof(ISingle)) != null)
                {
                    //Debug.Log($"{wi} in ISingle.");
                    if (singleTable.TryGetValue(f.FieldType.GetHashCode(), out var value))
                    {
                        //Debug.Log($"Inject {value}");
                        f.SetValue(mb, value);
                    }
                    else
                    {
                        if (!diWaitingTable.ContainsKey(f.FieldType))
                        {
                            diWaitingTable.Add(f.FieldType, new List<MonoBehaviour>());
                        }

                        diWaitingTable[f.FieldType].Add(mb);
                        //Debug.Log($"Inject Waiting {wi}");
                    }

                    continue;
                }

                if (f.FieldType.IsSubclassOf(typeof(UIBehaviour)))
                {
                    InjectNameAndType(uiElements, f, mb);
                    continue;
                }

                if (f.FieldType.IsSubclassOf(typeof(Transform)) || f.FieldType.Equals(typeof(Transform)))
                {
                    InjectNameAndType(childTransforms, f, mb);
                    continue;
                }
            }
        }
        static void MoveToTrash(MonoBehaviour target, MonoBehaviour origin)
        {
            var trash = target.gameObject.TryAddComponent<TrashBehaviour>();
            trash.originBehaviour = origin; 
            wiTable.Remove(target);
            GameObject.Destroy(target);
        }
        static void InjectNameAndType<T>(List<T> arr, FieldInfo info, MonoBehaviour target) where T : Component
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
    }
}