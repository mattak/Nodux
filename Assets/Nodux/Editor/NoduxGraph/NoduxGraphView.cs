using System;
using System.Collections.Generic;
using Nodux.PluginGraph;
using UniRx;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.UIElements;

namespace Nodux.PluginEditor.NoduxGraph
{
    public class NoduxGraphView : GraphView
    {
        private readonly Vector2 StartNodeOrigin = new Vector2(50, 50);

        private NoduxGraphWindowDelegate _windowDelegate;
        private NoduxGraphSearchWindow _searchWindow;
        private SerializedNoduxLinkGraph _serializedGraph;

        private GameObject _internalGameObject;
        private GameObject internalGameObject => _internalGameObject ?? (_internalGameObject = new GameObject());

        private NoduxLinkGraph _internalGraph => internalGameObject.GetComponent<NoduxLinkGraph>() ??
                                                 internalGameObject.AddComponent<NoduxLinkGraph>();

        public SerializedNoduxLinkGraph SerializedGraph =>
            _serializedGraph ?? (_serializedGraph = new SerializedNoduxLinkGraph(_internalGraph));

        public NoduxGraphView(NoduxGraphWindow window)
        {
            _windowDelegate = window;
            styleSheets.Add(Resources.Load<StyleSheet>("NoduxGraph"));
            SetupZoom(ContentZoomer.DefaultMinScale, ContentZoomer.DefaultMaxScale);

            this.AddManipulator(new ContentDragger());
            this.AddManipulator(new SelectionDragger());
            this.AddManipulator(new RectangleSelector());
            this.SetUpGrid();
            this.AddSearchWindow(window);
        }

        public void OnHierarchyChange()
        {
            var graphSceneName = _serializedGraph?.GraphSceneName;
            if (graphSceneName == null) return;

            for (var i = 0; i < EditorSceneManager.loadedSceneCount; i++)
            {
                var scene = EditorSceneManager.GetSceneAt(i);
                if (graphSceneName == scene.name) return;
            }

            NoduxGraphOperation.ClearGraph(this);
            _internalGameObject = null;
            _serializedGraph = null;
        }

        public void SetSerializeTarget(NoduxLinkGraph graph)
        {
            this._serializedGraph = new SerializedNoduxLinkGraph(graph);
            var title = SelectionUtil.GetSceneGameObjectPath(graph);
            _windowDelegate.SetGraphName(title);
        }

        public override List<Port> GetCompatiblePorts(Port startPort, NodeAdapter nodeAdapter)
        {
            var compatiblePorts = new List<Port>();
            ports.ForEach(port =>
            {
                if (startPort != port && startPort.node != port.node)
                {
                    compatiblePorts.Add(port);
                }
            });
            return compatiblePorts;
        }

        private void SetUpGrid()
        {
            var grid = new GridBackground();
            Insert(0, grid);
            grid.StretchToParentSize();
        }

        private void AddSearchWindow(EditorWindow editorWindow)
        {
            _searchWindow = ScriptableObject.CreateInstance<NoduxGraphSearchWindow>();
            _searchWindow.Initialize(editorWindow, this);
            nodeCreationRequest = context => SearchWindow.Open(
                new SearchWindowContext(context.screenMousePosition),
                _searchWindow
            );
        }

        public void AlignNodes(int columns)
        {
            var nodesList = NoduxGraphOperation.ExtractGraphNodeChains(this);
            if (nodesList == null) return;

            var origin = StartNodeOrigin;
            foreach (var nodes in nodesList)
            {
                var rect = NoduxGraphOperation.LayoutNodes(columns, origin, nodes);
                origin.y += rect.height;
            }
        }
    }
}