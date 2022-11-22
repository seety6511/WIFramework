using UnityEngine;
using WIFramework.Core;
using WIFramework.Util;

namespace WIFramework.UI
{
    /// <summary>
    /// For Unique UI Group
    /// </summary>
    public class PanelBase : SingleBehaviour
    {
        /// <summary>
        /// rootCanvas == null ? WorldSpacePanel : ChildPanel
        /// 가장 가까이 있는 Canvas가 rootCanvas가 된다.
        /// </summary>
        [SerializeField] CanvasBase rootCanvas;
        protected override void Awake()
        {
            base.Awake();
            FindRootCanvas();
        }
        void FindRootCanvas()
        {
            rootCanvas = transform.GetComponentInParent<CanvasBase>();
            if (rootCanvas == null)
            {
                Debug.Log($"rootCanvas Is Null = {gameObject.name}");
            }
        }
    }
}
