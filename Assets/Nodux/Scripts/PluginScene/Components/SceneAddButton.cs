using Nodux.PluginScene.Nodes;
using Nodux.PluginState;
using Nodux.PluginUI;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace Nodux.PluginScene.Components
{
    [RequireComponent(typeof(Button))]
    public class SceneAddButton : MonoBehaviour
    {
        [SerializeField] private string[] Scenes = default;
        [SerializeField] private StoreHolder StoreHolder = default;

        private void Start()
        {
            if (Scenes == null || Scenes.Length < 1)
            {
                Debug.LogWarning("Not defined scenes. subscription aborted");
                return;
            }

            var button = new OnClickButtonNode(this.GetComponent<Button>());
            var scene = new SceneAddNode(button, this.StoreHolder, this.Scenes);
            scene
                .Subscribe(_ => { }, Debug.LogException)
                .AddTo(this);
        }
    }
}