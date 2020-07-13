using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

namespace Nodux.PluginEditor.NoduxGraph
{
    public class NoduxGraphView : GraphView
    {
        private readonly Vector2 StartNodeOrigin = new Vector2(50, 50);

        private NoduxGraphWindowDelegate _windowDelegate;

        // private NoduxGraphNode[][] _nodesList;
        private NoduxGraphSearchWindow _searchWindow;

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
            var nodesList = NoduxGraphUtil.ExtractGraphNodeChains(this);
            if (nodesList == null) return;

            var origin = StartNodeOrigin;
            foreach (var nodes in nodesList)
            {
                var rect = NoduxGraphUtil.LayoutNodes(columns, origin, nodes);
                origin.y += rect.height;
            }
        }

        private void SetGraphTitleBySelection()
        {
            var graphTitle = SelectionUtil.GetSceneGameObjectPath();
            _windowDelegate.SetGraphName(graphTitle);
        }
    }
}