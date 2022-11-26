using UnityEngine;

namespace WIFramework
{
    public partial class WIBehaviour : MonoBehaviour
    {
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
