using System;
using Nodux.Core;
using Nodux.PluginNode;
using Nodux.PluginScene.Reducers;
using Nodux.PluginState;
using UnityEngine;
using UnityEngine.Serialization;

namespace Nodux.PluginScene.Nodes
{
    [Serializable]
    [TypeSelectionEnable("Node")]
    public class SceneRemoveNode : Node
    {
        [SerializeField] private StoreHolder storeHolder;
        [SerializeField] private string[] removeSceneNames;

        public SceneRemoveNode(INode parent, StoreHolder holder, string[] removeSceneNames) : base(parent)
        {
            this.storeHolder = holder;
            this.removeSceneNames = removeSceneNames;
        }

        public override IDisposable Subscribe(IObserver<Any> observer)
        {
            return this.CreateRemoveNode(this.Parent).Subscribe(observer);
        }

        private INode CreateRemoveNode(INode parent)
        {
            var removeSceneNode = new ValueNode(parent, removeSceneNames);
            var actionNode = new StateActionNode(removeSceneNode, "scene", new SceneRemoveReducer());
            var writerNode = new StateWriterNode(actionNode, this.storeHolder);
            return writerNode;
        }
    }
}