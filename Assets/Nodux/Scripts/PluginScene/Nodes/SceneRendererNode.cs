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
        public StoreHolder storeHolder;
        public MonoBehaviour behaviour;

        public override IDisposable Subscribe(IObserver<Any> observer)
        {
            var stateNode = new StateReaderNode(this.storeHolder, "scene");
            var sceneNode = new SceneWriteNode(stateNode, this.behaviour);
            return sceneNode.Subscribe(observer);
        }
    }
}