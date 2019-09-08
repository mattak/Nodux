using System;
using UnityLeaf.Core;

namespace UnityLeaf.PluginState
{
    [Serializable]
    [TypeSelectionEnable("Reducer")]
    public class IncrementReducer : IReducer
    {
        public State Reduce(State state, StateAction stateAction)
        {
            if (!(stateAction.Reducer is IncrementReducer)) return state;

            var value = state.Get(stateAction.StateKey).Value<int>() + stateAction.Value.Value<int>();
            state.Set(stateAction.StateKey, new Any(value));

            return state;
        }
    }
}