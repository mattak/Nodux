using System;
using UnityEngine;

namespace UnityLeaf.Core
{
    [Serializable]
    public class TypeSelection
    {
        private Type Type;
        private object Value;

        [HideInInspector] [SerializeField] private string FullName;
        [HideInInspector] [SerializeField] private string ContentJson = "{}";

        public TypeSelection(UnityEngine.Object value)
        {
            this.Type = value.GetType();
            this.Value = value;
            this.FullName = this.Type.AssemblyQualifiedName;
            this.ContentJson = JsonUtility.ToJson(this.Value);
        }

        public void Set(UnityEngine.Object value)
        {
            this.Type = value.GetType();
            this.Value = value;
            this.FullName = this.Type.FullName;
            this.ContentJson = JsonUtility.ToJson(this.Value);
        }

        public bool Restore()
        {
            if (string.IsNullOrEmpty(this.FullName))
            {
                UnityEngine.Debug.LogWarning("FullName is null or empty");
                return false;
            }
            this.Type = Type.GetType(this.FullName);

            if (string.IsNullOrEmpty(this.ContentJson))
            {
                UnityEngine.Debug.LogWarning("ContentJson is null or empty");
                return false;
            }
            this.Value = JsonUtility.FromJson(this.ContentJson, this.Type);

            return this.Type != null && this.Value != null;
        }

        public T Get<T>()
        {
            return (T)this.Value;
        }

        public Type ToType()
        {
            return this.Type;
        }
    }
}