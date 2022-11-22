using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using WIFramework.UI;
using WIFramework.Util;

namespace WIFramework.Core.Manager
{
    [DefaultExecutionOrder(int.MinValue)]
    public class WIManager : MonoBehaviour
    {
        [SerializeField] WIBehaviour[] wiObjects;
        private void Awake()
        {
            Debug.Log($"WIManager Awake");
            List<GameObject> disableStart = new List<GameObject>();
            wiObjects = Resources.FindObjectsOfTypeAll<WIBehaviour>();
            foreach (var wi in wiObjects)
            {
                var obj = wi as MonoBehaviour;
                if (!obj.gameObject.activeSelf)
                {
                    //Debug.Log($"Find_Disable_Object : {obj.gameObject.name}");
                    obj.gameObject.SetActive(true);
                    disableStart.Add(obj.gameObject);
                }
            }
            
            foreach (var wi in wiObjects)
            {
                wi.Initialize();
            }

            foreach (var disable in disableStart)
            {
                disable.SetActive(false);
            }
        }

        private void Update()
        {
            Mailing_KeyboardInput();
        }

        void Mailing_KeyboardInput()
        {
            if (Input.anyKey)
            {
                var key = InputManager.GetCurrentKey;
                foreach (var wi in wiObjects)
                {
                    if (wi is IKeyboardActor)
                    {
                        var actor = wi as IKeyboardActor;
                        actor.KeyboardAction(key);
                    }
                }
            }
        }

        private void OnDestroy()
        {
            WIBehaviour.uniqueWI.Clear();
            Debug.Log($"WIManager Destroyed");
        }
    }
}