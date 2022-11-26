using UnityEngine;
using WIFramework; 
public class Test_RuntimeWIMaker : SingleBehaviour, IGetKeyDown
{
    public WIBehaviour testBehaviour1;
    public WIBehaviour testBehaviour2;

    public void GetKey(KeyCode key)
    {
        if (key == KeyCode.Q)
            WIManager.Instantiate(testBehaviour1);
        if (key == KeyCode.W)
            WIManager.Instantiate(testBehaviour2);
    }
}
