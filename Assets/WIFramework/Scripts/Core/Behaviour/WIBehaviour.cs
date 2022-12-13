using UnityEngine;

namespace WIFramework
{
    public partial class WIBehaviour : MonoBehaviour
    {
        protected virtual void Awake()
        {
            WIManager.Regist(this);
        }
        public virtual void Initialize()
        {
        }

        private void OnDestroy()
        {
            WIManager.Unregist(this);
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
