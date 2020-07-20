using System;
using System.Collections.Generic;
using System.Linq;
using Nodux.PluginGraph;
using Nodux.PluginNode;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

namespace Nodux.PluginEditor.NoduxGraph
{
    public static class NoduxGraphOperation
    {
        public static void ClearGraph(GraphView view)
        {
            var nodes = view.nodes.ToList().Cast<NoduxGraphNodeView>().ToList();
            var edges = view.edges.ToList();

            foreach (var node in nodes)
            {
                foreach (var edge in edges.Where(x => x.input.node == node).ToList())
                {
                    view.RemoveElement(edge);
                }

                view.RemoveElement(node);
            }
        }

        public static Rect LayoutNodes(int maxColumns, Vector2 origin, IList<NoduxGraphNodeView> nodes)
        {
            if (nodes.Count < 1) return new Rect(0, 0, 0, 0);

            var position = new Vector2(origin.x, origin.y);
            var drawAreaSize = new Vector2(0, 0);

            var rowHeight = 0f;

            for (var i = 0; i < nodes.Count; i++)
            {
                var height = nodes[i].resolvedStyle.height;
                var width = nodes[i].resolvedStyle.width;
                var size = new Vector2(width, height);

                // set position
                nodes[i].SetPosition(new Rect(position, size));

                // update draw area
                drawAreaSize = new Vector2(
                    Math.Max(drawAreaSize.x, position.x - origin.x + size.x),
                    Math.Max(drawAreaSize.y, position.y - origin.y + size.y)
                );
                rowHeight = Math.Max(rowHeight, size.y);

                // select next node
                var x = (i + 1) % maxColumns;

                if (x == 0)
                {
                    position = new Vector2(origin.x, position.y + rowHeight);
                    rowHeight = 0f;
                }
                else
                {
                    position = new Vector2(position.x + size.x, position.y);
                }
            }

            return new Rect(origin.x, origin.y, drawAreaSize.x, drawAreaSize.y);
        }

        public static void LinkNode(GraphView view, NoduxGraphNodeView source, NoduxGraphNodeView target)
        {
            var sourcePort = source.outputContainer.Q<Port>();
            var targetPort = target.inputContainer.Q<Port>();
            LinkPort(view, sourcePort, targetPort);
        }

        private static void LinkPort(GraphView view, Port source, Port target)
        {
            var edge = new Edge
            {
                output = source,
                input = target,
            };
            edge.input.Connect(edge);
            edge.output.Connect(edge);
            view.Add(edge);
        }

        public static INode[][] ExtractGraphNodeChains(GraphContainer container)
        {
            var links = container.Links
                .Where(x => x.TargetNodeGuid != null && x.SourceNodeGuid != null)
                .ToArray();
            var target2SourceDictionary = new Dictionary<string, string>();
            var source2TargetDictionary = new Dictionary<string, string>();
            var nodeDictionary = new Dictionary<string, INode>();

            foreach (var link in links)
            {
                // edge represents node1:output -> node2:input
                target2SourceDictionary[link.TargetNodeGuid] = link.SourceNodeGuid;
                source2TargetDictionary[link.SourceNodeGuid] = link.TargetNodeGuid;
            }

            foreach (var node in container.Nodes)
            {
                nodeDictionary[node.Guid] = node.Node;
            }

            var roots = new List<string>();
            foreach (var entry in target2SourceDictionary)
            {
                var input = entry.Value;
                if (!target2SourceDictionary.ContainsKey(input))
                {
                    roots.Add(entry.Value);
                }
            }

            var results = new List<INode[]>();
            for (var i = 0; i < roots.Count; i++)
            {
                var cursorGuid = roots[i];
                var result = new List<INode>();

                result.Add(nodeDictionary[cursorGuid]);
                for (var x = 0; x < nodeDictionary.Count; x++)
                {
                    if (!source2TargetDictionary.ContainsKey(cursorGuid)) break;
                    cursorGuid = source2TargetDictionary[cursorGuid];
                    result.Add(nodeDictionary[cursorGuid]);
                }

                results.Add(result.ToArray());
            }

            var singleNodes = ExtractSingleGraphNodeChains(container, nodeDictionary);
            foreach (var node in singleNodes)
            {
                results.Add(new INode[] {node});
            }

            return results.ToArray();
        }

        public static NoduxGraphNodeView[][] ExtractGraphNodeChains(GraphView view)
        {
            var connectedEdges = view.edges.ToList().Where(x => x.input.node != null).ToArray();
            var target2SourceDictionary = new Dictionary<string, string>();
            var source2TargetDictionary = new Dictionary<string, string>();
            var nodeDictionary = new Dictionary<string, NoduxGraphNodeView>();

            foreach (var edge in connectedEdges)
            {
                // edge represents node1:output -> node2:input
                var sourceNode = edge.output.node as NoduxGraphNodeView;
                var targetNode = edge.input.node as NoduxGraphNodeView;
                target2SourceDictionary[targetNode.Guid] = sourceNode.Guid;
                source2TargetDictionary[sourceNode.Guid] = targetNode.Guid;
                nodeDictionary[sourceNode.Guid] = sourceNode;
                nodeDictionary[targetNode.Guid] = targetNode;
            }

            var roots = new List<string>();
            foreach (var entry in target2SourceDictionary)
            {
                var input = entry.Value;
                if (!target2SourceDictionary.ContainsKey(input))
                {
                    roots.Add(entry.Value);
                }
            }

            var results = new List<NoduxGraphNodeView[]>();
            for (var i = 0; i < roots.Count; i++)
            {
                var cursorGuid = roots[i];
                var result = new List<NoduxGraphNodeView>();

                result.Add(nodeDictionary[cursorGuid]);
                for (var x = 0; x < nodeDictionary.Count; x++)
                {
                    if (!source2TargetDictionary.ContainsKey(cursorGuid)) break;
                    cursorGuid = source2TargetDictionary[cursorGuid];
                    result.Add(nodeDictionary[cursorGuid]);
                }

                results.Add(result.ToArray());
            }

            var singleNodes = ExtractSingleGraphNodeChains(view, nodeDictionary);
            foreach (var node in singleNodes)
            {
                results.Add(new NoduxGraphNodeView[] {node});
            }

            return results.ToArray();
        }

        private static IList<INode> ExtractSingleGraphNodeChains(
            GraphContainer container,
            IDictionary<string, INode> linkedNodeMap)
        {
            var unlinkedNodes = container.Nodes
                .Where(x => !linkedNodeMap.ContainsKey(x.Guid))
                .Select(x => x.Node)
                .ToList();

            return unlinkedNodes;
        }

        private static IList<NoduxGraphNodeView> ExtractSingleGraphNodeChains(
            GraphView view,
            IDictionary<string, NoduxGraphNodeView> linkedNodeMap)
        {
            var unlinkedNodes = view.nodes.ToList().Cast<NoduxGraphNodeView>()
                .Where(x => !linkedNodeMap.ContainsKey(x.Guid))
                .ToList();

            return unlinkedNodes;
        }
    }
}