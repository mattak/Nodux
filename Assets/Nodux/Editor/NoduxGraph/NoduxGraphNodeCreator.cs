using System;
using Nodux.Core;
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

        public static NoduxGraphNode Create(GraphNodeData nodeData)
        {
            var type = nodeData.Node.GetType();
            var name = type.Name;
            var node = new NoduxGraphNode()
            {
                title = name,
                name = name,
                Data = nodeData.Node,
                Guid = nodeData.Guid
            };

            var inputPort = GenerateInputPort(node);
            var outputPort = GenerateOutputPort(node);

            // NoduxGraphPropertyUtil.CreateProperties(node, nodeData.Data);
            UIElementPropertyRenderer.RenderClass(
                node.extensionContainer,
                nodeData.Node.GetType(),
                nodeData.Node,
                newValue => node.Data = (PluginNode.Node) newValue
            );

            node.styleSheets.Add(Resources.Load<StyleSheet>("NoduxGraphNode"));
            node.RefreshPorts();
            node.RefreshExpandedState();
            node.SetPosition(new Rect(nodeData.Position, DefaultNodeSize));
            return node;
        }

        public static NoduxGraphNode Create(INode nodeData, Vector2 position)
        {
            var type = nodeData.GetType();
            var name = type.Name;
            var node = new NoduxGraphNode()
            {
                title = name,
                name = name,
                Data = nodeData,
                Guid = Guid.NewGuid().ToString(),
            };

            var inputPort = GenerateInputPort(node);
            var outputPort = GenerateOutputPort(node);

            // NoduxGraphPropertyUtil.Create(node, nodeContent);
            UIElementPropertyRenderer.RenderClass(
                node.extensionContainer,
                type,
                nodeData,
                newValue => node.Data = (INode) newValue
            );

            node.styleSheets.Add(Resources.Load<StyleSheet>("NoduxGraphNode"));
            node.RefreshPorts();
            node.RefreshExpandedState();
            node.SetPosition(new Rect(position, DefaultNodeSize));
            return node;
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