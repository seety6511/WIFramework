using System;
using UnityEngine;
using WIFramework.UI;

namespace WIFramework.Core.Manager
{
    public class UIManager : MonoBehaviour
    {
        [SerializeField] WIBehaviour[] wiObjects;
        public void Start()
        {
            wiObjects = FindObjectsOfType<WIBehaviour>();
            foreach(var wi in wiObjects)
            {
                wi.Initialize();
            }
            Debug.Log($"UIManager Start");
        }

        private void Update()
        {
            if (Input.anyKey)
            {
                //Debug.Log(InputManager.GetCurrentKey);
            }
        }

        private void OnDestroy()
        {
            WIBehaviour.uniqueWI.Clear();
            Debug.Log($"UIManager Destroyed");
        }
    }
}