using System;
using Nodux.Core;
using Nodux.Nodux.Scripts.PluginScene;
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

        public SceneRendererNode(StoreHolder holder)
        {
            StoreHolder = holder;
        }

        public override IDisposable Subscribe(IObserver<Any> observer)
        {
            var stateNode = new StateReaderNode(this.StoreHolder, SceneConst.StateKey);
            var sceneNode = new SceneWriteNode(stateNode, this.StoreHolder);
            return sceneNode.Subscribe(observer);
        }
    }
}