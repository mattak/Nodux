using System;
using Nodux.PluginNode;
using UnityEngine;

namespace Nodux.PluginGraph
{
    [Serializable]
    public class GraphNodeData
    {
        public string Guid;
        public string Name;
        public Vector2 Position;
        [SerializeReference] public INode Node;
    }
}