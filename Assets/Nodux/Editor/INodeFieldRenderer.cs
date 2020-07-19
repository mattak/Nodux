using System.Linq;
using System.Reflection;
using Nodux.PluginNode;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace Nodux.PluginEditor
{
    public static class INodeFieldRenderer
    {
        public static void Render(VisualElement container, SerializedProperty property, INode node)
        {
            var infos = node.GetType()
                .GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)
                .Where(field => field.IsPublic || field.GetCustomAttribute(typeof(SerializeField)) != null);

            foreach (var info in infos)
            {
                var fieldProperty = property.FindPropertyRelative(info.Name);
                var field = new PropertyField();

                if (fieldProperty != null)
                {
                    field.BindProperty(fieldProperty);
                    container.Add(field);
                }
                else
                {
                    Debug.LogWarning($"fieldProperty {info.Name} is null: {property.name} of {property.propertyPath}");
                }
            }
        }
    }
}