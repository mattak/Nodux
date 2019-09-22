using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using Nodux.Core;
using Nodux.PluginState;

namespace Nodux.PluginEditor
{
    public class StatePanel : EditorWindow
    {
        [MenuItem("Window/StatePanel %&l")]
        public static void Init()
        {
            var window = GetWindow<StatePanel>();
            window.titleContent.text = "State Panel";
            window.Show();
        }

        private Vector2 scrollPosition = Vector2.zero;
        private IStoreAccessor accessor;

        public void OnGUI()
        {
            this.CheckInitialize();

            if (accessor == null)
            {
                EditorGUILayout.HelpBox("Play to show properties", MessageType.Info);
                return;
            }

            this.Render(accessor);
        }

        public void Render(IStoreAccessor accessor)
        {
            this.scrollPosition = EditorGUILayout.BeginScrollView(this.scrollPosition);

            this.Render(accessor, accessor.GetRawData());

            EditorGUILayout.EndScrollView();
        }

        private bool Render(IStoreAccessor accessor, IDictionary<string, Any> map)
        {
            var dirty = false;

            foreach (var key in map.Keys.ToArray())
            {
                EditorGUILayout.LabelField(key);
                EditorGUI.indentLevel++;
                if (EditorGUILayoutRenderer.RenderSystemObject(
                    key, map[key].Type, map[key].Object, newValue => map[key] = new Any(newValue),
                    type => null))
                {
                    dirty = true;
                    accessor.Notify(key);
                }
                EditorGUI.indentLevel--;
            }

            return dirty;
        }

        private void CheckInitialize()
        {
            if (!Application.isPlaying)
            {
                accessor = null;
                return;
            }

            if (accessor != null) return;
            this.accessor = SingleStore.Instance?.GetStoreAccessor();
        }
    }
}