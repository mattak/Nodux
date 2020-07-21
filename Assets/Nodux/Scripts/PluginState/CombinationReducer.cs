using System;
using Nodux.Core;
using UnityEngine;

namespace Nodux.PluginState
{
    [Serializable]
    [TypeSelectionEnable("Reducer")]
    public class CombinationReducer : IReducer
    {
        [SerializeReference, SerializeField, TypeSelectionFilter("Reducer")]
        private IReducer[] reducers;

        public CombinationReducer(params IReducer[] reducers)
        {
            this.reducers = reducers;
        }

        public State Reduce(State state, StateAction action)
        {
            foreach (var reducer in reducers)
            {
                state = reducer.Reduce(state, action);
            }

            return state;
        }
    }
}