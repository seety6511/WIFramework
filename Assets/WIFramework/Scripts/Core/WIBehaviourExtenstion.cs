
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using WIFramework.Core;

namespace WIFramework.UI
{
    public static class WIBehaviourExtenstion
    {
        public static T2 PanelTypeConvertor<T, T2>(T origin) where T : WIBehaviour where T2 : WIBehaviour

        {
            return origin as T2;
        }
    }
}
