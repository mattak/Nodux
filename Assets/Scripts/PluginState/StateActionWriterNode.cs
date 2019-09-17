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

        public StateActionWriterNode(
            INode parent,
            string stateKey,
            StoreHolder storeHolder,
            IReducer reducer,
            TypeSelection reducerSelection
        ) : base(parent)
        {
            this.StateKey = stateKey;
            this.StoreHolder = storeHolder;
            this.Reducer = reducer;
            this.ReducerSelection = reducerSelection;
        }

        public override IDisposable Subscribe(IObserver<Any> observer)
        {
            var actionNode = new StateActionNode(this.Parent, this.StateKey, this.Reducer, this.ReducerSelection);
            var writerNode = new StateWriterNode(actionNode, this.StoreHolder);
            return writerNode.Subscribe(observer);
        }
    }
}