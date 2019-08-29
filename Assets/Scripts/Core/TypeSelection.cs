using System;
using UnityEngine;

namespace UnityLeaf.Core
{
    [Serializable]
    public struct TypeSelection
    {
        private Type type;
        [SerializeField] private string FullName;

        public TypeSelection(Type type)
        {
            this.type = type;
            this.FullName = type.FullName;
        }

        public Type ToType()
        {
            return this.type;
        }
    }
}