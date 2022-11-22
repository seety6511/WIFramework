using UnityEngine;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using WIFramework.Util;
using WIFramework.UI;
using System.Linq;
using System;

namespace WIFramework.Core
{
    /// <summary>
    /// Awake 사용 및 수정 금지. Awake 대신 Initialize Override 하여 사용바람.
    /// </summary>
    public class WIBehaviour : MonoBehaviour
    {
        public static Dictionary<Type, WIBehaviour> uniqueWI
        {
            get;
            private set;
        } = new Dictionary<Type, WIBehaviour>();
        public static List<WIBehaviour> wiList
        {
            get;
            private set;
        } = new List<WIBehaviour>();
        protected static bool AddWI(WIBehaviour obj)
        {
            var t = obj.GetType();
            if (uniqueWI.TryGetValue(t, out var origin))
            {
                if (obj != origin)
                {
                    Debug.LogError($"DuplicateUniqueWI:{obj.gameObject}, {origin.gameObject}");
                    Destroy(obj);
                }
                return false;
            }
            wiList.Add(obj);
            uniqueWI.Add(t, obj);
            return true;
        }
        #region Dependencies
        public virtual void Initialize()
        {
            Inject_Canvas();
            Inject_ChildPanels();
            Inject_UIBehaviour();
        }
        /// <summary>
        /// 자식으로 있는 PanelBase 캐싱
        /// </summary>
        public List<PanelBase> childPanels
        {
            get;
            private set;
        } = new List<PanelBase>();

        void Inject_ChildPanels()
        {
            //GetChildPanels(transform);
            childPanels = GetComponentsInChildren<PanelBase>().ToList();
            var fields = GetType().GetFields();
            foreach (var f in fields)
            {
                foreach (var c in childPanels)
                {
                    if (f.FieldType.Equals(c.GetType()))
                    {
                        f.SetValue(this, c);
                    }
                }
            }
        }

        [Obsolete]
        void GetChildPanels(Transform root)
        {
            if (root == null)
                return;

            int childCount = root.childCount;
            if (childCount == 0)
                return;
            for (int i = 0; i < childCount; ++i)
            {
                var child = root.GetChild(i);
                if (child.TryGetComponent<PanelBase>(out var panel))
                {
                    childPanels.Add(panel);
                    //Debug.Log($"FindChildPanel:{panel.gameObject.name}");
                }
                GetChildPanels(child);
            }
        }
        /// <summary>
        /// 자식으로 있는 UI 오브젝트들 중 멤버변수의 이름과 동일한 것을 찾아 자동 캐싱 해줍니다.
        /// </summary>
        void Inject_UIBehaviour()
        {
            var uiElements = GetComponentsInChildren<UIBehaviour>().ToList();
            var wiType = GetType();
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
                            uiElements.Remove(e);
                            break;
                        }
                    }
                }
                if (targetUIObject == null)
                    continue;

                //Debug.Log($"Find Dependency Object:{targetUIObject.name}");
                field.SetValue(this, targetUIObject);
            }

        }

        /// <summary>
        /// Canvas Inject.
        /// CanvasBase는 Unique한 ID 이므로 위치에 상관 없이 의존성을 주입해줍니다.
        /// </summary>
        /// <param name="q"></param>
        void Inject_Canvas()
        {
            //Debug.Log($"Start Injecting WICanvas:{GetType().Name}");
            var fields = GetType().GetFields();
            foreach (var f in fields)
            {
                var canvas = FindObjectOfType(f.FieldType);
                if (canvas != null && canvas is CanvasBase)
                {
                    //Debug.Log($"Find! : {canvas.name}");
                    AddWI(canvas as CanvasBase);
                    f.SetValue(this, canvas as CanvasBase);
                }
                //if (!uniqueWI.TryGetValue(f.FieldType, out var canvas))
                //{
                //    var findCanvas = FindObjectOfType(f.FieldType);
                //    if (findCanvas == null)
                //    {
                //        Debug.Log($"Not Found... {f.FieldType.Name}");
                //        continue;
                //    }
                //    AddWI(findCanvas as CanvasBase);
                //}

                //if (!uniqueWI.TryGetValue(f.FieldType, out canvas))
                //{
                //    Debug.Log($"Injecting WICanvas!:{GetType().Name}/{f.FieldType.Name}");
                //    f.SetValue(this, canvas);
                //}
                //else
                //{
                //    Debug.LogError($"Not Found WICanvas! {f.FieldType.Name}");
                //}
            }
        }


        #endregion

        #region UnityMethods
        protected virtual void OnEnable() { }
        protected virtual void OnDisable() { }
        #endregion

        #region Action Methods
        public virtual void Active()
        {
            gameObject.SetActive(true);
        }
        public virtual void Deactive()
        {
            gameObject.SetActive(false);
        }
    
        #endregion
    }
}
