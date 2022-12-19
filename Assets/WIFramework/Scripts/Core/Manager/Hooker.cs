using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.LowLevel;
namespace WIFramework
{
    public class Hooker
    {
        static Array codes;
        internal static void Initialize()
        {
            codes = Enum.GetValues(typeof(KeyCode));
            //PlayerLoopInterface.InsertSystemAfter(typeof(Hooker), MonoTracking, typeof(UnityEngine.PlayerLoop.EarlyUpdate.UpdatePreloading));
            PlayerLoopInterface.InsertSystemBefore(typeof(Hooker), DetectingKey, typeof(UnityEngine.PlayerLoop.PreUpdate.NewInputUpdate));
            //SetHook();
        }

        //static Queue<MonoBehaviour> registWatingQueue = new Queue<MonoBehaviour>(); 
        //static void MonoTracking()
        //{
        //    if (!Application.isPlaying)
        //        return;

        //    while (registWatingQueue.Count > 0)
        //    {
        //        var curr = registWatingQueue.Dequeue();
        //        if (curr != null)
        //            WIManager.Regist(curr);
        //    }
        //}

        //internal static void RegistReady(MonoBehaviour monoBehaviour)
        //{
        //    if (!Application.isPlaying)
        //        return;

        //    registWatingQueue.Enqueue(monoBehaviour);
        //}

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
