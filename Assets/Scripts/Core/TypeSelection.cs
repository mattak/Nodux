using System;
using UnityEngine;

namespace UnityLeaf.Core
{
    [Serializable]
    public class TypeSelection
    {
        private Type _type;
        private object _value;

        [HideInInspector] [SerializeField] private string FullName;
        [HideInInspector] [SerializeField] private string ContentJson = "{}";

        public Type Type
        {
            get { return _type; }
            set { SetType(value); }
        }

        public object Object
        {
            get { return _value; }
            set { SetValue(value); }
        }

        public TypeSelection()
        {
        }

        public TypeSelection(object value)
        {
            this._type = value.GetType();
            this._value = value;
            this.FullName = this._type.AssemblyQualifiedName;
            this.ContentJson = JsonUtility.ToJson(this._value);
        }

        public void SetValue(object value)
        {
            this._type = value.GetType();
            this._value = value;
            this.FullName = this._type.FullName;
            this.ContentJson = JsonUtility.ToJson(this._value);
        }

        public void SetType(Type type)
        {
            this._type = type;
            this.FullName = this._type.AssemblyQualifiedName;
            this._value = null;
            this.ContentJson = "{}";
        }

        public bool Restore()
        {
            if (string.IsNullOrEmpty(this.FullName))
            {
                UnityEngine.Debug.LogWarning("FullName is null or empty");
                return false;
            }

            this._type = Type.GetType(this.FullName);

            if (string.IsNullOrEmpty(this.ContentJson))
            {
                this._value = JsonUtility.FromJson("{}", this._type);
            }
            else
            {
                this._value = JsonUtility.FromJson(this.ContentJson, this._type);
            }

            return this._type != null && this._value != null;
        }

        public T Get<T>()
        {
            return (T) this._value;
        }
    }
}