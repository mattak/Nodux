using System;

namespace Nodux.PluginGraph
{
    [Serializable]
    public class GraphLinkData
    {
        public string SourceNodeGuid;
        public string TargetNodeGuid;
    }
}