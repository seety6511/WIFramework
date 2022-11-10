using UnityEngine;
using TMPro;

namespace WIFramework.UI.Util
{
    #region Editor

    public class UITextBundleController : MonoBehaviour
    {
        public TextMeshProUGUI[] texts
        {
            get => GetComponentsInChildren<TextMeshProUGUI>();
        }

        public struct FontOption
        {
            public FontOption(TextMeshProUGUI t)
            {
                autoSize = t.enableAutoSizing;
                fontSize = t.fontSize;
                autoFontSize = t.fontSize;
                fontSizeMin = t.fontSizeMin;
                fontSizeMax = t.fontSizeMax;
                font = t.font;
                color = t.color;
            }

            public bool autoSize;
            public float fontSize;
            public float fontSizeMin;
            public float fontSizeMax;
            public TMP_FontAsset font;
            public Color color;
            public float autoFontSize;
        }

    }
#endregion
}