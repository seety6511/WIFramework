using UnityEngine;
using WIFramework.Core;

namespace WIFramework.Util
{
    public class SingleBehaviour : WIBehaviour
    {
        protected void Awake()
        {
            if (AddWI(this))
            {
            }
        }
    }
}