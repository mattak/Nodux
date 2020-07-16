using System;
using Nodux.Core;
using Nodux.PluginNode;
using Nodux.PluginScene.Reducers;
using Nodux.PluginState;

namespace Nodux.PluginScene.Nodes
{
    [Serializable]
    [TypeSelectionEnable("Node")]
    public class SceneSyncNode : Node
    {
        public StoreHolder StoreHolder;

        public SceneSyncNode(StoreHolder holder) : base(null)
        {
            this.StoreHolder = holder;
        }

        public SceneSyncNode(INode parent, StoreHolder holder) : base(parent)
        {
            this.StoreHolder = holder;
        }

        public override IDisposable Subscribe(IObserver<Any> observer)
        {
            var map = new CurrentSceneDictionaryNode(this.Parent);
            var actionNode = new StateActionNode(map, "scene", new SceneSyncReducer());
            var writerNode = new StateWriterNode(actionNode, this.StoreHolder);
            return writerNode.Subscribe(observer);
        }
    }
}