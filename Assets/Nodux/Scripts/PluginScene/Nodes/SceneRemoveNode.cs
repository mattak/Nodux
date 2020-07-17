using System;
using Nodux.Core;
using Nodux.PluginNode;
using Nodux.PluginScene.Reducers;
using Nodux.PluginState;

namespace Nodux.PluginScene.Nodes
{
    [Serializable]
    [TypeSelectionEnable("Node")]
    public class SceneRemoveNode : Node
    {
        public StoreHolder StoreHolder;
        public string[] RemoveSceneNames;

        public SceneRemoveNode(INode parent, StoreHolder holder, string[] removeSceneNames) : base(parent)
        {
            this.StoreHolder = holder;
            this.RemoveSceneNames = removeSceneNames;
        }

        public override IDisposable Subscribe(IObserver<Any> observer)
        {
            return this.CreateRemoveNode(this.Parent).Subscribe(observer);
        }

        private INode CreateRemoveNode(INode parent)
        {
            var removeSceneNode = new ValueNode(parent, RemoveSceneNames);
            var actionNode = new StateActionNode(removeSceneNode, "scene", new SceneRemoveReducer());
            var writerNode = new StateWriterNode(actionNode, this.StoreHolder);
            return writerNode;
        }
    }
}