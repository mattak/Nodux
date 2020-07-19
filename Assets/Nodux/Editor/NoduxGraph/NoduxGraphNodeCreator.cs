using System;
using Nodux.PluginGraph;
using Nodux.PluginNode;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;
using Node = UnityEditor.Experimental.GraphView.Node;

namespace Nodux.PluginEditor.NoduxGraph
{
    public static class NoduxGraphNodeCreator
    {
        private static readonly Vector2 DefaultNodeSize = new Vector2(150, 200);

        public static NoduxGraphNodeView Load(SerializedNoduxLinkGraph serializedObject, GraphNodeData inspectorNode,
            int index)
        {
            var graphNode = new NoduxGraphNodeView()
            {
                title = inspectorNode.Name,
                name = inspectorNode.Name,
                Data = inspectorNode.Node,
                Guid = inspectorNode.Guid
            };

            var inputPort = GenerateInputPort(graphNode);
            var outputPort = GenerateOutputPort(graphNode);

            var property = serializedObject.GetNode(index);
            INodeFieldRenderer.Render(graphNode.extensionContainer, property, inspectorNode.Node);

            graphNode.styleSheets.Add(Resources.Load<StyleSheet>("NoduxGraphNode"));
            graphNode.RefreshPorts();
            graphNode.RefreshExpandedState();
            graphNode.SetPosition(new Rect(inspectorNode.Position, DefaultNodeSize));
            return graphNode;
        }

        public static NoduxGraphNodeView Create(SerializedNoduxLinkGraph serializedObject, INode nodeData,
            Vector2 position)
        {
            var type = nodeData.GetType();
            var name = type.Name;
            var inspectorNode = new GraphNodeData()
            {
                Guid = Guid.NewGuid().ToString(),
                Name = name,
                Position = position,
                Node = nodeData,
            };

            var nodeView = new NoduxGraphNodeView()
            {
                title = inspectorNode.Name,
                name = inspectorNode.Name,
                Data = inspectorNode.Node,
                Guid = inspectorNode.Guid
            };

            var inputPort = GenerateInputPort(nodeView);
            var outputPort = GenerateOutputPort(nodeView);

            var index = serializedObject.AddNode(inspectorNode);
            var property = serializedObject.GetNode(index);
            INodeFieldRenderer.Render(nodeView.extensionContainer, property, inspectorNode.Node);

            nodeView.styleSheets.Add(Resources.Load<StyleSheet>("NoduxGraphNode"));
            nodeView.RefreshPorts();
            nodeView.RefreshExpandedState();
            nodeView.SetPosition(new Rect(inspectorNode.Position, DefaultNodeSize));
            return nodeView;
        }

        private static Port GenerateInputPort(Node node)
        {
            var port = GeneratePort(node, Direction.Input);
            port.portName = "Input";
            node.inputContainer.Add(port);
            return port;
        }

        private static Port GenerateOutputPort(Node node)
        {
            var port = GeneratePort(node, Direction.Output);
            port.portName = "Output";
            node.outputContainer.Add(port);
            return port;
        }

        private static Port GeneratePort(
            Node node,
            Direction portDirection,
            Port.Capacity capacity = Port.Capacity.Single)
        {
            return node.InstantiatePort(Orientation.Horizontal, portDirection, capacity, typeof(float));
        }
    }
}