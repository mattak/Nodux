using Nodux.PluginGraph;
using Nodux.PluginNode;
using UnityEditor;

namespace Nodux.PluginEditor
{
    public class SerializedNoduxLinkGraph
    {
        private SerializedObject _graph;
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
    }
}