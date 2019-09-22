using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Nodux.Core;
using UnityEditor;
using UnityEngine;

namespace Nodux.PluginEditor
{
    public static class EditorGUILayoutRenderer
    {
        public static bool RenderSystemObject(
            string key,
            Type type,
            object value,
            Action<object> setter,
            Func<Type, Attribute> attributeAccessor)
        {
            var dirty = false;

            if (type == typeof(int)) dirty = RenderInt(key, (int) value, setter);
            else if (type == typeof(string))
                dirty = RenderString(key, (string) value, setter);
            else if (type == typeof(bool))
                dirty = RenderBool(key, (bool) value, setter);
            else if (type == typeof(float))
                dirty = RenderFloat(key, (float) value, setter);
            else if (type == typeof(double))
                dirty = RenderDouble(key, (double) value, setter);
            else if (type == typeof(Enum))
                dirty = RenderEnum(key, (Enum) value, setter);
            else if (type == typeof(Vector2))
                dirty = RenderVector2(key, (Vector2) value, setter);
            else if (type == typeof(Vector3))
                dirty = RenderVector3(key, (Vector3) value, setter);
            else if (type == typeof(Vector4))
                dirty = RenderVector4(key, (Vector4) value, setter);
            else if (type == typeof(Color))
                dirty = RenderColor(key, (Color) value, setter);
            else if (typeof(IDictionary).IsAssignableFrom(type))
                dirty = RenderIDictionary(key, (IDictionary) value, setter);
            else if (typeof(IList).IsAssignableFrom(type))
                dirty = RenderIList(key, (IList) value, setter);
            else if (typeof(ICollection).IsAssignableFrom(type))
                dirty = RenderICollection(key, (ICollection) value, setter);
            else if (typeof(UnityEngine.Object).IsAssignableFrom(type))
                dirty = RenderUnityObject(key, type, (UnityEngine.Object) value, setter);
            else if (typeof(TypeSelection).IsAssignableFrom(type))
                dirty = TypeSelectionRenderer.RenderFromSystemObject(key, (TypeSelection) value,
                    attributeAccessor, setter);
            else if (type.GetCustomAttribute(typeof(SerializableAttribute)) != null)
                dirty = RenderClass(key, value);
            else
            {
                EditorGUILayout.HelpBox($"cannot render object: key:{key} type:{type}", MessageType.Warning);
            }

            return dirty;
        }

        public static bool RenderInt(string key, int value, Action<object> setter)
        {
            var newValue = EditorGUILayout.IntField(key, value);

            if (newValue == value) return false;
            setter(newValue);
            return true;
        }

        public static bool RenderString(string key, string value, Action<object> setter)
        {
            var newValue = EditorGUILayout.TextField(key, value);

            if (newValue == value) return false;
            setter(newValue);
            return true;
        }

        public static bool RenderBool(string key, bool value, Action<object> setter)
        {
            var newValue = EditorGUILayout.Toggle(key, value);

            if (newValue == value) return false;
            setter(newValue);
            return true;
        }

        public static bool RenderFloat(string key, float value, Action<object> setter)
        {
            var newValue = EditorGUILayout.FloatField(key, value);

            if (Math.Abs(newValue - value) < float.Epsilon * 10) return false;
            setter(newValue);
            return true;
        }

        public static bool RenderDouble(string key, double value, Action<object> setter)
        {
            var newValue = EditorGUILayout.DoubleField(key, value);

            if (Math.Abs(newValue - value) < double.Epsilon * 10) return false;
            setter(newValue);
            return true;
        }

        public static bool RenderEnum(string key, Enum value, Action<object> setter)
        {
            var newValue = EditorGUILayout.EnumPopup(key, value);

            if (newValue.Equals(value)) return false;
            setter(newValue);
            return true;
        }

        public static bool RenderVector2(string key, Vector2 value, Action<object> setter)
        {
            var newValue = EditorGUILayout.Vector2Field(key, value);

            if (newValue.Equals(value)) return false;
            setter(newValue);
            return true;
        }

        public static bool RenderVector3(string key, Vector3 value, Action<object> setter)
        {
            var newValue = EditorGUILayout.Vector3Field(key, value);

            if (newValue.Equals(value)) return false;
            setter(newValue);
            return true;
        }

        public static bool RenderVector4(string key, Vector4 value, Action<object> setter)
        {
            var newValue = EditorGUILayout.Vector4Field(key, value);

            if (newValue.Equals(value)) return false;
            setter(newValue);
            return true;
        }

        public static bool RenderColor(string key, Color value, Action<object> setter)
        {
            var newValue = EditorGUILayout.ColorField(key, value);

            if (newValue.Equals(value)) return false;
            setter(newValue);
            return true;
        }

        public static bool RenderUnityObject(string key, Type type, UnityEngine.Object value,
            Action<object> setter)
        {
            var newValue = EditorGUILayout.ObjectField(key, value, type, true);

            if (newValue == null && value == null) return false;
            if (newValue != null && newValue.Equals(value)) return false;
            setter(newValue);
            return true;
        }

        public static bool RenderIList(string key, IList value, Action<object> setter)
        {
            var dirty = false;
            var index = 0;

            foreach (var element in value)
            {
                dirty |= RenderSystemObject(
                    $"{key}[{index}]",
                    element?.GetType(),
                    element,
                    newValue => value[index] = newValue,
                    type => value?.GetType().GetCustomAttribute(type));

                index++;
            }

            return true;
        }

        public static bool RenderICollection(string key, ICollection value, Action<object> setter)
        {
            var dirty = false;
            var list = value;
            var index = 0;

            foreach (var element in list)
            {
                dirty |= RenderSystemObject(
                    $"[{index}]",
                    element?.GetType(),
                    element,
                    newValue => { },
                    type => value?.GetType().GetCustomAttribute(type));
                index++;
            }

            return dirty;
        }

        public static bool RenderIDictionary(string key, IDictionary value, Action<object> setter)
        {
            var dirty = false;
            var dictionary = value;

            var keys = new List<object>();
            foreach (var dictionaryKey in dictionary.Keys)
            {
                keys.Add(dictionaryKey);
            }

            foreach (var dictionaryKey in keys.ToList())
            {
                dirty |= RenderSystemObject(
                    dictionaryKey.ToString(),
                    dictionary[dictionaryKey]?.GetType(),
                    dictionary[dictionaryKey],
                    newValue => dictionary[dictionaryKey] = newValue,
                    type => value?.GetType().GetCustomAttribute(type));
            }

            return dirty;
        }

        public static bool RenderClass(string key, object objectValue)
        {
            var dirty = false;

            // label
            if (!string.IsNullOrEmpty(key))
            {
                EditorGUILayout.LabelField(key);
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
                        field.Name,
                        field.FieldType,
                        fieldValue,
                        newValue => field.SetValue(objectValue, newValue),
                        type => field.GetCustomAttribute(type)
                    );
                }
            }

            return dirty;
        }
    }
}