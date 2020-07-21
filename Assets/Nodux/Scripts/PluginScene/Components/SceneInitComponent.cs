using Nodux.PluginScene.Nodes;
using Nodux.PluginState;
using UniRx;
using UnityEngine;
using UnityEngine.Serialization;

namespace Nodux.PluginScene.Components
{
    public class SceneInitComponent : MonoBehaviour
    {
        [SerializeField] private StoreHolder storeHolder = default;

        private void Start()
        {
            new SceneSyncNode(storeHolder)
                .Subscribe(_ => { }, Debug.LogException)
                .AddTo(this);

            new SceneRendererNode(this.storeHolder)
                .Subscribe(_ => { }, Debug.LogException)
                .AddTo(this);
        }
    }
}