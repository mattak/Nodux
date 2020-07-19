using Nodux.PluginNode;
using UnityEngine;
using Node = UnityEditor.Experimental.GraphView.Node;

namespace Nodux.PluginEditor.NoduxGraph
{
    public class NoduxGraphNode : Node
    {
        public string Guid;
        [SerializeReference] public INode Data;
    }
}