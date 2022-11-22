using System;
using System.Collections.Generic;
using UnityEngine;

namespace WIFramework.Core.Manager
{
    public static class InputManager 
    {
        public static KeyCode GetCurrentKey
        {
            get
            {
                var codes = Enum.GetValues(typeof(KeyCode));
                foreach(KeyCode k in codes)
                {
                    if (Input.GetKey(k))
                    {
                        return k;
                    }
                }
                return KeyCode.None;
            }
        }
    }
}
