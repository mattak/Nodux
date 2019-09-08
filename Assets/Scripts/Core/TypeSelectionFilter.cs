using UnityEngine;

namespace UnityLeaf.Core
{
    public class TypeSelectionFilter : PropertyAttribute
    {
        public string Category = "Default";

        public TypeSelectionFilter(string category)
        {
            this.Category = category;
        }
    }
}