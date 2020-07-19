using Nodux.PluginScene.Nodes;
using Nodux.PluginState;
using Nodux.PluginUI;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace Nodux.PluginScene.Components
{
    [RequireComponent(typeof(Button))]
    public class SceneRemoveSelfButton : MonoBehaviour
    {
        [SerializeField] private StoreHolder StoreHolder = default;

        private void Start()
        {
            var scenes = new[] {this.gameObject.scene.name};

            var button = new OnClickButtonNode(this.GetComponent<Button>());
            var scene = new SceneRemoveNode(button, this.StoreHolder, scenes);
            scene
                .Subscribe(_ => { }, Debug.LogException)
                .AddTo(this);
        }
    }
}