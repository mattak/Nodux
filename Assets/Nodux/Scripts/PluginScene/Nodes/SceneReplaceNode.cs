using System;
using Nodux.Core;
using Nodux.PluginNode;
using Nodux.PluginScene.Reducers;
using Nodux.PluginState;

namespace Nodux.PluginScene.Nodes
{
    [Serializable]
    [TypeSelectionEnable("Node")]
    public class SceneReplaceNode : Node
    {
        public StoreHolder StoreHolder;
        public string[] AddSceneNames;

        public SceneReplaceNode(INode parent, StoreHolder holder, string[] addSceneNames) : base(parent)
        {
            this.StoreHolder = holder;
            this.AddSceneNames = addSceneNames;
        }

        public override IDisposable Subscribe(IObserver<Any> observer)
        {
            var removeNode = this.CreateRemoveNode(this.Parent);
            var addNode = this.CreateAddNode(removeNode);
            return addNode.Subscribe(observer);
        }

        private INode CreateRemoveNode(INode parent)
        {
            var removeSceneNode = new LoadListSceneNode(parent);
            var actionNode = new StateActionNode(removeSceneNode, "scene", new SceneRemoveReducer());
            var writerNode = new StateWriterNode(actionNode, this.StoreHolder);
            return writerNode;
        }

        private INode CreateAddNode(INode parent)
        {
            var addSceneNode = new ValueNode(parent, this.AddSceneNames);
            var actionNode = new StateActionNode(addSceneNode, "scene", new SceneAddReducer());
            var writerNode = new StateWriterNode(actionNode, this.StoreHolder);
            return writerNode;
        }
    }
}