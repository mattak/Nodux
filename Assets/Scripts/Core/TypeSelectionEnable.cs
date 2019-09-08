using System;

namespace UnityLeaf.Core
{
    public class TypeSelectionEnable : Attribute
    {
        public string Category = "Default";

        public TypeSelectionEnable(string category)
        {
            this.Category = category;
        }
    }
}