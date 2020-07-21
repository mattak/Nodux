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
        [SerializeField] private string stateKey;

        [SerializeField, SerializeReference, TypeSelectionFilter("Reducer")]
        private IReducer reducer;

        [SerializeField] private StoreHolder storeHolder;

        public StateActionWriterNode(
            INode parent,
            string stateKey,
            StoreHolder storeHolder,
            IReducer reducer
        ) : base(parent)
        {
            this.stateKey = stateKey;
            this.storeHolder = storeHolder;
            this.reducer = reducer;
        }

        public override IDisposable Subscribe(IObserver<Any> observer)
        {
            var actionNode = new StateActionNode(this.Parent, this.stateKey, this.reducer);
            var writerNode = new StateWriterNode(actionNode, this.storeHolder);
            return writerNode.Subscribe(observer);
        }
    }
}