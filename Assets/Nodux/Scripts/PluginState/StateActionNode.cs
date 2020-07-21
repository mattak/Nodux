using System;
using Nodux.Core;
using Nodux.PluginNode;
using UniRx;
using UnityEngine;

namespace Nodux.PluginState
{
    [Serializable]
    [TypeSelectionEnable("Node")]
    public class StateActionNode : Node
    {
        [SerializeField] private string stateKey;

        [SerializeField, SerializeReference, TypeSelectionFilter("Reducer")]
        private IReducer reducer;

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