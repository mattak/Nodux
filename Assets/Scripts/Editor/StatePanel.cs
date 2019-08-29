using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityLeaf.Core;
using UnityLeaf.PluginState;

namespace UnityLeaf.PluginEditor
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
        private StoreHolder holder;
        private IStoreAccessor accessor;

        public void OnGUI()
        {
            this.CheckInitialize();

            if (holder == null)
            {
                EditorGUILayout.HelpBox("Not found StateHolder", MessageType.Warning);
                return;
            }

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
                if (PropertyRenderer.RenderAny(key, map[key], newValue => map[key] = newValue))
                {
                    dirty = true;
                    accessor.Notify(key);
                }
            }

            return dirty;
        }

        private void CheckInitialize()
        {
            if (accessor != null) return;
            this.holder = FindObjectOfType<StoreHolder>();
            this.accessor = holder?.GetStoreAccessor();
        }
    }
}