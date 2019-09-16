using System;
using Nodux.Core;
using Nodux.PluginNode;
using UnityEngine;

namespace Nodux.PluginState
{
    [Serializable]
    [TypeSelectionEnable("Node")]
    public class StateActionWriterNode : Node
    {
        private IReducer Reducer;
        [SerializeField] private string StateKey;

        [SerializeField] [TypeSelectionFilter("Reducer")]
        private TypeSelection ReducerSelection;

        [SerializeField] private StoreHolder StoreHolder;

        public StateActionWriterNode(INode parent) : base(parent)
        {
        }

        public override IDisposable Subscribe(IObserver<Any> observer)
        {
            var actionNode = new StateActionNode(this.Parent, this.StateKey, this.Reducer, this.ReducerSelection);
            var writerNode = new StateWriterNode(actionNode, this.StoreHolder);
            return writerNode.Subscribe(observer);
        }
    }
}