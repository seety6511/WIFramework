using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WIFramework;

public class Test_GetKeyDown : WIBehaviour, IGetKeyDown
{
    public void GetKey(KeyCode key)
    {
        Debug.Log($"{name} is GetKeyDown_({key})");
    }

}
