using UnityEngine;
using WIFramework;

public class Test_GetKey : WIBehaviour, IGetKey
{
    public void GetKey(KeyCode key)
    {
        Debug.Log($"{name} is GetKey({key})");
    }
}
