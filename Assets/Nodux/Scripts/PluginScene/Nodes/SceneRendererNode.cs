using System;
using Nodux.Core;
using Nodux.PluginNode;
using Nodux.PluginState;
using UnityEngine;

namespace Nodux.PluginScene.Nodes
{
    [Serializable]
    [TypeSelectionEnable("Node")]
    public class SceneRendererNode : RootNode
    {
        [SerializeField] private StoreHolder StoreHolder = default;

        public override IDisposable Subscribe(IObserver<Any> observer)
        {
            var stateNode = new StateReaderNode(this.StoreHolder, "scene");
            var sceneNode = new SceneWriteNode(stateNode, this.StoreHolder);
            return sceneNode.Subscribe(observer);
        }
    }
}