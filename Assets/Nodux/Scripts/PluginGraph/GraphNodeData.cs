using System;
using Nodux.Core;
using UnityEngine;

namespace Nodux.PluginGraph
{
    [Serializable]
    public class GraphNodeData
    {
        public string Guid;
        public string Name;
        public Vector2 Position;
        public TypeSelection Data;
    }
}