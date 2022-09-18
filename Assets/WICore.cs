using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using UnityEngine.EventSystems;
using System;
using TMPro;

namespace WI.Core
{
    [InitializeOnLoad]
    public class WICore
    {
        static Dictionary<int, GameObject> uiTable = new Dictionary<int, GameObject>();
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
            typeof(TMP_)
        } 
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

        private static void Event_HierarchyChange()
        {
            var objects = Resources.FindObjectsOfTypeAll(typeof(RectTransform));
            uiTable.Clear();
            foreach (var o in objects)
            {
                if (uiTable.ContainsKey(o.GetInstanceID()))
                    continue;

                var obj = o as GameObject;
                uiTable.Add(o.GetInstanceID(), obj);
                //Debug.Log($"UI Object Found : {o.name}");
            }
        }
    }
}