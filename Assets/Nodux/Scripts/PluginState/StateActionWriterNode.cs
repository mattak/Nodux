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
        [SerializeField] private string StateKey;

        [SerializeField, SerializeReference, TypeSelectionFilter("Reducer")]
        private IReducer Reducer;

        [SerializeField] private StoreHolder StoreHolder;

        public StateActionWriterNode(
            INode parent,
            string stateKey,
            StoreHolder storeHolder,
            IReducer reducer
        ) : base(parent)
        {
            this.StateKey = stateKey;
            this.StoreHolder = storeHolder;
            this.Reducer = reducer;
        }

        public override IDisposable Subscribe(IObserver<Any> observer)
        {
            var actionNode = new StateActionNode(this.Parent, this.StateKey, this.Reducer);
            var writerNode = new StateWriterNode(actionNode, this.StoreHolder);
            return writerNode.Subscribe(observer);
        }
    }
}