using System;

namespace UnityLeaf.Core
{
    public struct Any
    {
        public object Object;
        public Type Type;

        public Any(object obj)
        {
            this.Type = obj?.GetType();
            this.Object = obj;
        }

        public TValue Value<TValue>()
        {
            if (this.Object is TValue)
            {
                return (TValue)this.Object;
            }

            return default(TValue);
        }

        public bool Is<TValue>()
        {
            return this.Object is TValue;
        }

        public override string ToString()
        {
            return $"Any(Type:{Type}, Object:{Object})";
        }
    }
}