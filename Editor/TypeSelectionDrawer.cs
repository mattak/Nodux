using Nodux.Core;
using UnityEditor;
using UnityEngine;

namespace Nodux.PluginEditor
{
    [CustomPropertyDrawer(typeof(TypeSelectionFilter))]
    public class TypeSelectionDrawer : PropertyDrawer
    {
        private TypeSelectionRenderer _renderer = new TypeSelectionRenderer();
        private float TotalHeight = 0;

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            if (!(this.attribute is TypeSelectionFilter))
            {
                UnityEngine.Debug.LogWarning("TypeSelectionEnable is not attached on attribute");
                return;
            }

            _renderer.CheckInitialize(((TypeSelectionFilter) this.attribute).Category);

            EditorGUI.BeginProperty(position, label, property);
            this.TotalHeight = 0;

            _renderer.RenderTypeAndValue(ref position, property);

            this.TotalHeight = position.height;
            EditorGUI.EndProperty();
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return this.TotalHeight;
        }
    }
}