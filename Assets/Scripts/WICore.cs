using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using UnityEngine.EventSystems;
using System;
using TMPro;
using WI.Element;

namespace WI.Core
{
    [InitializeOnLoad]
    public static class WICore
    {
        readonly static Type[] uiTypeTable = new Type[]
        {
            typeof(Canvas),
            typeof(Image),
            typeof(Button),
            typeof(Toggle),
            typeof(Slider),
            typeof(Text),
            typeof(RawImage),
            typeof(Dropdown),
            typeof(InputField),
            typeof(Scrollbar),
            typeof(ScrollRect),
            typeof(TextMeshProUGUI),
            typeof(TMP_InputField),
            typeof(TMP_Text),
        };
        static WICore()
        {
            Debug.Log("WICore Active");
            LoadUIObjects();
            EditorApplication.hierarchyChanged += Event_HierarchyChange;
        }

        static void LoadUIObjects()
        {
            Event_HierarchyChange();
        }

        ///WICore의 작업을 Editor 상에서 표시하기 위한 대리자
        ///단 한개의 agent만 허용한다.
        static WIEventSystem agent;
        private static void Event_HierarchyChange()
        {
            var objects = Resources.FindObjectsOfTypeAll(typeof(EventSystem));
            if (objects == null || objects.Length <= 0)
            {
                if (!agent)
                {
                    Debug.Log($"Not Found EventSystem.");
                    return;
                }
            }
            else
            {
                GameObject eventObj = GameObject.Find(objects[0].name);
                agent = eventObj.GetOrAddComponent<WIEventSystem>();
                for(int i = 1;i<objects.Length;++i)
                {
#if UNITY_EDITOR
                    GameObject.DestroyImmediate(objects[i]);
#else
                    GameObject.Destroy(objects[i]);
#endif
                }    
            }

            agent.name = "WICore";
        }

        public static T GetOrAddComponent<T>(this GameObject parent) where T : MonoBehaviour
        {
            if (parent.TryGetComponent<T>(out T result))
                return result;
            return parent.AddComponent<T>();
        }
    }
}