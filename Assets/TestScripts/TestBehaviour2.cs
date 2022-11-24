using UnityEngine;
using WIFramework.Core;

public class TestBehaviour2 : WIBehaviour
{
    public override void Initialize()
    {
        base.Initialize();
        Debug.Log($"{name} is Initialize");
    }
}