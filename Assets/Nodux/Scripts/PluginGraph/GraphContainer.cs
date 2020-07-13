using System;
using System.Collections.Generic;

namespace Nodux.PluginGraph
{
    [Serializable]
    public class GraphContainer
    {
        public List<GraphNodeData> Nodes = new List<GraphNodeData>();
        public List<GraphLinkData> Links = new List<GraphLinkData>();
    }
}