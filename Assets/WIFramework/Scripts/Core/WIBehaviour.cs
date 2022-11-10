using UnityEngine;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using WIFramework.Util;
using Debug = UnityEngine.Debug;
using System.Linq;

namespace WIFramework.UI
{
    public class WIBehaviour : UnityAwakeSealer
    {
        #region Dependencies
        protected override sealed void Awake()
        {
            var uiElements = FindObjectsOfType<UIBehaviour>().ToList();
            var wiType = GetType();
            var targetFields = wiType.GetFields();
            foreach(var field in targetFields)
            {
                UIBehaviour targetUIObject = null;
                foreach(var e in uiElements)
                {
                    //Debug.Log($"Compare Name. A={e.name} B={field.Name}");
                    if (e.name.Equals(field.Name))
                    {
                        //Debug.Log($"Type Check. A={e.GetType().Name}, B={field.FieldType.Name}");
                        if (e.GetType().Equals(field.FieldType))
                        {
                            //Debug.Log($"IsPair! Find:{e.gameObject.name}");
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

        #endregion

        #region UnityMethods
        protected virtual void OnEnable() { }
        protected virtual void OnDisable() { }
        #endregion

        #region ConvenienceMethods
        public T GetCanvas<T>() where T : CanvasBase
        {
            return transform.GetCanvas<T>();
        }
        public bool GetPanel<T>(out T result) where T : PanelBase
        {
            return transform.GetPanel(out result);
        }
        public bool GetPanels<T>(out List<T> result) where T : PanelBase
        {
            return transform.GetPanels(out result);
        }
        public bool GetUIElement<T>(string targetName, out T result) where T : UIBehaviour
        {
            return transform.GetUIElement(targetName, out result);
        }
        public bool GetUIElements<T>(out T[] result) where T : UIBase
        {
            return transform.GetUIElements(out result);
        }
        #endregion

        #region Action Methods
        public virtual void Active() { }
        public virtual void Deactive() { }
        public virtual void Initialize() { }
        #endregion
    }
}
