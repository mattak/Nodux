using System;
using Nodux.Core;
using Nodux.PluginScene;
using Nodux.PluginNode;
using Nodux.PluginState;
using UnityEngine;

namespace Nodux.PluginScene.Nodes
{
    [Serializable]
    [TypeSelectionEnable("Node")]
    public class SceneRendererNode : Node
    {
        [SerializeField] private StoreHolder StoreHolder = default;

        public SceneRendererNode(StoreHolder holder) : base(null)
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