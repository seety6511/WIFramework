using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using WIFramework.Core;
using WIFramework.Util;

namespace WIFramework.UI
{
    /// <summary>
    /// For Unique Canvas. Is PanelBase Group
    /// </summary>
    [RequireComponent(typeof(Canvas))]
    public class CanvasBase : SingleBehaviour
    {
        public List<PanelBase> childPanels
        {
            get;
            private set;
        } = new List<PanelBase>();

        protected override void Awake()
        {
            base.Awake();
            GetChildPanels();
        }

        void GetChildPanels()
        {
            GetChildPanels(transform);
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

        void GetChildPanels(Transform root)
        {
            if (root == null)
                return;

            int childCount = root.childCount;
            if (childCount == 0)
                return;
            for(int i = 0; i < childCount; ++i)
            {
                var child = root.GetChild(i);
                if(child.TryGetComponent<PanelBase>(out var panel))
                {
                    childPanels.Add(panel);
                    //Debug.Log($"FindChildPanel:{panel.gameObject.name}");
                }
                GetChildPanels(child);
            }
        }
    }
}
