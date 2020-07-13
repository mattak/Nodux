using System.Collections.Generic;
using System.Linq;
using Nodux.PluginGraph;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace Nodux.PluginEditor.NoduxGraph
{
    public class NoduxGraphSaveUtility
    {
        private NoduxGraphView _graphView;
        private IList<Edge> _edges => _graphView.edges.ToList();
        private IList<Edge> _connectedEdges => _edges.Where(x => x.input.node != null).ToArray();
        private IList<NoduxGraphNode> _nodes => _graphView.nodes.ToList().Cast<NoduxGraphNode>().ToList();

        public NoduxGraphSaveUtility(NoduxGraphView view)
        {
            this._graphView = view;
        }

        public void LoadGraph(PluginGraph.NoduxGraph component)
        {
            NoduxGraphUtil.ClearGraph(_graphView);

            var graphNodeMap = new Dictionary<string, NoduxGraphNode>();

            // create nodes
            foreach (var node in component.GraphContainer.Nodes)
            {
                node.Data.Restore();
                var graphNode = NoduxGraphNodeCreator.Create(node);
                _graphView.AddElement(graphNode);
                graphNodeMap[graphNode.Guid] = graphNode;
            }

            // connect nodes
            foreach (var link in component.GraphContainer.Links)
            {
                if (link.TargetNodeGuid == null || link.SourceNodeGuid == null) continue;
                if (!graphNodeMap.ContainsKey(link.TargetNodeGuid) || !graphNodeMap.ContainsKey(link.SourceNodeGuid))
                {
                    Debug.LogWarning($"Link nodes failed {link.SourceNodeGuid} -> {link.TargetNodeGuid}");
                    continue;
                }

                var source = graphNodeMap[link.SourceNodeGuid];
                var target = graphNodeMap[link.TargetNodeGuid];
                NoduxGraphUtil.LinkNode(_graphView, source, target);
            }
        }

        public void SaveGraph(PluginGraph.NoduxGraph component)
        {
            Undo.RecordObject(component, "save node data");

            // 1. Containerを抽出
            // 2. ChainNodeを抽出
            var container = ExtractGraphContainer();
            var chainNodes = ExtractChainNodes();
            component.UpdateEditData(container, chainNodes);
        }

        private GraphContainer ExtractGraphContainer()
        {
            var container = new GraphContainer();

            foreach (var node in _nodes)
            {
                container.Nodes.Add(new GraphNodeData()
                {
                    Guid = node.Guid,
                    Name = node.name,
                    Position = node.GetPosition().position,
                    Data = node.Data,
                });
            }

            foreach (var edge in _connectedEdges)
            {
                var outputNode = edge.output.node as NoduxGraphNode;
                var inputNode = edge.input.node as NoduxGraphNode;

                container.Links.Add(new GraphLinkData()
                {
                    SourceNodeGuid = outputNode.Guid,
                    TargetNodeGuid = inputNode.Guid,
                });
            }

            return container;
        }

        private ChainNode[] ExtractChainNodes()
        {
            var nodesArray = NoduxGraphUtil.ExtractGraphNodeChains(_graphView);
            return nodesArray
                .Select(nodes =>
                    new ChainNode(
                        null,
                        nodes.Select(it => it.Data)
                    )
                )
                .ToArray();
        }
    }
}