using System;
using UniRx;
using UnityLeaf.Core;
using UnityLeaf.PluginState;

namespace UnityLeaf.PluginNode
{
    public class StateActionNode : Node
    {
        private IReducer reducer;
        private string stateKey;

        public StateActionNode(INode parent, string stateKey, IReducer reducer) : base(parent)
        {
            this.stateKey = stateKey;
            this.reducer = reducer;
        }

        public override IObservable<Any> GetObservable()
        {
            return this.GetParent()
                .GetObservable()
                .Select(it => new Any(new StateAction(reducer, stateKey, it)));
        }
    }
}