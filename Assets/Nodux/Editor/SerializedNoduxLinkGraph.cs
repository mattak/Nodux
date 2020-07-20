using System.Collections.Generic;
using System.Linq;
using Nodux.PluginEditor.NoduxGraph;
using Nodux.PluginGraph;
using Nodux.PluginNode;
using UnityEditor;
using UnityEditor.Experimental.GraphView;

namespace Nodux.PluginEditor
{
    public class SerializedNoduxLinkGraph
    {
        private SerializedObject _graph;

        public GraphContainer Container => this._graph.FindProperty("_graphContainer").GetValue<GraphContainer>();
        public string GraphSceneName { get; }

        public SerializedNoduxLinkGraph(NoduxLinkGraph graph)
        {
            this._graph = new SerializedObject(graph);
            this._graph.Update();
            this.GraphSceneName = graph.gameObject.scene.name;
        }

        public void SetContainer(GraphContainer containerData, INode[] nodesData)
        {
            this._graph.Update();

            var container = this._graph.FindProperty("_graphContainer");
            container.SetValue(containerData);

            var nodes = this._graph.FindProperty("_nodes");
            nodes.SetValue(nodesData);

            this._graph.ApplyModifiedProperties();
        }

        public int AddNode(GraphNodeData data)
        {
            this._graph.Update();

            var container = this._graph.FindProperty("_graphContainer");
            var nodes = container.FindPropertyRelative("Nodes");

            var index = nodes.arraySize;
            nodes.InsertArrayElementAtIndex(index);
            this._graph.ApplyModifiedProperties();

            nodes.SetValueToIListIndex(data, index);
            this._graph.ApplyModifiedProperties();
            this._graph.Update();

            return index;
        }

        public SerializedProperty GetNode(int index)
        {
            var container = this._graph.FindProperty("_graphContainer");
            var nodes = container.FindPropertyRelative("Nodes");
            return nodes.GetArrayElementAtIndex(index).FindPropertyRelative("Node");
        }

        public void RemoveUnExistNodes(IEnumerable<NoduxGraphNodeView> views)
        {
            this._graph.Update();
            var guidMap = new Dictionary<string, bool>();
            foreach (var view in views) guidMap[view.Guid] = true;

            var filteredNodes = Container.Nodes
                .Where(x => guidMap.ContainsKey(x.Guid))
                .ToList();

            this.Container.Nodes = filteredNodes;
            this._graph.ApplyModifiedProperties();
        }

        public bool UpdateNodeMeta(IEnumerable<NoduxGraphNodeView> views)
        {
            this._graph.Update();

            var dictionary = new Dictionary<string, NoduxGraphNodeView>();
            foreach (var view in views) dictionary[view.Guid] = view;

            var isUpdated = false;
            var nodes = Container.Nodes;
            for (var i = 0; i < nodes.Count; i++)
            {
                var node = nodes[i];
                if (dictionary.ContainsKey(nodes[i].Guid))
                {
                    var view = dictionary[nodes[i].Guid];
                    node.Name = view.name;
                    node.Position = view.GetPosition().position;
                    isUpdated = true;
                }
            }

            if (isUpdated)
            {
                this._graph.ApplyModifiedProperties();
                this._graph.Update();
            }

            return isUpdated;
        }

        public void RemoveNode(int index)
        {
            var container = this._graph.FindProperty("_graphContainer");
            var nodes = container.FindPropertyRelative("Nodes");
            nodes.DeleteArrayElementAtIndex(index);
        }

        public SerializedProperty AddLink(GraphLinkData data)
        {
            this._graph.Update();

            var container = this._graph.FindProperty("_graphContainer");
            var links = container.FindPropertyRelative("Links");

            var index = links.arraySize;
            links.InsertArrayElementAtIndex(index);
            var element = links.GetArrayElementAtIndex(index);
            element.SetValue(data);

            this._graph.ApplyModifiedProperties();
            return element;
        }

        public SerializedProperty GetLink(int index)
        {
            var container = this._graph.FindProperty("_graphContainer");
            var links = container.FindPropertyRelative("Links");
            return links.GetArrayElementAtIndex(index);
        }

        public void RemoveLink(int index)
        {
            var container = this._graph.FindProperty("_graphContainer");
            var links = container.FindPropertyRelative("Links");
            links.DeleteArrayElementAtIndex(index);
        }

        public void UpdateLinks(IEnumerable<Edge> edges)
        {
            this._graph.Update();

            var links = Container.Links;
            links.Clear();

            foreach (var edge in edges)
            {
                var inputNode = edge.input.node as NoduxGraphNodeView;
                var outputNode = edge.output.node as NoduxGraphNodeView;

                links.Add(new GraphLinkData()
                {
                    SourceNodeGuid = outputNode.Guid,
                    TargetNodeGuid = inputNode.Guid,
                });
            }

            this._graph.ApplyModifiedProperties();
        }

        public void UpdateLinkedNodes(INode[][] nodes)
        {
            var linkedNodes = nodes.Select(x => new LinkedNode(null, x)).ToArray();
            this._graph.Update();

            var property = this._graph.FindProperty("_nodes");
            property.arraySize = linkedNodes.Length;
            this._graph.ApplyModifiedProperties();

            for (var i = 0; i < linkedNodes.Length; i++)
            {
                property.SetValueToIListIndex(linkedNodes[i], i);
            }

            this._graph.ApplyModifiedProperties();
        }
    }
}