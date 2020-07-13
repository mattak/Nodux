using System.Linq;
using System.Reflection;
using Nodux.Core;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace Nodux.PluginEditor.NoduxGraph
{
    public static class NoduxGraphPropertyUtil
    {
        public static void CreateProperties(NoduxGraphNode node, TypeSelection selection)
        {
            if (selection.Type == null || selection.Object == null)
            {
                node.extensionContainer.Add(new Label("no properties"));
                return;
            }

            var fields = selection.Type
                .GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)
                .Where(field => field.IsPublic || field.GetCustomAttribute(typeof(SerializeField)) != null);

            foreach (var field in fields)
            {
                if (field.FieldType == typeof(string)) CreateStringProperty(node, field, selection);
                else if (field.FieldType == typeof(int)) CreateIntProperty(node, field, selection);
                else if (field.FieldType == typeof(float)) CreateFloatProperty(node, field, selection);
                else if (field.FieldType == typeof(bool)) CreateBooleanProperty(node, field, selection);
                else if (field.FieldType == typeof(long)) CreateLongProperty(node, field, selection);
                else if (field.FieldType == typeof(double)) CreateDoubleProperty(node, field, selection);
                else if (field.FieldType == typeof(System.Enum)) CreateEnumProperty(node, field, selection);
                else if (field.FieldType == typeof(Vector2)) CreateVector2Property(node, field, selection);
                else if (field.FieldType == typeof(Vector3)) CreateVector3Property(node, field, selection);
                else if (field.FieldType == typeof(Vector2Int)) CreateVector2IntProperty(node, field, selection);
                else if (field.FieldType == typeof(Vector3Int)) CreateVector3IntProperty(node, field, selection);
                else if (field.FieldType == typeof(Color)) CreateColorProperty(node, field, selection);
                else if (field.FieldType == typeof(Vector4)) CreateVector4Property(node, field, selection);
                else if (field.FieldType == typeof(Rect)) CreateRectProperty(node, field, selection);
                else if (field.FieldType == typeof(AnimationCurve)) CreateCurveProperty(node, field, selection);
                else if (field.FieldType == typeof(Gradient)) CreateGradientProperty(node, field, selection);
                else if (field.FieldType == typeof(Bounds)) CreateBoundsProperty(node, field, selection);
                else if (typeof(UnityEngine.Object).IsAssignableFrom(field.FieldType))
                {
                    CreateUnityObjectProperty(node, field, selection);
                }
                else if (field.GetCustomAttribute<TypeSelectionFilter>() != null)
                {
                    CreateTypeSelectionFilterProperty(node, field, selection);
                }
                else CreateUndefinedProperty(node, field, selection);
            }
        }

        private static void CreateStringProperty(NoduxGraphNode node, FieldInfo info, TypeSelection selection)
        {
            var field = new TextField(info.Name);
            var value = info.GetValue(selection.Object);
            field.SetValueWithoutNotify(value == null ? "" : value.ToString());
            field.MarkDirtyRepaint();
            field.RegisterValueChangedCallback(evt => selection.UpdateValue(info, evt.newValue));
            node.extensionContainer.Add(field);
        }

        private static void CreateIntProperty(NoduxGraphNode node, FieldInfo info, TypeSelection selection)
        {
            var field = new IntegerField(info.Name);
            var value = (int) info.GetValue(selection.Object);
            field.SetValueWithoutNotify(value);
            field.MarkDirtyRepaint();
            field.RegisterValueChangedCallback(evt => selection.UpdateValue(info, evt.newValue));
            node.extensionContainer.Add(field);
        }

        private static void CreateFloatProperty(NoduxGraphNode node, FieldInfo info, TypeSelection selection)
        {
            var field = new FloatField(info.Name);
            var value = (float) info.GetValue(selection.Object);
            field.SetValueWithoutNotify(value);
            field.MarkDirtyRepaint();
            field.RegisterValueChangedCallback(evt => selection.UpdateValue(info, evt.newValue));
            node.extensionContainer.Add(field);
        }

        private static void CreateBooleanProperty(NoduxGraphNode node, FieldInfo info, TypeSelection selection)
        {
            var field = new Toggle(info.Name);
            var value = (bool) info.GetValue(selection.Object);
            field.SetValueWithoutNotify(value);
            field.MarkDirtyRepaint();
            field.RegisterValueChangedCallback(evt => selection.UpdateValue(info, evt.newValue));
            node.extensionContainer.Add(field);
        }

        private static void CreateLongProperty(NoduxGraphNode node, FieldInfo info, TypeSelection selection)
        {
            var field = new LongField(info.Name);
            var value = (long) info.GetValue(selection.Object);
            field.SetValueWithoutNotify(value);
            field.MarkDirtyRepaint();
            field.RegisterValueChangedCallback(evt => selection.UpdateValue(info, evt.newValue));
            node.extensionContainer.Add(field);
        }

        private static void CreateDoubleProperty(NoduxGraphNode node, FieldInfo info, TypeSelection selection)
        {
            var field = new DoubleField(info.Name);
            var value = (double) info.GetValue(selection.Object);
            field.SetValueWithoutNotify(value);
            field.MarkDirtyRepaint();
            field.RegisterValueChangedCallback(evt => selection.UpdateValue(info, evt.newValue));
            node.extensionContainer.Add(field);
        }

        private static void CreateEnumProperty(NoduxGraphNode node, FieldInfo info, TypeSelection selection)
        {
            var field = new EnumField(info.Name);
            var value = (System.Enum) info.GetValue(selection.Object);
            field.SetValueWithoutNotify(value);
            field.MarkDirtyRepaint();
            field.RegisterValueChangedCallback(evt => selection.UpdateValue(info, evt.newValue));
            node.extensionContainer.Add(field);
        }

        private static void CreateVector2Property(NoduxGraphNode node, FieldInfo info, TypeSelection selection)
        {
            var field = new Vector2Field(info.Name);
            var value = (Vector2) info.GetValue(selection.Object);
            field.SetValueWithoutNotify(value);
            field.MarkDirtyRepaint();
            field.RegisterValueChangedCallback(evt => selection.UpdateValue(info, evt.newValue));
            node.extensionContainer.Add(field);
        }

        private static void CreateVector3Property(NoduxGraphNode node, FieldInfo info, TypeSelection selection)
        {
            var field = new Vector3Field(info.Name);
            var value = (Vector3) info.GetValue(selection.Object);
            field.SetValueWithoutNotify(value);
            field.MarkDirtyRepaint();
            field.RegisterValueChangedCallback(evt => selection.UpdateValue(info, evt.newValue));
            node.extensionContainer.Add(field);
        }

        private static void CreateVector2IntProperty(NoduxGraphNode node, FieldInfo info, TypeSelection selection)
        {
            var field = new Vector2IntField(info.Name);
            var value = (Vector2Int) info.GetValue(selection.Object);
            field.SetValueWithoutNotify(value);
            field.MarkDirtyRepaint();
            field.RegisterValueChangedCallback(evt => selection.UpdateValue(info, evt.newValue));
            node.extensionContainer.Add(field);
        }

        private static void CreateVector3IntProperty(NoduxGraphNode node, FieldInfo info, TypeSelection selection)
        {
            var field = new Vector3IntField(info.Name);
            var value = (Vector3Int) info.GetValue(selection.Object);
            field.SetValueWithoutNotify(value);
            field.MarkDirtyRepaint();
            field.RegisterValueChangedCallback(evt => selection.UpdateValue(info, evt.newValue));
            node.extensionContainer.Add(field);
        }

        private static void CreateColorProperty(NoduxGraphNode node, FieldInfo info, TypeSelection selection)
        {
            var field = new ColorField(info.Name);
            var value = (Color) info.GetValue(selection.Object);
            field.SetValueWithoutNotify(value);
            field.MarkDirtyRepaint();
            field.RegisterValueChangedCallback(evt => selection.UpdateValue(info, evt.newValue));
            node.extensionContainer.Add(field);
        }

        private static void CreateVector4Property(NoduxGraphNode node, FieldInfo info, TypeSelection selection)
        {
            var field = new Vector4Field(info.Name);
            var value = (Vector4) info.GetValue(selection.Object);
            field.SetValueWithoutNotify(value);
            field.MarkDirtyRepaint();
            field.RegisterValueChangedCallback(evt => selection.UpdateValue(info, evt.newValue));
            node.extensionContainer.Add(field);
        }

        private static void CreateRectProperty(NoduxGraphNode node, FieldInfo info, TypeSelection selection)
        {
            var field = new RectField(info.Name);
            var value = (Rect) info.GetValue(selection.Object);
            field.SetValueWithoutNotify(value);
            field.MarkDirtyRepaint();
            field.RegisterValueChangedCallback(evt => selection.UpdateValue(info, evt.newValue));
            node.extensionContainer.Add(field);
        }

        private static void CreateCurveProperty(NoduxGraphNode node, FieldInfo info, TypeSelection selection)
        {
            var field = new CurveField(info.Name);
            var value = (AnimationCurve) info.GetValue(selection.Object);
            field.SetValueWithoutNotify(value);
            field.MarkDirtyRepaint();
            field.RegisterValueChangedCallback(evt => selection.UpdateValue(info, evt.newValue));
            node.extensionContainer.Add(field);
        }

        private static void CreateGradientProperty(NoduxGraphNode node, FieldInfo info, TypeSelection selection)
        {
            var field = new GradientField(info.Name);
            var value = (Gradient) info.GetValue(selection.Object);
            field.SetValueWithoutNotify(value);
            field.MarkDirtyRepaint();
            field.RegisterValueChangedCallback(evt => selection.UpdateValue(info, evt.newValue));
            node.extensionContainer.Add(field);
        }

        private static void CreateBoundsProperty(NoduxGraphNode node, FieldInfo info, TypeSelection selection)
        {
            var field = new BoundsField(info.Name);
            var value = (Bounds) info.GetValue(selection.Object);
            field.SetValueWithoutNotify(value);
            field.MarkDirtyRepaint();
            field.RegisterValueChangedCallback(evt => selection.UpdateValue(info, evt.newValue));
            node.extensionContainer.Add(field);
        }

        private static void CreateUnityObjectProperty(NoduxGraphNode node, FieldInfo info, TypeSelection selection)
        {
            var field = new ObjectField(info.Name) {allowSceneObjects = true, objectType = info.FieldType};
            var value = (UnityEngine.Object) info.GetValue(selection.Object);
            field.SetValueWithoutNotify(value);
            field.MarkDirtyRepaint();
            field.RegisterValueChangedCallback(evt => { selection.UpdateValue(info, evt.newValue); });
            node.extensionContainer.Add(field);
        }

        private static void CreateTypeSelectionFilterProperty(NoduxGraphNode node, FieldInfo info,
            TypeSelection selection)
        {
            var enable = info.GetCustomAttribute<TypeSelectionFilter>();
            var field = new Button();
            var value = (TypeSelection) info.GetValue(selection.Object);
            value?.Restore(withWarning: false);
            var fieldText = (value == null || value.Object == null) ? "null" : value.Object.ToString();
            field.text = fieldText;
            field.MarkDirtyRepaint();
            field.clickable.clicked += () =>
            {
                TypeSelectionSearchWindow.Show(enable,
                    type =>
                    {
                        field.text = type.Name;
                        var instance = JsonUtility.FromJson("{}", type);
                        selection.UpdateValue(info, new TypeSelection(instance));
                    });
            };
            node.extensionContainer.Add(field);
        }

        private static void CreateUndefinedProperty(NoduxGraphNode node, FieldInfo info, TypeSelection selection)
        {
            var textField = new TextField(info.Name);
            var value = info.GetValue(selection.Object);
            textField.SetEnabled(false);
            textField.SetValueWithoutNotify(value == null ? "null" : value.ToString());
            textField.MarkDirtyRepaint();
            node.extensionContainer.Add(textField);
        }
    }
}