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
    public class SceneAddNode : Node
    {
        [SerializeField] private StoreHolder storeHolder;
        [SerializeField] private string[] addSceneNames;

        public SceneAddNode(INode parent, StoreHolder holder, string[] addSceneNames) : base(parent)
        {
            this.storeHolder = holder;
            this.addSceneNames = addSceneNames;
        }

        public override IDisposable Subscribe(IObserver<Any> observer)
        {
            return this.CreateAddNode(this.Parent).Subscribe(observer);
        }

        private INode CreateAddNode(INode parent)
        {
            var addSceneNode = new ValueNode(parent, this.addSceneNames);
            var actionNode = new StateActionNode(addSceneNode, "scene", new SceneAddReducer());
            var writerNode = new StateWriterNode(actionNode, this.storeHolder);
            return writerNode;
        }
    }
}