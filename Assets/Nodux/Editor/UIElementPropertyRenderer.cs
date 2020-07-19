using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Nodux.Core;
using Nodux.PluginEditor.NoduxGraph;
using UnityEngine;
using UnityEngine.UIElements;
using Object = UnityEngine.Object;

namespace Nodux.PluginEditor
{
    public static class UIElementPropertyRenderer
    {
        public static void RenderClass(
            VisualElement container,
            Type type,
            object obj,
            Action<object> setter)
        {
            if (type == null || obj == null)
            {
                var label = new Label($"no properties - {type}");
                container.Add(label);
                return;
            }

            var fields = type
                .GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)
                .Where(field => field.IsPublic || field.GetCustomAttribute(typeof(SerializeField)) != null);

            foreach (var info in fields)
            {
                RenderInstanceProperty(
                    container,
                    info.FieldType,
                    info.Name,
                    info.GetValue(obj),
                    newValue => info.SetValue(obj, newValue),
                    info.GetCustomAttributes<Attribute>()
                );
            }
        }

        public static void RenderInstanceProperty(
            VisualElement container,
            Type fieldType,
            string name,
            object value,
            Action<object> setter,
            IEnumerable<Attribute> attributes)
        {
            if (fieldType == typeof(string))
                UIElementPrimitivePropertyUtil.RenderStringProperty(container, name, value, setter);
            else if (fieldType == typeof(int))
                UIElementPrimitivePropertyUtil.RenderIntProperty(container, name, value, setter);
            else if (fieldType == typeof(float))
                UIElementPrimitivePropertyUtil.RenderFloatProperty(container, name, value, setter);
            else if (fieldType == typeof(bool))
                UIElementPrimitivePropertyUtil.RenderBooleanProperty(container, name, value, setter);
            else if (fieldType == typeof(long))
                UIElementPrimitivePropertyUtil.RenderLongProperty(container, name, value, setter);
            else if (fieldType == typeof(double))
                UIElementPrimitivePropertyUtil.RenderDoubleProperty(container, name, value, setter);
            else if (fieldType == typeof(Enum))
                UIElementPrimitivePropertyUtil.RenderEnumProperty(container, name, value, setter);
            else if (fieldType == typeof(Vector2))
                UIElementPrimitivePropertyUtil.RenderVector2Property(container, name, value, setter);
            else if (fieldType == typeof(Vector3))
                UIElementPrimitivePropertyUtil.RenderVector3Property(container, name, value, setter);
            else if (fieldType == typeof(Vector2Int))
                UIElementPrimitivePropertyUtil.RenderVector2IntProperty(container, name, value, setter);
            else if (fieldType == typeof(Vector3Int))
                UIElementPrimitivePropertyUtil.RenderVector3IntProperty(container, name, value, setter);
            else if (fieldType == typeof(Color))
                UIElementPrimitivePropertyUtil.RenderColorProperty(container, name, value, setter);
            else if (fieldType == typeof(Vector4))
                UIElementPrimitivePropertyUtil.RenderVector4Property(container, name, value, setter);
            else if (fieldType == typeof(Rect))
                UIElementPrimitivePropertyUtil.RenderRectProperty(container, name, value, setter);
            else if (fieldType == typeof(AnimationCurve))
                UIElementPrimitivePropertyUtil.RenderCurveProperty(container, name, value, setter);
            else if (fieldType == typeof(Gradient))
                UIElementPrimitivePropertyUtil.RenderGradientProperty(container, name, value, setter);
            else if (fieldType == typeof(Bounds))
                UIElementPrimitivePropertyUtil.RenderBoundsProperty(container, name, value, setter);
            else if (fieldType.IsArray)
            {
                RenderArrayProperty(container, name, value, setter, fieldType, attributes);
            }
            else if (fieldType.IsGenericType && fieldType.GetGenericTypeDefinition() == typeof(List<>))
            {
                RenderListProperty(container, name, value, setter, fieldType, attributes);
            }
            else if (typeof(Object).IsAssignableFrom(fieldType))
            {
                UIElementPrimitivePropertyUtil.RenderUnityObjectProperty(container, name, value, setter, fieldType);
            }
            else if (attributes.FirstOrDefault(x => x is TypeSelectionFilter) != null)
            {
                var filter = (TypeSelectionFilter) attributes.FirstOrDefault(x => x is TypeSelectionFilter);
                RenderTypeSelectionFilterProperty(container, name, value, setter, filter);
            }
            else
            {
                RenderClass(container, fieldType, value, setter);
                // RenderUndefinedProperty(container, name, value, setter);
            }
        }

        private static void RenderArrayProperty(
            VisualElement container,
            string name,
            object value,
            Action<object> setter,
            Type fieldType,
            IEnumerable<Attribute> attributes)
        {
            var listValue = (IList) value;
            var elementType = fieldType.GetElementType();
            Func<VisualElement> makeItem = () => new VisualElement();
            Action<VisualElement, int> bindItem = (element, i) =>
            {
                element.Clear();
                RenderInstanceProperty(element, elementType, $"{name}[{i}]", listValue[i], newValue =>
                {
                    listValue[i] = newValue;
                    setter(listValue);
                }, attributes);
            };

            var listView = new ListView(listValue, 16, makeItem, bindItem);
            listView.style.flexGrow = 1.0f;
            listView.style.width = -1;
            listView.style.minHeight = 16 * listValue?.Count ?? 0;

            UIElementPrimitivePropertyUtil.RenderIntProperty(container, $"{name}.Count", (listValue?.Count ?? 0),
                newValue =>
                {
                    var size = (int) newValue;
                    var newArray = Array.CreateInstance(elementType, size > 0 ? size : 0);
                    for (var i = 0; i < (listValue?.Count ?? 0) && i < newArray.Length; i++)
                    {
                        newArray.SetValue(listValue[i], i);
                    }

                    listValue = newArray;
                    setter(listValue);
                    listView.itemsSource = listValue;
                    listView.style.minHeight = 16 * listValue.Count;
                });
            container.Add(listView);
        }

        private static void RenderListProperty(
            VisualElement container,
            string name,
            object value,
            Action<object> setter,
            Type fieldType,
            IEnumerable<Attribute> attributes)
        {
            var listValue = (IList) value;
            var elementType = fieldType.GetGenericArguments()[0];
            Func<VisualElement> makeItem = () => new VisualElement();
            Action<VisualElement, int> bindItem = (element, i) =>
            {
                element.Clear();
                RenderInstanceProperty(element, elementType, $"{name}[{i}]", listValue[i], newValue =>
                {
                    listValue[i] = newValue;
                    setter(listValue);
                }, attributes);
            };

            var listView = new ListView(listValue, 16, makeItem, bindItem);
            listView.style.flexGrow = 1.0f;
            listView.style.width = -1;
            listView.style.minHeight = 16 * listValue?.Count ?? 0;

            UIElementPrimitivePropertyUtil.RenderIntProperty(container, $"{name}.Count", (listValue?.Count ?? 0),
                newValue =>
                {
                    var size = (int) newValue;
                    var newArray = (IList) Activator.CreateInstance(fieldType);
                    for (var i = 0; i < size; i++)
                    {
                        if (i < (listValue?.Count ?? 0))
                        {
                            newArray.Add(listValue[i]);
                        }
                        else
                        {
                            newArray.Add(elementType.IsValueType ? Activator.CreateInstance(elementType) : null);
                        }
                    }

                    listValue = newArray;
                    setter(listValue);
                    listView.itemsSource = listValue;
                    listView.style.minHeight = 16 * listValue.Count;
                });
            container.Add(listView);
        }

        private static void RenderTypeSelectionFilterProperty(
            VisualElement container,
            string name,
            object value,
            Action<object> setter,
            TypeSelectionFilter filter)
        {
            var field = new Button();
            var selection = (TypeSelection) value;
            selection?.Restore(withWarning: false);
            var fieldText = (selection == null || selection.Type == null) ? "null" : selection.Type.Name;
            field.text = fieldText;
            field.MarkDirtyRepaint();
            field.clickable.clicked += () =>
            {
                TypeSelectionSearchWindow.Show(filter,
                    type =>
                    {
                        field.text = type.Name;
                        var instance = JsonUtility.FromJson("{}", type);
                        setter(new TypeSelection(instance));
                    });
            };
            container.Add(field);
        }

        private static void RenderUndefinedProperty(
            VisualElement container,
            string name,
            object value,
            Action<object> setter)
        {
            var textField = new TextField(name);
            textField.SetEnabled(false);
            textField.SetValueWithoutNotify(value?.ToString() ?? "null");
            textField.MarkDirtyRepaint();
            container.Add(textField);
        }
    }
}