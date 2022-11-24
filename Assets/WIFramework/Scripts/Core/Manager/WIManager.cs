using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using WIFramework.Core.Behaviour;
using WIFramework.Util;

namespace WIFramework.Core.Manager
{
    [DefaultExecutionOrder(int.MinValue)]
    public class WIManager : MonoBehaviour
    {
        [SerializeField] static List<WIBehaviour> wiList = new List<WIBehaviour>();
        [SerializeField] static SDictionary<WIBehaviour, GameObject> wiTable = new SDictionary<WIBehaviour, GameObject>();
        [SerializeField] static SDictionary<Type, SingleBehaviour> singleWiList = new SDictionary<Type, SingleBehaviour>();
        static void Regist(WIBehaviour wi)
        {
            if (wiList.Contains(wi))
                return;

            wiList.Add(wi);
            wiTable.Add(wi, wi.gameObject);

            switch (wi)
            {
                case IGetKey gk:
                    getKeyActors.Add(gk);
                    break;
                case IGetKeyUp gu:
                    getKeyUpActors.Add(gu);
                    break;
                case IGetKeyDown gd:
                    getKeyDownActors.Add(gd);
                    break;
            }
        }
        static bool RegistSingle(WIBehaviour wi)
        {
            if (wi is not SingleBehaviour)
                return false;
         
            var single = wi as SingleBehaviour;
            var singleType = single.GetType();
            
            if (!singleWiList.TryGetValue(singleType, out var origin))
            {
                singleWiList.Add(singleType, single);
                return true;
            }

            if (!origin.Equals(wi))
            {
                var trash = wi.gameObject.TryAddComponent<TrashBehaviour>();
                trash.prevBehaviourType = wi.GetType().Name;
                wiList.Remove(wi);
                wiTable.Remove(wi);
                Destroy(wi);
            }
            return false;
        }
        static void Injecting(WIBehaviour wi)
        {
            InjectUIBehaviour(wi);
            InjectSingleBehaviour(wi);
        }
        static void RuntimeRegist(WIBehaviour wi)
        {
            Regist(wi);
            RegistSingle(wi);
            Injecting(wi);
            wi.Initialize();
        }

        public static new T Instantiate<T>(T origin) where T : WIBehaviour
        {
            var copy = GameObject.Instantiate(origin);
            RuntimeRegist(copy);
            return copy;
        }

        private void Awake()
        {
            Debug.Log($"WIManager Awake");
            Debug.Log($"Find WIBehaviours...");
            var wbs = GameObject.FindObjectsOfType<WIBehaviour>();
            foreach(var wi in wbs)
            {
                Regist(wi);
                RegistSingle(wi);
            }

            Debug.Log($"Registed WI : {wiTable.Count}");
            foreach (var wi in wiList)
            {
                InjectUIBehaviour(wi);
                InjectSingleBehaviour(wi);
                wi.Initialize();

            }
            codes = Enum.GetValues(typeof(KeyCode));
        }
        static void InjectSingleBehaviour(WIBehaviour wi)
        {
            var fields = wi.GetType().GetFields();
            foreach(var f in fields)
            {
                if(f.FieldType.BaseType.Equals(typeof(SingleBehaviour)))
                {
                    if(singleWiList.TryGetValue(f.FieldType, out var value))
                    {
                        f.SetValue(wi, value);
                    }
                }
            }
        }

        /// <summary>
        /// 자식으로 있는 UI 오브젝트들 중 멤버변수의 이름과 동일한 것을 찾아 자동 캐싱 해줍니다.
        /// </summary>
        static void InjectUIBehaviour(WIBehaviour wi)
        {
            var uiElements = wi.GetComponentsInChildren<UIBehaviour>();
            
            if (uiElements == null || uiElements.Length == 0)
                return;

            var wiType = wi.GetType();
            //Debug.Log($"Start Injecting UIBehaviour:{wiType.Name}");
            var targetFields = wiType.GetFields();
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
        private void Update()
        {
            DetectingKey();
        }

        #region Input
        Array codes;
        static List<IGetKey> getKeyActors = new List<IGetKey>();
        static List<IGetKeyUp> getKeyUpActors = new List<IGetKeyUp>();
        static List<IGetKeyDown> getKeyDownActors = new List<IGetKeyDown>();
        void DetectingKey()
        {
            foreach (KeyCode k in codes)
            {
                if (Input.GetKey(k))
                {
                    Posting(k, getKeyActors);
                }
                if (Input.GetKeyDown(k))
                {
                    Posting(k, getKeyDownActors);
                }
                if (Input.GetKeyUp(k))
                {
                    Posting(k, getKeyUpActors);
                }
            }
        }
        void Posting<T>(KeyCode key, List<T> actorList) where T : IKeyboardActor
        {
            foreach (var actor in actorList)
            {
                actor.GetKey(key);
            }
        }
#endregion
        private void OnDestroy()
        {
            Debug.Log($"WIManager Destroyed");
            wiList.Clear();
            wiTable.Clear();
            singleWiList.Clear();
            getKeyActors.Clear();
            getKeyUpActors.Clear();
            getKeyDownActors.Clear();
        }
    }
}