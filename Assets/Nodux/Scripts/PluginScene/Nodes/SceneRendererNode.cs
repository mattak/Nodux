using System;
using Nodux.Core;
using Nodux.PluginScene;
using Nodux.PluginNode;
using Nodux.PluginState;
using UnityEngine;
using UnityEngine.Serialization;

namespace Nodux.PluginScene.Nodes
{
    [Serializable]
    [TypeSelectionEnable("Node")]
    public class SceneRendererNode : Node
    {
        [SerializeField] private StoreHolder storeHolder = default;

        public SceneRendererNode(StoreHolder holder) : base(null)
        {
            storeHolder = holder;
        }

        public override IDisposable Subscribe(IObserver<Any> observer)
        {
            var stateNode = new StateReaderNode(this.storeHolder, SceneConst.StateKey);
            var sceneNode = new SceneWriteNode(stateNode, this.storeHolder);
            return sceneNode.Subscribe(observer);
        }
    }
}