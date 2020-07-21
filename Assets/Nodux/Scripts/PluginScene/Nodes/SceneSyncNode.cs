using System;
using Nodux.Core;
using Nodux.PluginNode;
using Nodux.PluginScene.Reducers;
using Nodux.PluginState;
using UnityEngine;

namespace Nodux.PluginScene.Nodes
{
    [Serializable]
    [TypeSelectionEnable("Node")]
    public class SceneSyncNode : Node
    {
        [SerializeField] private StoreHolder storeHolder;

        public SceneSyncNode(StoreHolder holder) : base(null)
        {
            this.storeHolder = holder;
        }

        public SceneSyncNode(INode parent, StoreHolder holder) : base(parent)
        {
            this.storeHolder = holder;
        }

        public override IDisposable Subscribe(IObserver<Any> observer)
        {
            var map = new CurrentSceneDictionaryNode(this.Parent);
            var actionNode = new StateActionNode(map, "scene", new SceneSyncReducer());
            var writerNode = new StateWriterNode(actionNode, this.storeHolder);
            return writerNode.Subscribe(observer);
        }
    }
}