using Nodux.PluginScene.Nodes;
using Nodux.PluginState;
using Nodux.PluginUI;
using UniRx;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Nodux.PluginScene.Components
{
    [RequireComponent(typeof(Button))]
    public class SceneAddButton : MonoBehaviour
    {
        [SerializeField] private string[] scenes = default;
        [SerializeField] private StoreHolder storeHolder = default;

        private void Start()
        {
            if (scenes == null || scenes.Length < 1)
            {
                Debug.LogWarning("Not defined scenes. subscription aborted");
                return;
            }

            var button = new OnClickButtonNode(this.GetComponent<Button>());
            var scene = new SceneAddNode(button, this.storeHolder, this.scenes);
            scene
                .Subscribe(_ => { }, Debug.LogException)
                .AddTo(this);
        }
    }
}