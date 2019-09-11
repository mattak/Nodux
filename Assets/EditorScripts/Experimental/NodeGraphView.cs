using UnityEditor;
using UnityEditor.Experimental.GraphView;

namespace UnityLeaf.PluginEditor.Experimental
{
    public class NodeGraphView : GraphView
    {
        [MenuItem("Window/Leaf/NodeGraphView %&g")]
        public static void Init()
        {
            var view = new NodeGraphView();
            view.name = "Node Graph";
//            var window = GetWindow<StatePanel>();
//            window.titleContent.text = "State Panel";
//            window.Show();
        }
    }
}