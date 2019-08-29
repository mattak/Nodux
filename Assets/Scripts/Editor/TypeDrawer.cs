using System;
using System.Collections.Generic;
using System.Linq;
using UnityLeaf.Core;
using UnityEditor;
using UnityEngine;

namespace UnityLeaf.PluginEditor
{
    [CustomPropertyDrawer(typeof(TypeSelection))]
    public class TypeDrawer : PropertyDrawer
    {
        private static readonly List<Type> TypeList = new List<Type>
        {
            typeof(int),
            typeof(bool),
            typeof(string),
        };

        private static List<string> TypeFullNameList = null;

        private float TotalHeight;

        private void CheckInitialize()
        {
            if (TypeFullNameList == null)
            {
                TypeFullNameList = TypeList.Select(it => it.FullName).ToList();
            }
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            this.CheckInitialize();

            EditorGUI.BeginProperty(position, label, property);

            var rectPopup = new Rect(position.x, position.y, position.width, 18f);
            this.TotalHeight = rectPopup.height;

            var fullName = property.FindPropertyRelative("FullName");

            // FIXME: 要素数増やすときに考える
            {
                var selectIndex = TypeFullNameList.IndexOf(fullName.stringValue);
                selectIndex = EditorGUI.Popup(
                    rectPopup,
                    property.name,
                    selectIndex,
                    TypeList.Select(it => it.FullName).ToArray()
                );

                if (selectIndex >= 0)
                {
                    fullName.stringValue = TypeFullNameList[selectIndex];
                    property.serializedObject.ApplyModifiedProperties();
                }
            }


            EditorGUI.EndProperty();
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return this.TotalHeight;
        }
    }
}