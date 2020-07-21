using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace Nodux.PluginEditor
{
    public static class SerializedPropertyExtensions
    {
        public static T GetValue<T>(this SerializedProperty property)
        {
            return GetNestedObject<T>(property.propertyPath, GetSerializedPropertyRootComponent(property));
        }

        public static bool SetValue<T>(this SerializedProperty property, T value)
        {
            object obj = GetSerializedPropertyRootComponent(property);

            string[] fieldStructure = property.propertyPath.Split('.');
            for (int i = 0; i < fieldStructure.Length - 1; i++)
            {
                if (obj is IList && i < fieldStructure.Length - 1)
                {
                    i++;
                    obj = GetIListValue<object>(fieldStructure[i], obj);
                }
                else
                {
                    obj = GetFieldOrPropertyValue<object>(fieldStructure[i], obj);
                }
            }

            var fieldName = fieldStructure.Last();
            return SetFieldOrPropertyValue(fieldName, obj, value);
        }

        public static bool SetValueToIListIndex<T>(this SerializedProperty property, T value, int index)
        {
            object obj = GetSerializedPropertyRootComponent(property);

            string[] fieldStructure = property.propertyPath.Split('.');
            for (int i = 0; i < fieldStructure.Length; i++)
            {
                obj = GetFieldOrPropertyValue<object>(fieldStructure[i], obj);
            }

            if (obj is IList)
            {
                var list = (IList) obj;
                list[index] = value;
                return true;
            }

            return false;
        }

        public static Component GetSerializedPropertyRootComponent(SerializedProperty property)
        {
            return (Component) property.serializedObject.targetObject;
        }

        public static T GetNestedObject<T>(string path, object obj, bool includeAllBases = false)
        {
            var parts = path.Split('.');
            for (var i = 0; i < parts.Length; i++)
            {
                if (obj is IList && i < parts.Length - 1)
                {
                    i++;
                    obj = GetIListValue<object>(parts[i], obj);
                }
                else
                {
                    obj = GetFieldOrPropertyValue<object>(parts[i], obj);
                }
            }

            return (T) obj;
        }

        public static T GetIListValue<T>(string fieldName, object obj)
        {
            if (!(obj is IList)) return default;

            var start = fieldName?.IndexOf("[") ?? -1;
            var end = fieldName?.IndexOf("]") ?? -1;
            if (start == -1 || end == -1 || start >= end) return default(T);

            var indexText = fieldName.Substring(start + 1, end - start - 1);
            var index = Int32.Parse(indexText);
            var list = (IList) obj;
            if (index >= list.Count) return default;
            return (T) list[index];
        }

        public static T GetFieldOrPropertyValue<T>(string fieldName, object obj, bool includeAllBases = false,
            BindingFlags bindings = BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public |
                                    BindingFlags.NonPublic)
        {
            FieldInfo field = obj.GetType().GetField(fieldName, bindings);
            if (field != null) return (T) field.GetValue(obj);

            PropertyInfo property = obj.GetType().GetProperty(fieldName, bindings);
            if (property != null) return (T) property.GetValue(obj, null);

            if (includeAllBases)
            {
                foreach (Type type in GetBaseClassesAndInterfaces(obj.GetType()))
                {
                    field = type.GetField(fieldName, bindings);
                    if (field != null) return (T) field.GetValue(obj);

                    property = type.GetProperty(fieldName, bindings);
                    if (property != null) return (T) property.GetValue(obj, null);
                }
            }

            return default(T);
        }

        public static bool SetFieldOrPropertyValue(string fieldName, object obj, object value,
            bool includeAllBases = false,
            BindingFlags bindings = BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public |
                                    BindingFlags.NonPublic)
        {
            FieldInfo field = obj?.GetType()?.GetField(fieldName, bindings);
            if (field != null)
            {
                field.SetValue(obj, value);
                return true;
            }

            PropertyInfo property = obj.GetType().GetProperty(fieldName, bindings);
            if (property != null)
            {
                property.SetValue(obj, value, null);
                return true;
            }

            if (includeAllBases)
            {
                foreach (Type type in GetBaseClassesAndInterfaces(obj.GetType()))
                {
                    field = type.GetField(fieldName, bindings);
                    if (field != null)
                    {
                        field.SetValue(obj, value);
                        return true;
                    }

                    property = type.GetProperty(fieldName, bindings);
                    if (property != null)
                    {
                        property.SetValue(obj, value, null);
                        return true;
                    }
                }
            }

            return false;
        }

        public static IEnumerable<Type> GetBaseClassesAndInterfaces(this Type type, bool includeSelf = false)
        {
            var allTypes = new List<Type>();

            if (includeSelf) allTypes.Add(type);

            if (type.BaseType == typeof(object))
            {
                allTypes.AddRange(type.GetInterfaces());
            }
            else
            {
                allTypes.AddRange(
                    Enumerable
                        .Repeat(type.BaseType, 1)
                        .Concat(type.GetInterfaces())
                        .Concat(type.BaseType.GetBaseClassesAndInterfaces())
                        .Distinct());
            }

            return allTypes;
        }
    }
}