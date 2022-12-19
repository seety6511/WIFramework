using System;

namespace WIFramework
{
    [AttributeUsage(AttributeTargets.Field)]
    public class Label : Attribute
    {
        public string name;
        public Type target;

        public Label(Type targetType, string gameObjectName)
        {
            target = targetType;
            name = gameObjectName;
        }
    }
}