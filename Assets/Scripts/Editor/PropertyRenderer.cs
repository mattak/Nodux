using UnityEditor;
using UnityLeaf.Core;

namespace UnityLeaf.PluginEditor
{
    public static class PropertyRenderer
    {
        public static bool RenderAny(string key, Any value)
        {
            if (value.Is<int>()) return RenderInt(key, value);
            return true;
        }

        public static bool RenderInt(string key, Any value)
        {
            var newValue = EditorGUILayout.IntField(key, value.Value<int>());
            return newValue != value.Value<int>();
        }
    }
}