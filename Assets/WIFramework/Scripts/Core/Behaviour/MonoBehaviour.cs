using WIFramework;
public class MonoBehaviour : UnityEngine.MonoBehaviour
{
    public MonoBehaviour()
    {
        Hooker.RegistReady(this);
    }
    public virtual void Initialize()
    {
    }

    private void OnDestroy()
    {
        WIManager.Unregist(this);
    }
}