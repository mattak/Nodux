using System.Collections.Generic;
using System.Linq;
using Nodux.PluginGraph;
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
            NoduxGraphOperation.ClearGraph(_graphView);
            _graphView.SetSerializeTarget(component);

            var graphNodeMap = new Dictionary<string, NoduxGraphNodeView>();

            // create nodes
            for (var i = 0; i < component.GraphContainer.Nodes.Count; i++)
            {
                var graphNode =
                    NoduxGraphNodeViewCreator.Load(_graphView.SerializedGraph, component.GraphContainer.Nodes[i], i);
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
                NoduxGraphOperation.LinkNode(_graphView, source, target);
            }
        }

        public void SaveGraph(NoduxLinkGraph component)
        {
            // node, link, LinkedNodeの更新
            _graphView.SerializedGraph.RemoveUnExistNodes(_nodes);
            _graphView.SerializedGraph.UpdateNodeMeta(_nodes);
            _graphView.SerializedGraph.UpdateLinks(_edges);
            var nodesArray = NoduxGraphOperation.ExtractGraphNodeChains(_graphView.SerializedGraph.Container);
            _graphView.SerializedGraph.UpdateLinkedNodes(nodesArray);
        }
    }
}