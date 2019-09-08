using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;
using UnityLeaf.Core;
using UnityLeaf.PluginLeaf;

namespace UnityLeaf.PluginEditor
{
//    [CustomEditor(typeof(LeafWrapperBehaviour))]
//    public class LeafWrapperBehaviourEditor : Editor
//    {
//        private static List<TypeSelectionFilter> FilterTypes = null;
//        private static List<Type> TypeList = null;
//        private static List<string> TypeFullNameList = null;
//
//        private float TotalHeight;
//
//        private void CheckInitialize()
//        {
//            if (FilterTypes == null)
//            {
//                FilterTypes = this.target.GetType().GetFields()
//                    .Select(it => it.GetCustomAttribute(typeof(TypeSelectionFilter)))
//                    .Where(it => it != null)
//                    .Select(it => (TypeSelectionFilter) it)
//                    .OrderBy(it => it.Category)
//                    .Distinct()
//                    .ToList();
//            }
//
//            if (TypeList == null)
//            {
//                var filters = FilterTypes.Select(it => it.Category).ToList();
//
//                TypeList = AppDomain.CurrentDomain.GetAssemblies().SelectMany(it => it.GetTypes())
//                    .Where(it => it.GetCustomAttribute(typeof(TypeSelectionEnable)) != null)
//                    .Select(it => new
//                    {
//                        Type = it,
//                        Category = ((TypeSelectionEnable) it.GetCustomAttribute(typeof(TypeSelectionEnable))).Category
//                    })
//                    .Where(it => filters.Any(filter => filter == it.Category))
//                    .Select(it => it.Type)
//                    .ToList();
//            }
//
//            if (TypeFullNameList == null)
//            {
//                TypeFullNameList = TypeList.Select(it => it.AssemblyQualifiedName).ToList();
//            }
//        }
//
//        public override void OnInspectorGUI()
//        {
//            this.CheckInitialize();
//
//            var selection = serializedObject.FindProperty("Selection");
//            if (selection == null) return;
//
//            if (this.RenderType(selection))
//            {
//                this.RenderValue(selection);
//            }
//        }
//
//        private bool RenderType(SerializedProperty property)
//        {
//            var fullName = property.FindPropertyRelative("FullName");
//            var selectIndex = TypeFullNameList.IndexOf(fullName.stringValue);
//            selectIndex = EditorGUILayout.Popup(
//                property.name,
//                selectIndex,
//                TypeList.Select(it => it.FullName).ToArray()
//            );
//
//            if (selectIndex >= 0)
//            {
//                fullName.stringValue = TypeFullNameList[selectIndex];
//                property.serializedObject.ApplyModifiedProperties();
//                return true;
//            }
//
//            return false;
//        }
//
//        private bool RenderValue(SerializedProperty property)
//        {
//            var fullName = property.FindPropertyRelative("FullName").stringValue;
//            var contentJson = property.FindPropertyRelative("ContentJson");
//            var type = Type.GetType(fullName);
//
//
//            if (type != null)
//            {
//                object obj = null;
//
//                try
//                {
//                    obj = JsonUtility.FromJson(contentJson.stringValue, type);
//                }
//                catch (ArgumentException e)
//                {
//                }
//
//                if (obj == null) obj = JsonUtility.FromJson("{}", type);
//
//                if (obj != null && PropertyRenderer.RenderClass("key", obj))
//                {
//                    contentJson.stringValue = JsonUtility.ToJson(obj);
//                    property.serializedObject.ApplyModifiedProperties();
//                    return true;
//                }
//            }
//            else
//            {
//                EditorGUILayout.HelpBox($"Not found serialized type: {fullName}", MessageType.Warning);
//            }
//
//            return false;
//        }
//    }
}