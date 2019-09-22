using System;

namespace Nodux.Core
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