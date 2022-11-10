using System.Collections.Generic;
using UnityEngine;

namespace WIFramework.Util
{
    [System.Serializable]
    public struct SKeyValuePair<T, T2> 
    {
        [SerializeField] public T Key;
        [SerializeField] public T2 Value;
        public SKeyValuePair(T key, T2 value)
        {
            Key = key;
            Value = value;
        }
    }

    /// <summary>
    /// SerializeDictionary For Unity
    /// </summary>
    [System.Serializable]
    public class SDictionary<T, T2> : Dictionary<T, T2>
    {
        [SerializeField] public List<SKeyValuePair<T, T2>> datas=new List<SKeyValuePair<T, T2>>();
        
        public new bool Add(T key, T2 value)
        {
            if (base.ContainsKey(key))
                return false;

            base.Add(key, value);
            datas.Add(new SKeyValuePair<T, T2>(key, value));
            return true;
        }

        public new bool Remove(T key)
        {
            if (!base.ContainsKey(key))
                return false;
            for(int i = 0; i < datas.Count; ++i)
            {
                if (datas[i].Key.Equals(key))
                {
                    datas.RemoveAt(i);
                    base.Remove(key);
                    return true;
                }
            }
            return false;
        }

        public new void Clear()
        {
            base.Clear();
            datas.Clear();
        }
    }
}