using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.LowLevel;
namespace WIFramework
{
    public class KeyHooker
    {
        static Array codes;
        internal static void Initialize()
        {
            Debug.Log($"KeyHookerOn");
            codes = Enum.GetValues(typeof(KeyCode));
            PlayerLoopInterface.InsertSystemBefore(typeof(KeyHooker), DetectingKey, typeof(UnityEngine.PlayerLoop.PreUpdate.NewInputUpdate));
            //SetHook();
        }
        static void DetectingKey()
        {
            foreach (KeyCode k in codes)
            {
                if (Input.GetKey(k))
                {
                    Posting(k, WIManager.getKeyActors);
                }
                if (Input.GetKeyDown(k))
                {
                    Posting(k, WIManager.getKeyDownActors);
                }
                if (Input.GetKeyUp(k))
                {
                    Posting(k, WIManager.getKeyUpActors);
                }
            }
        }
        static void Posting<T>(KeyCode key, HashSet<T> actorList) where T : IKeyboardActor
        {
            foreach (var actor in actorList)
            {
                if (actor is null)
                {
                    Debug.Log("A");
                    continue;
                }
                actor.GetKey(key);
            }
        }
    }
}
