using System;
using System.Collections.Generic;
using Nodux.Core;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace Nodux.PluginEditor.NoduxGraph
{
    public class TypeSelectionSearchWindow : ScriptableObject, ISearchWindowProvider
    {
        private string _category;
        private Action<Type> _onSelect = null;
        private Texture2D _indentationIcon;

        public static void Show(TypeSelectionFilter enable, Action<Type> onSelected)
        {
            var provider = CreateInstance<TypeSelectionSearchWindow>();
            provider.Initialize(enable, (selectedType) =>
            {
                onSelected?.Invoke(selectedType);
                DestroyImmediate(provider);
            });

            SearchWindow.Open(
                new SearchWindowContext(
                    new Vector2(
                        0, // window.position.x + window.position.width / 2,
                        0 //window.position.y + 50
                    ),
                    200 // window.position.width
                ),
                provider
            );
        }

        public void Initialize(TypeSelectionFilter enable, Action<Type> onSelect)
        {
            _category = enable.Category;
            _onSelect = onSelect;
            TypeSelectionHolder.CheckInitialize(_category);

            // Indentation hack for search window
            _indentationIcon = new Texture2D(1, 1);
            _indentationIcon.SetPixel(0, 0, new Color(0, 0, 0, 0));
            _indentationIcon.Apply();
        }

        public List<SearchTreeEntry> CreateSearchTree(SearchWindowContext context)
        {
            var tree = new List<SearchTreeEntry>()
            {
                new SearchTreeGroupEntry(new GUIContent(_category), 0),
                new SearchTreeGroupEntry(new GUIContent("All"), 1),
            };

            foreach (var type in TypeSelectionHolder.GetTypeList(_category))
            {
                tree.Add(new SearchTreeEntry(new GUIContent(type.Name, _indentationIcon))
                {
                    userData = type,
                    level = 2,
                });
            }

            return tree;
        }

        public bool OnSelectEntry(SearchTreeEntry searchTreeEntry, SearchWindowContext context)
        {
            var type = (Type) searchTreeEntry.userData;
            _onSelect?.Invoke(type);
            return true;
        }
    }
}