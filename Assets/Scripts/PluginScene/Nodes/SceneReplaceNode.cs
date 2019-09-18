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
        public string AddSceneName;

        public SceneReplaceNode(INode parent) : base(parent)
        {
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
            var addSceneNode = new ConstStringNode(parent, this.AddSceneName);
            var actionNOde = new StateActionNode(addSceneNode, "scene", new SceneAddReducer());
            var writerNode = new StateWriterNode(actionNOde, this.StoreHolder);
            return writerNode;
        }
    }
}