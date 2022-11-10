using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using WIFramework.UI.Util;
using static WIFramework.UI.Util.UITextBundleController;

namespace WIFramework.UI.EditorScripts
{
    [CustomEditor(typeof(UITextBundleController))]
    public class UITextBundleControllerEditor : Editor
    {
        FontOption option;
        UITextBundleController controller;
        private TextMeshProUGUI[] texts;

        private void OnEnable()
        {
            controller = target as UITextBundleController;
            texts = controller.texts;
            if (texts.Length > 0)
                option = new FontOption(texts[0]);
            Undo.RegisterCompleteObjectUndo(controller, "UITextBundleController");
        }
        public int FloatField(string label, ref float value)
        {
            value = EditorGUILayout.FloatField(label, value);
            return EditorGUIUtility.GetControlID(FocusType.Passive);
        }
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            option.autoSize = EditorGUILayout.Toggle("AutoSize", option.autoSize);
            if (option.autoSize)
            {
                //option.fontSizeMin = EditorGUILayout.FloatField("FontSizeMin", option.fontSizeMin);
                var minID = FloatField("FontSizeMin", ref option.fontSizeMin);
                if (option.fontSizeMin > option.fontSizeMax)
                    option.fontSizeMin = option.fontSizeMax;

                //option.fontSizeMax = EditorGUILayout.FloatField("FontSizeMax", option.fontSizeMax);
                var maxID = FloatField("FontSizeMax", ref option.fontSizeMax);
                if (option.fontSizeMax < option.fontSizeMin)
                    option.fontSizeMax = option.fontSizeMin;

                EditorGUI.BeginDisabledGroup(true);
                if (option.autoFontSize < option.fontSizeMin)
                {
                    option.autoFontSize = option.fontSizeMin;
                }
                if (option.autoFontSize > option.fontSizeMax)
                {
                    option.autoFontSize = option.fontSizeMax;
                }
                var autoFontSizeID = FloatField("FontSize", ref option.autoFontSize);
                EditorGUI.EndDisabledGroup();
            }
            else
            {
                var fs = FloatField("FontSize", ref option.fontSize);
                //EditorApplication.RepaintProjectWindow();
                //SceneView.RepaintAll();
            }

            FontOptionUpdate();
            EditorUtility.SetDirty(target);
        }

        void FontOptionUpdate()
        {
            foreach (var t in texts)
            {
                t.font = option.font;
                t.enableAutoSizing = option.autoSize;

                if (option.autoSize)
                {
                    t.fontSizeMin = option.fontSizeMin;
                    t.fontSizeMax = option.fontSizeMax;
                    t.fontSize = option.autoFontSize;
                }
                else
                    t.fontSize = option.fontSize;
            }

        }
    }
}