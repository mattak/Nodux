using System;
using System.Collections.Generic;
using System.Linq;
using Nodux.PluginNode;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

namespace Nodux.PluginEditor.NoduxGraph
{
    public class NoduxGraphSearchWindow : ScriptableObject, ISearchWindowProvider
    {
        private NoduxGraphView _graphView;
        private EditorWindow _window;
        private Texture2D _indentationIcon;

        public void Initialize(EditorWindow window, NoduxGraphView view)
        {
            _graphView = view;
            _window = window;

            // Indentation hack for search window
            _indentationIcon = new Texture2D(1, 1);
            _indentationIcon.SetPixel(0, 0, new Color(0, 0, 0, 0));
            _indentationIcon.Apply();
        }

        public List<SearchTreeEntry> CreateSearchTree(SearchWindowContext context)
        {
            var tree = new List<SearchTreeEntry>()
            {
                new SearchTreeGroupEntry(new GUIContent("Nodes"), 0),
                new SearchTreeGroupEntry(new GUIContent("All"), 1),
            };
            ListUpNodeTypes().ToList().ForEach(type =>
            {
                var entry = new SearchTreeEntry(new GUIContent(type.Name, _indentationIcon))
                {
                    userData = type,
                    level = 2
                };
                tree.Add(entry);
            });

            return tree;
        }

        public bool OnSelectEntry(SearchTreeEntry searchTreeEntry, SearchWindowContext context)
        {
            var worldMousePosition = _window.rootVisualElement.ChangeCoordinatesTo(
                _window.rootVisualElement,
                context.screenMousePosition - _window.position.position
            );
            var localMousePosition = _graphView.contentViewContainer.WorldToLocal(worldMousePosition);

            switch (searchTreeEntry.userData)
            {
                case Type type:
                    var nodeData = (INode) JsonUtility.FromJson("{}", type);
                    var node = NoduxGraphNodeCreator.Create(_graphView.SerializedGraph, nodeData, Vector2.zero);
                    node.SetPosition(new Rect(localMousePosition, new Vector2(320, 160)));
                    _graphView.AddElement(node);
                    return true;
                default:
                    return false;
            }
        }

        private IEnumerable<Type> ListUpNodeTypes()
        {
            return TypeSelectionHolder.GetTypeList("Node");
        }
    }
}