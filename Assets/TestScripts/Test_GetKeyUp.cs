using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WIFramework.Core;

public class Test_GetKeyUp : WIBehaviour, IGetKeyUp
{
    public void GetKey(KeyCode key)
    {
        Debug.Log($"{name} is GetKeyUp_({key})");
    }
}
