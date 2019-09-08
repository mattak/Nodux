using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;
using UnityLeaf.Core;

namespace UnityLeaf.PluginEditor
{
    public class TypeSelectionRenderer
    {
        private static List<Type> CandidateTypeList = null;
        private static IDictionary<string, List<Type>> TypeListMap = new Dictionary<string, List<Type>>();
        private static IDictionary<string, List<string>> TypeNameListMap = new Dictionary<string, List<string>>();

        private static IDictionary<string, List<string>> TypeAssemblyQualifiedNameListMap =
            new Dictionary<string, List<string>>();

        private string FilterCategory = null;
        private IList<string> TypeNameList => TypeNameListMap[FilterCategory];
        private IList<string> TypeAssemblyQualifiedNameList => TypeAssemblyQualifiedNameListMap[FilterCategory];

        public void CheckInitialize(string filterCategory)
        {
            if (CandidateTypeList == null)
            {
                CandidateTypeList = AppDomain.CurrentDomain.GetAssemblies().SelectMany(it => it.GetTypes())
                    .Where(it => it.GetCustomAttribute(typeof(TypeSelectionEnable)) != null)
                    .ToList();
            }

            if (FilterCategory == null)
            {
                FilterCategory = filterCategory;
            }

            if (FilterCategory != null && CandidateTypeList != null && !TypeListMap.ContainsKey(filterCategory))
            {
                TypeListMap[FilterCategory] = CandidateTypeList
                    .Select(it => new
                    {
                        Type = it,
                        Category = ((TypeSelectionEnable) it.GetCustomAttribute(typeof(TypeSelectionEnable))).Category
                    })
                    .Where(it => filterCategory == it.Category)
                    .Select(it => it.Type)
                    .ToList();

                TypeAssemblyQualifiedNameListMap[FilterCategory] =
                    TypeListMap[FilterCategory].Select(it => it.AssemblyQualifiedName).ToList();
                TypeNameListMap[FilterCategory] =
                    TypeListMap[FilterCategory].Select(it => it.FullName).ToList();
            }
        }

        public bool RenderTypeAndValue(ref Rect position, SerializedProperty property)
        {
            var rect = new Rect(position.x, position.y, position.width, 0);
            var dirty = false;

            // type
            {
                dirty |= this.RenderType(ref rect, property);
                position.height = rect.height;
                rect.y += rect.height;
                rect.height = 0;
            }

            // value
            {
                dirty |= this.RenderValue(ref rect, property);
                rect.y += rect.height;
                position.height += rect.height;
                rect.height = 0;
            }

            return dirty;
        }

        public bool RenderType(ref Rect position, SerializedProperty property)
        {
            var fullName = property.FindPropertyRelative("FullName");
            var selectIndex = TypeAssemblyQualifiedNameList.IndexOf(fullName.stringValue);
            var rect = new Rect(position.x, position.y, position.width, EditorGUIUtility.singleLineHeight);

            var newSelectIndex = EditorGUI.Popup(
                rect,
                "Type",
                selectIndex,
                TypeNameList.ToArray()
            );
            position.height += rect.height;

            if (newSelectIndex != selectIndex)
            {
                fullName.stringValue = TypeAssemblyQualifiedNameList[newSelectIndex];
                property.serializedObject.ApplyModifiedProperties();
                return true;
            }

            return false;
        }

        public bool RenderValue(ref Rect position, SerializedProperty property)
        {
            var fullName = property.FindPropertyRelative("FullName").stringValue;
            var contentJson = property.FindPropertyRelative("ContentJson");

            var type = Type.GetType(fullName);
            if (type != null)
            {
                object obj = null;

                try
                {
                    obj = JsonUtility.FromJson(contentJson.stringValue, type);
                }
                catch (ArgumentException)
                {
                }

                if (obj == null) obj = JsonUtility.FromJson("{}", type);

                if (obj != null)
                {
                    var rect = new Rect(position.x, position.y, position.width, 0);
                    var dirty = EditorGUIPropertyRenderer.RenderClass(ref rect, null, obj);
                    position.height += rect.height;

                    if (dirty)
                    {
                        contentJson.stringValue = JsonUtility.ToJson(obj);
                        property.serializedObject.ApplyModifiedProperties();
                    }

                    return true;
                }
            }
            else
            {
                var rect = new Rect(position.x, position.y, position.width, EditorGUIUtility.singleLineHeight);
                EditorGUI.HelpBox(rect, $"Not found serialized type: {fullName}", MessageType.Warning);
                position.height += rect.height;
            }

            return false;
        }
    }
}