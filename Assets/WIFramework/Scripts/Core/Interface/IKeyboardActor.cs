using UnityEngine;

namespace WIFramework.Core
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
    /// <summary>
    /// 2022-11-23. UniRx 도입 고민중
    /// </summary>
    public interface IGetKeyUp: IKeyboardActor { }
}