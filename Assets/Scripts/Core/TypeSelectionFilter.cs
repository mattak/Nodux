using UnityEngine;

namespace Nodux.Core
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