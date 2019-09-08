using System;
using UniRx;
using UnityEngine;
using UnityLeaf.Core;
using UnityLeaf.PluginState;

namespace UnityLeaf.PluginNode
{
    [Serializable]
    [TypeSelectionEnable("Node")]
    public class StateActionNode : Node
    {
        [SerializeField] private IReducer reducer;
        [SerializeField] private string stateKey;

        public StateActionNode(INode parent, string stateKey, IReducer reducer) : base(parent)
        {
            this.stateKey = stateKey;
            this.reducer = reducer;
        }

        public override IDisposable Subscribe(IObserver<Any> observer)
        {
            return this.Parent
                .Select(it => new Any(new StateAction(reducer, stateKey, it)))
                .Subscribe(observer);
        }
    }
}