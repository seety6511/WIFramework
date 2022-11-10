
using UnityEngine;

namespace WIFramework.Util
{
    public class GameObjectLender<T> : Lender<T> where T : Behaviour
    {
        public Vector3 originPos { get; private set; }
        public Quaternion originRot { get; private set; }
        public GameObjectLender(T item) : base(item)
        {
            originPos = item.transform.position;
            originRot = item.transform.rotation;
        }

        public bool IsUsePosition => item.transform.position != originPos;
        public bool IsUseRotation => item.transform.rotation != originRot;
        public bool IsNewProduct => !IsUsePosition && !IsUseRotation;
        public void Repack()
        {
            item.transform.position = originPos;
            item.transform.rotation = originRot;
        }
    }

    /// <summary>
    /// 소유권 클래스.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class Lender<T>
    {
        protected T item;
        protected bool lental;
        public Lender(T item)
        {
            this.item = item;
        }

        public virtual bool Lental(out T result)
        {
            result = default(T);
            if (lental)
                return false;

            result = item;
            lental = true;
            return true;
        }

        public virtual bool Repay(T item)
        {
            if (!this.item.Equals(item))
                return false;

            lental = false;
            return true;
        }

        public bool InUse => lental;
    }
}   