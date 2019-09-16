using System;
using UnityEngine;
using UnityLeaf.PluginEditor;

namespace UnityLeaf.Core
{
    [Serializable]
    public class TypeSelection : ISerializationCallbackReceiver
    {
        private Type _type;
        private object _value;

        [HideInInspector] [SerializeField] private string FullName;
        [HideInInspector] [SerializeField] private string ContentJson = "{}";
        [HideInInspector] [SerializeField] private UnityEngine.Object[] UnityObjects;

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

            if (this.UnityObjects != null && this.UnityObjects.Length > 0)
            {
                TypeUtil.RestoreUnityObjects(this._type, this._value, this.UnityObjects);
            }

            return this._type != null && this._value != null;
        }

        public void PrepareSerialize()
        {
            if (this._type != null && this._value != null)
                this.UnityObjects = TypeUtil.ListUpUnityObjects(this._type, this._value);
        }

        public T Get<T>()
        {
            return (T) this._value;
        }

        public void OnBeforeSerialize()
        {
            this.PrepareSerialize();
        }

        public void OnAfterDeserialize()
        {
        }
    }
}