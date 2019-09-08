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
        private IReducer Reducer;
        [SerializeField] private string StateKey;

        [SerializeField] [TypeSelectionFilter("Reducer")]
        private TypeSelection ReducerSelection;

        public StateActionNode(INode parent, string stateKey, IReducer reducer) : base(parent)
        {
            this.StateKey = stateKey;
            this.Reducer = reducer;
        }

        public override IDisposable Subscribe(IObserver<Any> observer)
        {
            if (this.ReducerSelection != null)
            {
                if (!this.ReducerSelection.Restore())
                {
                    UnityEngine.Debug.LogWarning("Cannot restore reducer selection");
                    return this.Parent.Subscribe(observer);
                }

                this.Reducer = this.ReducerSelection.Get<IReducer>();
                UnityEngine.Debug.Log($"Reducer: {Reducer}");
            }

            return this.Parent
                .Select(it => new Any(new StateAction(Reducer, StateKey, it)))
                .Subscribe(observer);
        }
    }
}