using System;
using Nodux.Core;
using Nodux.PluginNode;
using Nodux.PluginScene.Reducers;
using Nodux.PluginState;

namespace Nodux.PluginScene.Nodes
{
    [Serializable]
    [TypeSelectionEnable("Node")]
    public class SceneAddNode : Node
    {
        public StoreHolder StoreHolder;
        public string[] AddSceneNames;

        public SceneAddNode(INode parent, StoreHolder holder, string[] addSceneNames) : base(parent)
        {
            this.StoreHolder = holder;
            this.AddSceneNames = addSceneNames;
        }

        public override IDisposable Subscribe(IObserver<Any> observer)
        {
            return this.CreateAddNode(this.Parent).Subscribe(observer);
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