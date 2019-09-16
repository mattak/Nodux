using System;
using Nodux.Core;
using Nodux.PluginNode;
using Nodux.PluginState;

namespace Nodux.PluginScene
{
    [Serializable]
    [TypeSelectionEnable("Node")]
    public class SceneSyncNode : Node
    {
        public StoreHolder StoreHolder;

        public SceneSyncNode(INode parent) : base(parent)
        {
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