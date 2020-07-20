using System;
using UniRx;
using UnityEngine;
using Nodux.Core;
using Nodux.PluginNode;

namespace Nodux.PluginState
{
    [Serializable]
    [TypeSelectionEnable("Node")]
    public class StateActionNode : Node
    {
        [SerializeField] private string StateKey;

        [SerializeField, SerializeReference, TypeSelectionFilter("Reducer")]
        private IReducer Reducer;

        public StateActionNode(INode parent, string stateKey, IReducer reducer) : base(parent)
        {
            this.StateKey = stateKey;
            this.Reducer = reducer;
        }

        public override IDisposable Subscribe(IObserver<Any> observer)
        {
            return this.Parent
                .Select(it => new Any(new StateAction(Reducer, StateKey, it)))
                .Subscribe(observer);
        }
    }
}