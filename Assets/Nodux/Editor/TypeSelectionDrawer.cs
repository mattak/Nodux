using Nodux.Core;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace Nodux.PluginEditor
{
    [CustomPropertyDrawer(typeof(TypeSelectionFilter))]
    // public class TypeSelectionDrawer : PropertyDrawer
    // {
    //     public override VisualElement CreatePropertyGUI(SerializedProperty property)
    //     {
    //         // Create property container element.
    //         var container = new VisualElement();
    //
    //         if (!(this.attribute is TypeSelectionFilter))
    //         {
    //             UnityEngine.Debug.LogWarning("TypeSelectionEnable is not attached on attribute");
    //             container.Add(new Label("Not found attribute"));
    //             return container;
    //         }
    //
    //         var attr = (TypeSelectionFilter) this.attribute;
    //
    //         // Create property fields.
    //         var fieldContainer = new VisualElement();
    //         var label = new Label(this.fieldInfo.Name);
    //         fieldContainer.Add(label);
    //         var button = new Button(() =>
    //         {
    //             UnityEngine.Debug.Log("Clicked");
    //             TypeSelectionSearchWindow.Show(attr, type =>
    //             {
    //                 var value = JsonUtility.FromJson("{}", type);
    //                 property.SetValue(value);
    //             });
    //         });
    //         button.text = property.type ?? "None";
    //         fieldContainer.Add(button);
    //
    //         // Add fields to the container.
    //         container.Add(fieldContainer);
    //
    //         return container;
    //     }
    // }
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