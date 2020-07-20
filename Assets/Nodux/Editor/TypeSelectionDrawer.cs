using System;
using Nodux.Core;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace Nodux.PluginEditor
{
    [CustomPropertyDrawer(typeof(TypeSelectionFilter))]
    public class TypeSelectionDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect rect, SerializedProperty property, GUIContent label)
        {
            if (!(this.attribute is TypeSelectionFilter))
            {
                EditorGUILayout.HelpBox(new GUIContent("TypeSelectionEnable is not attached on attribute"));
                return;
            }

            // restore
            var attr = (TypeSelectionFilter) this.attribute;
            var propertyValue = property.GetValue<TypeSelection>();
            propertyValue?.Restore();

            // draw
            label = EditorGUI.BeginProperty(rect, label, property);
            var labelRect = EditorGUI.PrefixLabel(
                rect,
                label
            );
            var buttonRect = new Rect(
                rect.x + EditorGUIUtility.labelWidth + 2,
                rect.y,
                rect.width - EditorGUIUtility.labelWidth - 2,
                rect.height
            );

            if (GUI.Button(buttonRect, propertyValue.Type?.Name ?? "None"))
            {
                var screenButtonRect = GUIUtility.GUIToScreenRect(buttonRect);
                var position = new Vector2(
                    screenButtonRect.x + screenButtonRect.width / 2,
                    screenButtonRect.y + 2 * buttonRect.height
                );
                OnClick(attr, property, position, buttonRect.width, newName => { });
            }

            EditorGUI.EndProperty();
        }

        public override VisualElement CreatePropertyGUI(SerializedProperty property)
        {
            // Create property container element.
            var container = new VisualElement();

            if (!(this.attribute is TypeSelectionFilter))
            {
                UnityEngine.Debug.LogWarning("TypeSelectionEnable is not attached on attribute");
                container.Add(new Label("Not found attribute"));
                return container;
            }

            // restore
            var attr = (TypeSelectionFilter) this.attribute;
            var propertyValue = property.GetValue<TypeSelection>();
            propertyValue?.Restore();

            // Create property fields.
            var fieldContainer = new VisualElement()
            {
                style =
                {
                    flexDirection = FlexDirection.Row,
                    justifyContent = Justify.SpaceBetween,
                    alignItems = Align.Center,
                }
            };
            var label = new Label(this.fieldInfo.Name)
            {
                style =
                {
                    marginBottom = 3,
                    marginTop = 3,
                    marginLeft = 3,
                    marginRight = 3,
                }
            };
            fieldContainer.Add(label);
            var button = new Button();
            button.clickable.clicked += () =>
            {
                var position = GUIUtility.GUIToScreenPoint(Event.current.mousePosition);
                var width = button.resolvedStyle.width;
                OnClick(attr, property, position, width, newName => button.text = newName);
            };
            button.text = propertyValue?.Type?.Name ?? "None";
            fieldContainer.Add(button);

            container.Add(fieldContainer);

            return container;
        }

        private void OnClick(
            TypeSelectionFilter attr,
            SerializedProperty property,
            Vector2 position,
            float width,
            Action<string> invoke
        )
        {
            TypeSelectionSearchWindow.Show(
                attr,
                position,
                width,
                type =>
                {
                    Undo.RecordObject(property.serializedObject.targetObject, "update TypeSelection");
                    var value = JsonUtility.FromJson("{}", type);
                    var selection = new TypeSelection(value);
                    property.SetValue(selection);
                    property.serializedObject.ApplyModifiedProperties();
                    property.serializedObject.Update();
                    invoke(selection.Type?.Name ?? "None");
                });
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return 18f;
        }
    }
}