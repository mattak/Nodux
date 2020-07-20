using System;
using Nodux.Core;
using UnityEngine;

namespace Nodux.PluginState
{
    [Serializable]
    [TypeSelectionEnable("Reducer")]
    public class CombinationReducer : IReducer
    {
        [SerializeReference, TypeSelectionFilter("Reducer")]
        public IReducer[] Reducers;

        public State Reduce(State state, StateAction action)
        {
            foreach (var reducer in Reducers)
            {
                state = reducer.Reduce(state, action);
            }

            return state;
        }
    }
}