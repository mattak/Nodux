using System;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;
using Object = UnityEngine.Object;

namespace Nodux.PluginEditor
{
    public static class UIElementPrimitivePropertyUtil
    {
        public static void RenderStringProperty(
            VisualElement container,
            string name,
            object value,
            Action<object> setter)
        {
            var field = new TextField(name);
            field.SetValueWithoutNotify(value == null ? "" : value.ToString());
            field.MarkDirtyRepaint();
            field.RegisterValueChangedCallback(evt => setter(evt.newValue));
            container.Add(field);
        }

        public static void RenderIntProperty(
            VisualElement container,
            string name,
            object value,
            Action<object> setter)
        {
            var field = new IntegerField(name);
            field.SetValueWithoutNotify((int) value);
            field.MarkDirtyRepaint();
            field.RegisterValueChangedCallback(evt => setter(evt.newValue));
            container.Add(field);
        }

        public static void RenderFloatProperty(VisualElement container, string name, object value,
            Action<object> setter)
        {
            var field = new FloatField(name);
            field.SetValueWithoutNotify((float) value);
            field.MarkDirtyRepaint();
            field.RegisterValueChangedCallback(evt => setter(evt.newValue));
            container.Add(field);
        }

        public static void RenderBooleanProperty(VisualElement container, string name, object value,
            Action<object> setter)
        {
            var field = new Toggle(name);
            field.SetValueWithoutNotify((bool) value);
            field.MarkDirtyRepaint();
            field.RegisterValueChangedCallback(evt => setter(evt.newValue));
            container.Add(field);
        }

        public static void RenderLongProperty(VisualElement container, string name, object value, Action<object> setter)
        {
            var field = new LongField(name);
            field.SetValueWithoutNotify((long) value);
            field.MarkDirtyRepaint();
            field.RegisterValueChangedCallback(evt => setter(evt.newValue));
            container.Add(field);
        }

        public static void RenderDoubleProperty(VisualElement container, string name, object value,
            Action<object> setter)
        {
            var field = new DoubleField(name);
            field.SetValueWithoutNotify((double) value);
            field.MarkDirtyRepaint();
            field.RegisterValueChangedCallback(evt => setter(evt.newValue));
            container.Add(field);
        }

        public static void RenderEnumProperty(VisualElement container, string name, object value, Action<object> setter)
        {
            var field = new EnumField(name);
            field.SetValueWithoutNotify((Enum) value);
            field.MarkDirtyRepaint();
            field.RegisterValueChangedCallback(evt => setter(evt.newValue));
            container.Add(field);
        }

        public static void RenderVector2Property(VisualElement container, string name, object value,
            Action<object> setter)
        {
            var field = new Vector2Field(name);
            field.SetValueWithoutNotify((Vector2) value);
            field.MarkDirtyRepaint();
            field.RegisterValueChangedCallback(evt => setter(evt.newValue));
            container.Add(field);
        }

        public static void RenderVector3Property(VisualElement container, string name, object value,
            Action<object> setter)
        {
            var field = new Vector3Field(name);
            field.SetValueWithoutNotify((Vector3) value);
            field.MarkDirtyRepaint();
            field.RegisterValueChangedCallback(evt => setter(evt.newValue));
            container.Add(field);
        }

        public static void RenderVector2IntProperty(VisualElement container, string name, object value,
            Action<object> setter)
        {
            var field = new Vector2IntField(name);
            field.SetValueWithoutNotify((Vector2Int) value);
            field.MarkDirtyRepaint();
            field.RegisterValueChangedCallback(evt => setter(evt.newValue));
            container.Add(field);
        }

        public static void RenderVector3IntProperty(VisualElement container, string name, object value,
            Action<object> setter)
        {
            var field = new Vector3IntField(name);
            field.SetValueWithoutNotify((Vector3Int) value);
            field.MarkDirtyRepaint();
            field.RegisterValueChangedCallback(evt => setter(evt.newValue));
            container.Add(field);
        }

        public static void RenderColorProperty(VisualElement container, string name, object value,
            Action<object> setter)
        {
            var field = new ColorField(name);
            field.SetValueWithoutNotify((Color) value);
            field.MarkDirtyRepaint();
            field.RegisterValueChangedCallback(evt => setter(evt.newValue));
            container.Add(field);
        }

        public static void RenderVector4Property(VisualElement container, string name, object value,
            Action<object> setter)
        {
            var field = new Vector4Field(name);
            field.SetValueWithoutNotify((Vector4) value);
            field.MarkDirtyRepaint();
            field.RegisterValueChangedCallback(evt => setter(evt.newValue));
            container.Add(field);
        }

        public static void RenderRectProperty(VisualElement container, string name, object value, Action<object> setter)
        {
            var field = new RectField(name);
            field.SetValueWithoutNotify((Rect) value);
            field.MarkDirtyRepaint();
            field.RegisterValueChangedCallback(evt => setter(evt.newValue));
            container.Add(field);
        }

        public static void RenderCurveProperty(VisualElement container, string name, object value,
            Action<object> setter)
        {
            var field = new CurveField(name);
            field.SetValueWithoutNotify((AnimationCurve) value);
            field.MarkDirtyRepaint();
            field.RegisterValueChangedCallback(evt => setter(evt.newValue));
            container.Add(field);
        }

        public static void RenderGradientProperty(VisualElement container, string name, object value,
            Action<object> setter)
        {
            var field = new GradientField(name);
            field.SetValueWithoutNotify((Gradient) value);
            field.MarkDirtyRepaint();
            field.RegisterValueChangedCallback(evt => setter(evt.newValue));
            container.Add(field);
        }

        public static void RenderBoundsProperty(VisualElement container, string name, object value,
            Action<object> setter)
        {
            var field = new BoundsField(name);
            field.SetValueWithoutNotify((Bounds) value);
            field.MarkDirtyRepaint();
            field.RegisterValueChangedCallback(evt => setter(evt.newValue));
            container.Add(field);
        }

        public static void RenderUnityObjectProperty(
            VisualElement container,
            string name,
            object value,
            Action<object> setter,
            Type type)
        {
            var unityObject = (Object) value;
            var field = new ObjectField(name)
                {allowSceneObjects = true, objectType = type};
            field.SetValueWithoutNotify(unityObject);
            field.MarkDirtyRepaint();
            field.RegisterValueChangedCallback(evt => setter(evt.newValue));
            container.Add(field);
        }
    }
}