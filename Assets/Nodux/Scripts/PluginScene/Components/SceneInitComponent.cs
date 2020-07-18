using Nodux.PluginScene.Nodes;
using Nodux.PluginState;
using UniRx;
using UnityEngine;

namespace Nodux.Nodux.Scripts.PluginScene.Components
{
    public class SceneInitComponent : MonoBehaviour
    {
        public StoreHolder StoreHolder;

        private void Start()
        {
            new SceneSyncNode(StoreHolder)
                .Subscribe(_ => { }, Debug.LogException)
                .AddTo(this);

            new SceneRendererNode(this.StoreHolder)
                .Subscribe(_ => { }, Debug.LogException)
                .AddTo(this);
        }
    }
}