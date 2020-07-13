using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace Nodux.PluginEditor.NoduxGraph
{
    public class NoduxGraphWindow : EditorWindow, NoduxGraphWindowDelegate
    {
        private NoduxGraphView _graphView;
        private Toolbar _toolbar;
        private Label _toolbarTitle;

        [MenuItem("Window/NoduxGraph")]
        public static void OpenDialog()
        {
            var window = GetWindow<NoduxGraphWindow>();
            window.titleContent = new GUIContent("Nodux Graph");
            window.Show();
        }

        public void OnEnable()
        {
            ConstructGraphView();
            GenerateMiniMap();
            GenerateToolbar();
        }

        public void OnDisable()
        {
            rootVisualElement.Remove(_graphView);
            rootVisualElement.Remove(_toolbar);
        }

        private void ConstructGraphView()
        {
            _graphView = new NoduxGraphView(this) {name = "NoduxGraph"};
            _graphView.StretchToParentSize();
            rootVisualElement.Add(_graphView);
        }

        private void GenerateMiniMap()
        {
            var miniMap = new MiniMap() {title = "map"};
            var coords = _graphView.contentViewContainer.WorldToLocal(new Vector2(maxSize.x - 10, 30));
            miniMap.title = "map";
            miniMap.anchored = false;
            miniMap.SetPosition(new Rect(coords.x, coords.y, 200, 140));
            _graphView.Add(miniMap);
        }

        private void GenerateToolbar()
        {
            _toolbar = new Toolbar();

            _toolbarTitle = new Label("Nothing")
            {
                style =
                {
                    color = Color.blue
                }
            };
            _toolbar.Add(new Button(() => RequestLoad()) {text = "Load"});
            _toolbar.Add(new Button(() => RequestSave()) {text = "Save"});
            _toolbar.Add(new Button(() => RequestClear()) {text = "Clear"});
            _toolbar.Add(new Button(() => _graphView.AlignNodes(1)) {text = "Align1"});
            _toolbar.Add(new Button(() => _graphView.AlignNodes(3)) {text = "Align3"});
            _toolbar.Add(_toolbarTitle);

            rootVisualElement.Add(_toolbar);
        }

        public void SetGraphName(string name)
        {
            _toolbarTitle.text = name;
        }

        private void RequestSave()
        {
            var graphs = FetchActiveNoduxGraphs();
            if (graphs == null || graphs.Length < 1) return;

            new NoduxGraphSaveUtility(_graphView).SaveGraph(graphs[0]);
        }

        private void RequestLoad()
        {
            var graphs = FetchActiveNoduxGraphs();
            if (graphs == null || graphs.Length < 1) return;

            new NoduxGraphSaveUtility(_graphView).LoadGraph(graphs[0]);
        }

        private void RequestClear()
        {
            NoduxGraphUtil.ClearGraph(_graphView);
        }

        private PluginGraph.NoduxGraph[] FetchActiveNoduxGraphs()
        {
            if (Selection.activeTransform == null)
            {
                EditorUtility.DisplayDialog("Error",
                    "Not found selection of active transform. Please select some GameObject and retry", "OK");
                return null;
            }

            // FIXME: set guid to support multiple graph in same game object
            var components = Selection.activeTransform.GetComponents<PluginGraph.NoduxGraph>();

            if (components.Length < 1)
            {
                EditorUtility.DisplayDialog("Error",
                    "Not found NoduxGraph in active GameObject. Please select some NoduxGraph GameObject", "OK");
                return null;
            }

            return components;
        }
    }
}