using UnityEngine;

namespace WIFramework
{
    public interface IKeyboardActor
    {
        public void GetKey(KeyCode key);
    }
    public interface IGetKeyDown : IKeyboardActor
    {
    }
    public interface IGetKey : IKeyboardActor
    {
    }
    public interface IGetKeyUp: IKeyboardActor { }
}