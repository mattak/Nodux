using System.Collections.Generic;
using System.Linq;
using Nodux.PluginGraph;
using Nodux.PluginNode;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace Nodux.PluginEditor.NoduxGraph
{
    public class NoduxGraphSaveUtility
    {
        private NoduxGraphView _graphView;
        private IList<Edge> _edges => _graphView.edges.ToList();
        private IList<Edge> _connectedEdges => _edges.Where(x => x.input.node != null).ToArray();
        private IList<NoduxGraphNodeView> _nodes => _graphView.nodes.ToList().Cast<NoduxGraphNodeView>().ToList();

        public NoduxGraphSaveUtility(NoduxGraphView view)
        {
            this._graphView = view;
        }

        public void LoadGraph(NoduxLinkGraph component)
        {
            NoduxGraphUtil.ClearGraph(_graphView);
            _graphView.SetSerializeTarget(component);

            var graphNodeMap = new Dictionary<string, NoduxGraphNodeView>();

            // create nodes
            for (var i = 0; i < component.GraphContainer.Nodes.Count; i++)
            {
                var graphNode = NoduxGraphNodeCreator.Load(_graphView.SerializedGraph, component.GraphContainer.Nodes[i], i);
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

        public void SaveGraph(NoduxLinkGraph component)
        {
            // UnityEditor.Undo.RecordObject(component, "save node data");

            // 1. Containerを抽出
            // 2. ChainNodeを抽出
            var container = ExtractGraphContainer();
            var nodes = ExtractChainNodes();

            var serializedObject = new SerializedNoduxLinkGraph(component);
            serializedObject.SetContainer(container, nodes);
            // component.UpdateEditData(container, nodes);
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
                    Node = node.Data,
                });
            }

            foreach (var edge in _connectedEdges)
            {
                var outputNode = edge.output.node as NoduxGraphNodeView;
                var inputNode = edge.input.node as NoduxGraphNodeView;

                container.Links.Add(new GraphLinkData()
                {
                    SourceNodeGuid = outputNode.Guid,
                    TargetNodeGuid = inputNode.Guid,
                });
            }

            return container;
        }

        private INode[] ExtractChainNodes()
        {
            var nodesArray = NoduxGraphUtil.ExtractGraphNodeChains(_graphView);
            return nodesArray
                .Select(nodes =>
                    new LinkedNode(
                        null,
                        nodes.Select(it => it.Data)
                    )
                )
                .Cast<INode>()
                .ToArray();
        }
    }
}