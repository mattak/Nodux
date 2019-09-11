using System;
using System.Reflection;
using UnityEngine;

namespace UnityLeaf.PluginEditor
{
    public static class TypeUtil
    {
        public static int CountUnityObjects(Type type)
        {
            var count = 0;

            foreach (var field in type.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance))
            {
                if (field.IsPublic || field.GetCustomAttribute(typeof(SerializeField)) != null)
                {
                    if (typeof(UnityEngine.Object).IsAssignableFrom(field.FieldType)) count++;
                }
            }

            return count;
        }
    }
}