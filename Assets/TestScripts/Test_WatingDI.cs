using UnityEngine;
using WIFramework;

public class Test_WatingDI : WIBehaviour, IGetKeyDown
{
    public WIBehaviour prefab;
    public TestBehaviour2 target;

    public override void Initialize()
    {
        base.Initialize();
        Debug.Log($"I Wating DI...");
    }

    public void GetKey(KeyCode key)
    {
        if(key == KeyCode.A)
        {
            WIManager.Instantiate(prefab);
        }
    }
}
