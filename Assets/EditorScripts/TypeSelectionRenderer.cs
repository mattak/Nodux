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

        private static IDictionary<string, TypeSelectionRenderer> RendererMap =
            new Dictionary<string, TypeSelectionRenderer>();

        private string FilterCategory = null;
        private IList<string> TypeNameList => TypeNameListMap[FilterCategory];
        private IList<string> TypeAssemblyQualifiedNameList => TypeAssemblyQualifiedNameListMap[FilterCategory];

        public static bool RenderFromSystemObject(
            ref Rect rect,
            string fieldName,
            TypeSelection selection,
            Func<Type, Attribute> attributeAccessor,
            Action<object> setter
        )
        {
            var attribute = (TypeSelectionFilter) attributeAccessor(typeof(TypeSelectionFilter));

            if (attribute != null)
            {
                if (!RendererMap.ContainsKey(attribute.Category))
                {
                    RendererMap[attribute.Category] = new TypeSelectionRenderer();
                    RendererMap[attribute.Category].CheckInitialize(attribute.Category);
                }

                var dirty = RendererMap[attribute.Category].RenderTypeAndValue(ref rect, fieldName, ref selection);
                if (dirty) setter(selection);
                return dirty;
            }

            return false;
        }

        public static bool RenderFromSystemObject(
            string fieldName,
            TypeSelection selection,
            Func<Type, Attribute> attributeAccessor,
            Action<object> setter
        )
        {
            var attribute = (TypeSelectionFilter) attributeAccessor(typeof(TypeSelectionFilter));

            if (attribute != null)
            {
                if (!RendererMap.ContainsKey(attribute.Category))
                {
                    RendererMap[attribute.Category] = new TypeSelectionRenderer();
                    RendererMap[attribute.Category].CheckInitialize(attribute.Category);
                }

                var dirty = RendererMap[attribute.Category].RenderTypeAndValue(fieldName, ref selection);
                if (dirty) setter(selection);
                return dirty;
            }

            return false;
        }

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

        public bool RenderTypeAndValue(string fieldName, ref TypeSelection selection)
        {
            var dirty = false;

            selection?.Restore();

            // type
            {
                dirty |= this.RenderType(fieldName, ref selection);
            }

            // value
            {
                dirty |= this.RenderValue(ref selection);
            }

            return dirty;
        }


        public bool RenderTypeAndValue(ref Rect position, string fieldName, ref TypeSelection selection)
        {
            var rect = new Rect(position.x, position.y, position.width, 0);
            var dirty = false;

            selection?.Restore();

            // type
            {
                dirty |= this.RenderType(ref rect, fieldName, ref selection);
                position.height = rect.height;
                rect.y += rect.height;
                rect.height = 0;
            }

            // value
            {
                dirty |= this.RenderValue(ref rect, ref selection);
                rect.y += rect.height;
                position.height += rect.height;
                rect.height = 0;
            }

            return dirty;
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

        public bool RenderType(string fieldName, ref TypeSelection selection)
        {
            var selectIndex = -1;
            if (selection != null)
            {
                if (selection.Type != null)
                {
                    selectIndex = TypeAssemblyQualifiedNameList.IndexOf(selection.Type.AssemblyQualifiedName);
                }
            }
            else
            {
                selection = new TypeSelection();
            }

            var newSelectIndex = EditorGUILayout.Popup(
                fieldName,
                selectIndex,
                TypeNameList.ToArray()
            );

            if (newSelectIndex != selectIndex)
            {
                selection.Type = TypeListMap[this.FilterCategory][newSelectIndex];
            }

            return true;
        }

        public bool RenderType(ref Rect position, string fieldName, ref TypeSelection selection)
        {
            var selectIndex = -1;
            if (selection != null)
            {
                if (selection.Type != null)
                {
                    selectIndex = TypeAssemblyQualifiedNameList.IndexOf(selection.Type.AssemblyQualifiedName);
                }
            }
            else
            {
                selection = new TypeSelection();
            }

            var rect = new Rect(position.x, position.y, position.width, EditorGUIUtility.singleLineHeight);
            var newSelectIndex = EditorGUI.Popup(
                rect,
                fieldName,
                selectIndex,
                TypeNameList.ToArray()
            );
            position.height += rect.height;
            rect.y += rect.height;
            rect.height = 0;

            if (newSelectIndex != selectIndex)
            {
                selection.Type = TypeListMap[this.FilterCategory][newSelectIndex];
            }

            return true;
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

        public bool RenderValue(ref TypeSelection selection)
        {
            if (selection.Type != null)
            {
                object obj = selection.Object;
                if (obj == null) obj = JsonUtility.FromJson("{}", selection.Type);
                if (obj != null)
                {
                    var dirty = EditorGUILayoutRenderer.RenderClass("class", obj);

                    if (dirty)
                    {
                        selection.SetValue(obj);
                    }

                    return true;
                }
            }
            else
            {
                EditorGUILayout.HelpBox($"Not found serialized type on selection: {selection}", MessageType.Warning);
            }

            return false;
        }

        public bool RenderValue(ref Rect position, ref TypeSelection selection)
        {
            if (selection.Type != null)
            {
                object obj = selection.Object;
                if (obj == null) obj = JsonUtility.FromJson("{}", selection.Type);
                if (obj != null)
                {
                    var rect = new Rect(position.x, position.y, position.width, 0);
                    var dirty = EditorGUIPropertyRenderer.RenderClass(ref rect, null, obj);
                    position.height += rect.height;

                    if (dirty)
                    {
                        selection.SetValue(obj);
                        selection.PrepareSerialize();
                    }

                    return true;
                }
            }
            else
            {
                var rect = new Rect(position.x, position.y, position.width, EditorGUIUtility.singleLineHeight);
                EditorGUI.HelpBox(rect, $"Not found serialized type on selection: {selection}", MessageType.Warning);
                position.height += rect.height;
            }

            return false;
        }

        public bool RenderValue(ref Rect position, SerializedProperty property)
        {
            var fullName = property.FindPropertyRelative("FullName").stringValue;
            var contentJson = property.FindPropertyRelative("ContentJson");
            var unityObjects = property.FindPropertyRelative("UnityObjects");

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
                    if (unityObjects.arraySize > 0)
                    {
                        TypeUtil.RestoreUnityObjects(type, obj, GetSerializedUnityObjects(unityObjects));
                    }

                    var rect = new Rect(position.x, position.y, position.width, 0);
                    var dirty = EditorGUIPropertyRenderer.RenderClass(ref rect, null, obj);
                    position.height += rect.height;

                    if (dirty)
                    {
                        contentJson.stringValue = JsonUtility.ToJson(obj);
                        var array = TypeUtil.ListUpUnityObjects(type, obj);
                        unityObjects.arraySize = array.Length;
                        property.serializedObject.ApplyModifiedProperties();

                        for (var i = 0; i < array.Length; i++)
                        {
                            unityObjects.GetArrayElementAtIndex(i).objectReferenceValue = array[i];
                        }

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

        public static UnityEngine.Object[] GetSerializedUnityObjects(SerializedProperty property)
        {
            var list = new List<UnityEngine.Object>();
            for (var i = 0; i < property.arraySize; i++)
            {
                list.Add(property.GetArrayElementAtIndex(i).objectReferenceValue);
            }

            return list.ToArray();
        }
    }
}