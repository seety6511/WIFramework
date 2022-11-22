using UnityEngine;
using UnityEngine.UI;
using WIFramework.UI;
using WIFramework.Util;

namespace WIFramework.Test
{
    public class Canvas_InjectTest : CanvasBase, IKeyboardActor
    {
        public Image image_Profile;
        public Button button_Enter;

        public void KeyboardAction(KeyCode q)
        {
        }
    }
}