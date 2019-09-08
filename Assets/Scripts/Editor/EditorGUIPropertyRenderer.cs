using System;
using System.Collections;
using System.Reflection;
using UnityEditor;
using UnityEngine;
using UnityLeaf.Core;

namespace UnityLeaf.PluginEditor
{
    public static class EditorGUIPropertyRenderer
    {
        public static bool RenderSystemObject(
            ref Rect position, string key, Type type, object value,
            Action<object> setter,
            Func<Type, Attribute> attributeAccessor)
        {
            var rect = new Rect(position.x, position.y, position.width, 0);
            var dirty = false;

            if (type == typeof(int)) dirty = RenderInt(ref rect, key, (int) value, setter);
            else if (type == typeof(string))
                dirty = RenderString(ref rect, key, (string) value, setter);
            else if (type == typeof(bool))
                dirty = RenderBool(ref rect, key, (bool) value, setter);
            else if (type == typeof(float))
                dirty = RenderFloat(ref rect, key, (float) value, setter);
            else if (type == typeof(double))
                dirty = RenderDouble(ref rect, key, (double) value, setter);
            else if (type == typeof(Enum))
                dirty = RenderEnum(ref rect, key, (Enum) value, setter);
            else if (type == typeof(Vector2))
                dirty = RenderVector2(ref rect, key, (Vector2) value, setter);
            else if (type == typeof(Vector3))
                dirty = RenderVector3(ref rect, key, (Vector3) value, setter);
            else if (type == typeof(Vector4))
                dirty = RenderVector4(ref rect, key, (Vector4) value, setter);
            else if (type == typeof(Color))
                dirty = RenderColor(ref rect, key, (Color) value, setter);
            else if (typeof(IDictionary).IsAssignableFrom(type))
                dirty = RenderIDictionary(ref rect, key, (IDictionary) value, setter);
            else if (typeof(IList).IsAssignableFrom(type))
                dirty = RenderIList(ref rect, key, (IList) value, setter);
            else if (typeof(ICollection).IsAssignableFrom(type))
                dirty = RenderICollection(ref rect, key, (ICollection) value, setter);
            else if (typeof(UnityEngine.Object).IsAssignableFrom(type))
                dirty = RenderUnityObject(ref rect, key, type, (UnityEngine.Object) value, setter);
            else if (typeof(TypeSelection).IsAssignableFrom(type))
                dirty = TypeSelectionRenderer.RenderFromSystemObject(ref rect, key, (TypeSelection) value,
                    attributeAccessor, setter);

            position.height += rect.height;
            return dirty;
        }

        public static bool RenderInt(ref Rect position, string key, int value, Action<object> setter)
        {
            var rect = new Rect(position);
            rect.height = EditorGUIUtility.singleLineHeight;

            var newValue = EditorGUI.IntField(rect, key, value);
            position.height += rect.height;

            if (newValue == value) return false;
            setter(newValue);
            return true;
        }

        public static bool RenderString(ref Rect position, string key, string value, Action<object> setter)
        {
            var rect = new Rect(position);
            rect.height = EditorGUIUtility.singleLineHeight;

            var newValue = EditorGUI.TextField(rect, key, value);
            position.height += rect.height;

            if (newValue == value) return false;
            setter(newValue);
            return true;
        }

        public static bool RenderBool(ref Rect position, string key, bool value, Action<object> setter)
        {
            var rect = new Rect(position);
            rect.height = EditorGUIUtility.singleLineHeight;

            var newValue = EditorGUI.Toggle(rect, key, value);
            position.height += rect.height;

            if (newValue == value) return false;
            setter(newValue);
            return true;
        }

        public static bool RenderFloat(ref Rect position, string key, float value, Action<object> setter)
        {
            var rect = new Rect(position);
            rect.height = EditorGUIUtility.singleLineHeight;

            var newValue = EditorGUI.FloatField(rect, key, value);
            position.height += rect.height;

            if (Math.Abs(newValue - value) < float.Epsilon * 10) return false;
            setter(newValue);
            return true;
        }

        public static bool RenderDouble(ref Rect position, string key, double value, Action<object> setter)
        {
            var rect = new Rect(position);
            rect.height = EditorGUIUtility.singleLineHeight;

            var newValue = EditorGUI.DoubleField(rect, key, value);
            position.height += rect.height;

            if (Math.Abs(newValue - value) < double.Epsilon * 10) return false;
            setter(newValue);
            return true;
        }

        public static bool RenderEnum(ref Rect position, string key, Enum value, Action<object> setter)
        {
            var rect = new Rect(position);
            rect.height = EditorGUIUtility.singleLineHeight;

            var newValue = EditorGUI.EnumPopup(rect, key, value);
            position.height += rect.height;

            if (newValue.Equals(value)) return false;
            setter(newValue);
            return true;
        }

        public static bool RenderVector2(ref Rect position, string key, Vector2 value, Action<object> setter)
        {
            var rect = new Rect(position);
            rect.height = EditorGUIUtility.singleLineHeight;

            var newValue = EditorGUI.Vector2Field(rect, key, value);
            position.height += rect.height;

            if (newValue.Equals(value)) return false;
            setter(newValue);
            return true;
        }

        public static bool RenderVector3(ref Rect position, string key, Vector3 value, Action<object> setter)
        {
            var rect = new Rect(position);
            rect.height = EditorGUIUtility.singleLineHeight;

            var newValue = EditorGUI.Vector3Field(rect, key, value);
            position.height += rect.height;

            if (newValue.Equals(value)) return false;
            setter(newValue);
            return true;
        }

        public static bool RenderVector4(ref Rect position, string key, Vector4 value, Action<object> setter)
        {
            var rect = new Rect(position);
            rect.height = EditorGUIUtility.singleLineHeight;

            var newValue = EditorGUI.Vector4Field(rect, key, value);
            position.height += rect.height;

            if (newValue.Equals(value)) return false;
            setter(newValue);
            return true;
        }

        public static bool RenderColor(ref Rect position, string key, Color value, Action<object> setter)
        {
            var rect = new Rect(position);
            rect.height = EditorGUIUtility.singleLineHeight;

            var newValue = EditorGUI.ColorField(rect, key, value);
            position.height += rect.height;

            if (newValue.Equals(value)) return false;
            setter(newValue);
            return true;
        }

        public static bool RenderUnityObject(ref Rect position, string key, Type type, UnityEngine.Object value,
            Action<object> setter)
        {
            var rect = new Rect(position);
            rect.height = EditorGUIUtility.singleLineHeight;

            var newValue = EditorGUI.ObjectField(rect, key, value, type, true);
            position.height += rect.height;

            if (newValue == null && value == null) return false;
            if (newValue != null && newValue.Equals(value)) return false;
            setter(newValue);
            return true;
        }

        public static bool RenderIList(ref Rect position, string key, IList value, Action<object> setter)
        {
            var dirty = false;
            var rect = new Rect(position);
            var index = 0;
            rect.height = 0;

            foreach (var element in value)
            {
                dirty |= RenderSystemObject(
                    ref rect,
                    $"{key}[{index}]",
                    element?.GetType(),
                    element,
                    newValue => value[index] = newValue,
                    type => value?.GetType().GetCustomAttribute(type));
                rect.y += rect.height;
                position.height += rect.height;
                rect.height = 0;

                index++;
            }

            return true;
        }

        public static bool RenderICollection(ref Rect position, string key, ICollection value, Action<object> setter)
        {
            var dirty = false;
            var list = value;
            var rect = new Rect(position);
            var index = 0;

            foreach (var element in list)
            {
                dirty |= RenderSystemObject(
                    ref rect,
                    $"[{index}]",
                    element?.GetType(),
                    element,
                    newValue => { },
                    type => value?.GetType().GetCustomAttribute(type));
                index++;
                rect.y += rect.height;
                position.height += rect.height;
                rect.height = 0;
            }

            return dirty;
        }

        public static bool RenderIDictionary(ref Rect position, string key, IDictionary value, Action<object> setter)
        {
            var dirty = false;
            var dictionary = value;
            var rect = new Rect(position);

            foreach (var dictionaryKey in dictionary.Keys)
            {
                dirty |= RenderSystemObject(
                    ref rect,
                    dictionaryKey.ToString(),
                    dictionary[dictionaryKey]?.GetType(),
                    dictionary[dictionaryKey],
                    newValue => dictionary[dictionaryKey] = newValue,
                    type => value?.GetType().GetCustomAttribute(type));
                rect.y += rect.height;
                position.height += rect.height;
                rect.height = 0;
            }

            return dirty;
        }

        public static bool RenderClass(ref Rect position, string key, object objectValue)
        {
            var dirty = false;
            var rect = new Rect(position.x, position.y, position.width, 0);

            // label
            if (!string.IsNullOrEmpty(key))
            {
                rect.height = EditorGUIUtility.singleLineHeight;
                EditorGUI.LabelField(rect, key);
                rect.y += rect.height;
                position.height += rect.height;
                rect.height = 0;
            }

            // values
            var fields = objectValue.GetType()
                .GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
            foreach (var field in fields)
            {
                if (field.IsPublic || field.GetCustomAttribute(typeof(SerializeField)) != null)
                {
                    var fieldValue = field.GetValue(objectValue);

                    dirty |= RenderSystemObject(
                        ref rect,
                        field.Name,
                        field.FieldType,
                        fieldValue,
                        newValue => field.SetValue(objectValue, newValue),
                        type => field.GetCustomAttribute(type)
                    );

                    rect.y += rect.height;
                    position.height += rect.height;
                    rect.height = 0;
                }
            }

            return dirty;
        }
    }
}