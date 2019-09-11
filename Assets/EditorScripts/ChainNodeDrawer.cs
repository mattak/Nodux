using UnityEditor;
using UnityEngine;
using UnityLeaf.PluginLeaf;

namespace UnityLeaf.PluginEditor
{
    [CustomPropertyDrawer(typeof(ChainNode))]
    public class ChainNodeDrawer : PropertyDrawer
    {
        private float TotalHeight = 0;
        private TypeSelectionRenderer _renderer = new TypeSelectionRenderer();

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);
            this.TotalHeight = 0;
            position.height = 0;

            var dirty = false;
            var arraySize = 0;


            var rect = new Rect(position.x, position.y, position.width, 0);

            // set node size
            {
                dirty |= this.RenderCount(ref rect, property, "Count", ref arraySize);

                position.height += rect.height;
                rect.y += rect.height;
                rect.height = 0;
            }

            // show node list
            for (var i = 0; i < arraySize; i++)
            {
                dirty |= this.RenderClass(ref rect, property, i);

                position.height += rect.height;
                rect.y += rect.height;
                rect.height = 0;
            }

            this.TotalHeight = position.height;
            EditorGUI.EndProperty();
        }

        private bool RenderCount(ref Rect position, SerializedProperty property, string label, ref int size)
        {
            var rect = new Rect(position.x, position.y, position.width, EditorGUIUtility.singleLineHeight);
            var nodeList = property.FindPropertyRelative("NodeList");
            size = nodeList.arraySize;

            var newSize = EditorGUI.IntField(rect, label, size, EditorStyles.numberField);
            position.height += rect.height;

            if (size != newSize)
            {
                nodeList.arraySize = newSize;
                property.serializedObject.ApplyModifiedProperties();
                return true;
            }

            return false;
        }

        private void RenderLabel(ref Rect position, string label)
        {
            var rect = new Rect(position.x, position.y, position.width, EditorGUIUtility.singleLineHeight);
            EditorGUI.LabelField(rect, label);
            position.height += rect.height;
        }

        private bool RenderClass(ref Rect position, SerializedProperty property, int index)
        {
            var nodeList = property.FindPropertyRelative("NodeList");
            if (index >= nodeList.arraySize) return false;
            var elementProperty = nodeList.GetArrayElementAtIndex(index);
            var rect = new Rect(position.x, position.y, position.width, position.height);

            RenderLabel(ref rect, $"Node[{index}]");
            position.height += rect.height;
            rect.y += rect.height;
            rect.height = 0;

            if (index == 0) _renderer.CheckInitialize("Node");
            rect.x += EditorGUIUtility.singleLineHeight;
            rect.width -= EditorGUIUtility.singleLineHeight;
            var dirty = _renderer.RenderTypeAndValue(ref rect, elementProperty);
            position.height += rect.height;

            return dirty;
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return this.TotalHeight;
        }
    }
}