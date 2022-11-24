﻿using UnityEngine;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using WIFramework.Util;
using WIFramework.UI;
using System.Linq;
using System;
using UnityEngine.Pool;
using WIFramework.Core.Manager;

namespace WIFramework.Core
{
    public class WIBehaviour : MonoBehaviour
    {
        public virtual void Initialize()
        {
        }

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
