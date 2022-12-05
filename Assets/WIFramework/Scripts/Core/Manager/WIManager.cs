using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using WIFramework.Util;

namespace WIFramework
{
    //[DefaultExecutionOrder(int.MinValue)]
    public static class WIManager
    {
        
        static SDictionary<WIBehaviour, GameObject> wiTable = new SDictionary<WIBehaviour, GameObject>();
        static SDictionary<Type, SingleBehaviour> singleWiTable = new SDictionary<Type, SingleBehaviour>();
        internal static HashSet<IGetKey> getKeyActors = new HashSet<IGetKey>();
        internal static HashSet<IGetKeyUp> getKeyUpActors = new HashSet<IGetKeyUp>();
        internal static HashSet<IGetKeyDown> getKeyDownActors = new HashSet<IGetKeyDown>();
        static void MoveToTrash(WIBehaviour wi, SingleBehaviour origin)
        {
            var trash = wi.gameObject.TryAddComponent<TrashBehaviour>();
            trash.prevBehaviourType = wi.GetType().Name;
            trash.originBehaviour = origin; 
            wiTable.Remove(wi);
            GameObject.Destroy(wi);
        }
        public static void Unregist(WIBehaviour wi)
        {
            wiTable.Remove(wi);
            singleWiTable.Remove(wi.GetType());
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
        static void RegistSingleBehaviour(WIBehaviour wi)
        {
            var detailType = wi.GetType();
            if (wi is SingleBehaviour sb)
            {
                if (singleWiTable.TryGetValue(detailType, out var origin))
                {
                    if (origin != wi)
                    {
                        MoveToTrash(wi, origin);
                        return;
                    }
                }
                singleWiTable.Add(detailType, sb);
                if (diWaitingTable.TryGetValue(detailType, out var list))
                {
                    foreach (var l in list)
                    {
                        if (l is null)
                            continue;

                        //Debug.Log($"Tracking Missing DI : {l.name}_{detailType.Name}");
                        InjectSingleBehaviour(l);
                    }
                    list = list.Where(l => l != null).ToList();
                    if (diWaitingTable[detailType].Count == 0)
                        diWaitingTable.Remove(detailType);
                }
            }
        }
        public static void Regist(WIBehaviour wi)
        {
            if (wiTable.ContainsKey(wi))
                return;
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
            RegistSingleBehaviour(wi);

            Inject<UIBehaviour>(wi);
            InjectTransform<Transform>(wi);
            InjectTransform<RectTransform>(wi);
            InjectSingleBehaviour(wi);
            wi.Initialize();
        }
        static void InjectTransform<T>(WIBehaviour wi) where T : Transform
        {
            var uiElements = wi.GetComponentsInChildren<T>();

            if (uiElements == null || uiElements.Length == 0)
                return;

            var wiType = wi.GetType();
            //Debug.Log($"Start Injecting UIBehaviour:{wiType.Name}");
            var targetFields = wiType.GetAllFields();//.GetFields(BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public);
            foreach (var field in targetFields)
            {
                T targetUIObject = null;
                foreach (var e in uiElements)
                {
                    //Debug.Log($"Compare Name. A={e.name} B={field.Name}");
                    if (e.name.Equals(field.Name))
                    {
                        //Debug.Log($"Type Check. A={e.GetType().Name}, B={field.FieldType.Name}");
                        if (e.GetType().Equals(field.FieldType))
                        {
                            //Debug.Log($"InjectField={wiType.Name}:{field.Name}");
                            targetUIObject = e;
                            break;
                        }
                    }
                }
                if (targetUIObject == null)
                    continue;

                //Debug.Log($"Find Dependency Object:{targetUIObject.name}");
                field.SetValue(wi, targetUIObject);
            }
        }

        static void Inject<T>(WIBehaviour wi) where T : UIBehaviour
        {
            var uiElements = wi.GetComponentsInChildren<UIBehaviour>();
            
            if (uiElements == null || uiElements.Length == 0)
                return;

            var wiType = wi.GetType();
            //Debug.Log($"Start Injecting UIBehaviour:{wiType.Name}");
            var targetFields = wiType.GetAllFields();//.GetFields(BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public);
            foreach (var field in targetFields)
            {
                UIBehaviour targetUIObject = null;
                foreach (var e in uiElements)
                {
                    //Debug.Log($"Compare Name. A={e.name} B={field.Name}");
                    if (e.name.Equals(field.Name))
                    {
                        //Debug.Log($"Type Check. A={e.GetType().Name}, B={field.FieldType.Name}");
                        if (e.GetType().Equals(field.FieldType))
                        {
                            //Debug.Log($"InjectField={wiType.Name}:{field.Name}");
                            targetUIObject = e;
                            break;
                        }
                    }
                }
                if (targetUIObject == null)
                    continue;

                //Debug.Log($"Find Dependency Object:{targetUIObject.name}");
                field.SetValue(wi, targetUIObject);
            }
        }
        static Dictionary<Type, List<WIBehaviour>> diWaitingTable = new Dictionary<Type, List<WIBehaviour>>();
        static void InjectSingleBehaviour(WIBehaviour wi)
        {
            var fields = wi.GetType().GetAllFields();//.GetFields(BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public);
            foreach(var f in fields)
            {
                if(f.FieldType.BaseType.Equals(typeof(SingleBehaviour)))
                {
                    if(singleWiTable.TryGetValue(f.FieldType, out var value))
                    {
                        f.SetValue(wi, value);
                    }
                    else
                    {
                        //Debug.Log($"Missing Behaviour={wi.name}");
                        if (!diWaitingTable.TryGetValue(f.FieldType, out var waitingList))
                        {
                            diWaitingTable.Add(f.FieldType, new List<WIBehaviour>());
                        }
                        diWaitingTable[f.FieldType].Add(wi);
                    }
                }
            }
        }
        public static T Instantiate<T>(T origin) where T : WIBehaviour
        {
            if (origin is null)
                return null;

            var copy = GameObject.Instantiate(origin);
            Regist(copy);
            return copy;
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

        static WIManager()
        {
            Debug.Log($"WISystem Awake");
            wiTable.Clear();
            singleWiTable.Clear();
            getKeyActors.Clear();
            getKeyUpActors.Clear();
            getKeyDownActors.Clear();
            //Awake();
            //EditorApplication.update += DetectingKey;
        }
    }
}