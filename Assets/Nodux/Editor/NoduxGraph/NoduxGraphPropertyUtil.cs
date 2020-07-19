using Nodux.Core;

namespace Nodux.PluginEditor.NoduxGraph
{
    public static class NoduxGraphPropertyUtil
    {
        // public static void CreateProperties(NoduxGraphNode node, PluginNode.Node contentNode)
        // {
        //     UIElementPropertyRenderer.RenderClass(
        //         node.extensionContainer,
        //         contentNode.GetType(),
        //         contentNode,
        //         newValue => selection.SetValue(newValue)
        //     );
        // }

        public static void CreatePropertiesByTypeSelection(NoduxGraphNode node, TypeSelection selection)
        {
            UIElementPropertyRenderer.RenderClass(
                node.extensionContainer,
                selection.Type,
                selection.Object,
                newValue => selection.SetValue(newValue)
            );
        }
    }
}