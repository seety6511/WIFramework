using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using WIFramework.UI;

namespace WIFramework.Util
{
    [InitializeOnLoad]
    public static class HierachyTracker
    {
        static HierachyTracker()
        {
            //EditorApplication.hierarchyChanged += UITracking;
        }

        static void UITracking()
        {
            var uis = Resources.FindObjectsOfTypeAll(typeof(RectTransform));
            foreach(var ui in uis)
            {
                var obj = ui as RectTransform;
                if (obj.gameObject.TryGetComponent<UIBehaviour>(out var ub))
                {

                }
            }
        }
    }
}
