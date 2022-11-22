using UnityEngine;
using WIFramework.Core;

namespace WIFramework.Util
{
    public class SingleBehaviour : WIBehaviour
    {
        protected override void Awake()
        {
            if (AddWI(this))
            {
                base.Awake();
            }
        }
    }
}