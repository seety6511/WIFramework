using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WIFramework.Core;

public class Test_GetKey : WIBehaviour, IGetKey
{
    public void GetKey(KeyCode key)
    {
        Debug.Log($"{name} is GetKey({key})");
    }
}
