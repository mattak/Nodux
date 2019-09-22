using System;
using System.Collections;
using System.Reflection;
using Nodux.Core;
using UnityEditor;
using UnityEngine;

namespace Nodux.PluginEditor
{
    public static class PropertyRenderer
    {
        public static bool RenderAny(string key, Any value, Action<Any> setter)
        {
            if (value.Is<int>()) return RenderInt(key, value, setter);
            if (value.Is<string>()) return RenderString(key, value, setter);
            if (value.Is<bool>()) return RenderBool(key, value, setter);
            if (value.Is<float>()) return RenderFloat(key, value, setter);
            if (value.Is<double>()) return RenderDouble(key, value, setter);
            if (value.Is<Enum>()) return RenderEnum(key, value, setter);
            if (value.Is<Vector2>()) return RenderVector2(key, value, setter);
            if (value.Is<Vector3>()) return RenderVector3(key, value, setter);
            if (value.Is<Vector4>()) return RenderVector4(key, value, setter);
            if (value.Is<Color>()) return RenderColor(key, value, setter);
            if (value.Is<IDictionary>()) return RenderIDictionary(key, value, setter);
            if (value.Is<IList>()) return RenderIList(key, value, setter);
            if (value.Is<ICollection>()) return RenderICollection(key, value, setter);
            if (value.Is<UnityEngine.Object>()) return RenderObject(key, value, setter);
            EditorGUILayout.HelpBox($"{key} {value}", MessageType.None);
            return false;
        }

        public static bool RenderInt(string key, Any value, Action<Any> setter)
        {
            var newValue = EditorGUILayout.IntField(key, value.Value<int>());
            if (newValue == value.Value<int>()) return false;
            setter(new Any(newValue));
            return true;
        }

        public static bool RenderString(string key, Any value, Action<Any> setter)
        {
            var newValue = EditorGUILayout.TextField(key, value.Value<string>());
            if (newValue == value.Value<string>()) return false;
            setter(new Any(newValue));
            return true;
        }

        public static bool RenderBool(string key, Any value, Action<Any> setter)
        {
            var newValue = EditorGUILayout.Toggle(key, value.Value<bool>());
            if (newValue == value.Value<bool>()) return false;
            setter(new Any(newValue));
            return true;
        }

        public static bool RenderFloat(string key, Any value, Action<Any> setter)
        {
            var newValue = EditorGUILayout.FloatField(key, value.Value<float>());
            if (Math.Abs(newValue - value.Value<float>()) < float.Epsilon * 10) return false;
            setter(new Any(newValue));
            return true;
        }

        public static bool RenderDouble(string key, Any value, Action<Any> setter)
        {
            var newValue = EditorGUILayout.DoubleField(key, value.Value<double>());
            if (Math.Abs(newValue - value.Value<double>()) < double.Epsilon * 10) return false;
            setter(new Any(newValue));
            return true;
        }

        public static bool RenderEnum(string key, Any value, Action<Any> setter)
        {
            var newValue = EditorGUILayout.EnumPopup(key, value.Value<Enum>());
            if (newValue.Equals(value.Value<Enum>())) return false;
            setter(new Any(newValue));
            return true;
        }

        public static bool RenderVector2(string key, Any value, Action<Any> setter)
        {
            var newValue = EditorGUILayout.Vector2Field(key, value.Value<Vector2>());
            if (newValue != value.Value<Vector2>()) return false;
            setter(new Any(newValue));
            return true;
        }

        public static bool RenderVector3(string key, Any value, Action<Any> setter)
        {
            var newValue = EditorGUILayout.Vector3Field(key, value.Value<Vector3>());
            if (newValue != value.Value<Vector3>()) return false;
            setter(new Any(newValue));
            return true;
        }

        public static bool RenderVector4(string key, Any value, Action<Any> setter)
        {
            var newValue = EditorGUILayout.Vector4Field(key, value.Value<Vector4>());
            if (newValue != value.Value<Vector4>()) return false;
            setter(new Any(newValue));
            return true;
        }

        public static bool RenderColor(string key, Any value, Action<Any> setter)
        {
            var newValue = EditorGUILayout.ColorField(key, value.Value<Color>());
            if (newValue != value.Value<Color>()) return false;
            setter(new Any(newValue));
            return true;
        }

        public static bool RenderClass(string key, object objectValue)
        {
            var dirty = false;

            foreach (var field in objectValue.GetType().GetFields())
            {
                if (field.IsPublic || field.GetCustomAttribute(typeof(SerializeField)) != null)
                {
                    var fieldValue = field.GetValue(objectValue);
                    dirty |= RenderAny(field.Name, new Any(fieldValue, field.FieldType),
                        newValue => field.SetValue(objectValue, newValue.Object));
                }
            }
            
            return dirty;
        }

        public static bool RenderObject(string key, Any value, Action<Any> setter)
        {
            var newValue = EditorGUILayout.ObjectField(key, value.Value<UnityEngine.Object>(), value.Type, true, null);
            if (newValue == value.Value<UnityEngine.Object>()) return false;
            setter(new Any(newValue));
            return true;
        }

        public static bool RenderIList(string key, Any value, Action<Any> setter)
        {
            var dirty = false;
            var list = value.Value<IList>();
            var index = 0;

            foreach (var element in list)
            {
                dirty |= RenderAny($"{key}[{index}]", new Any(element), newValue => list[index] = newValue);
                index++;
            }

            return dirty;
        }

        public static bool RenderICollection(string key, Any value, Action<Any> setter)
        {
            var dirty = false;
            var list = value.Value<ICollection>();
            var index = 0;

            foreach (var element in list)
            {
                dirty |= RenderAny($"[{index}]", new Any(element), newValue => { });
                index++;
            }

            return dirty;
        }

        public static bool RenderIDictionary(string key, Any value, Action<Any> setter)
        {
            var dirty = false;
            var dictionary = value.Value<IDictionary>();

            foreach (var dictionaryKey in dictionary.Keys)
            {
                dirty |= RenderAny(dictionaryKey.ToString(), new Any(dictionary[key]),
                    newValue => dictionary[key] = newValue);
            }

            return dirty;
        }
    }
}