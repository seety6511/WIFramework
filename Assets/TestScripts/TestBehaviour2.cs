using UnityEngine;
using WIFramework;

public class TestBehaviour2 : SingleBehaviour
{
    public override void Initialize()
    {
        base.Initialize();
        Debug.Log($"{name} is Initialize");
    }
}