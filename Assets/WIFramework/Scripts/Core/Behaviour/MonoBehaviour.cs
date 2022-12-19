using WIFramework;

public class MonoBehaviour : UnityEngine.MonoBehaviour
{
    protected virtual void Awake()
    {
        //Hooker.RegistReady(this);
        WIManager.Regist(this);
    }
    public virtual void Initialize()
    {
    }

    private void OnDestroy()
    {
        WIManager.Unregist(this);
    }

}