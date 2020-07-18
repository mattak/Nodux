using Nodux.Core;

namespace Nodux.PluginEditor.NoduxGraph
{
    public static class NoduxGraphPropertyUtil
    {
        public static void CreateProperties(NoduxGraphNode node, TypeSelection selection)
        {
            UIElementPropertyRenderer.RenderTypeSelection(node.extensionContainer, selection);
        }
    }
}