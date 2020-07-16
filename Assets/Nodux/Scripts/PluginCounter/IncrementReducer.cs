using System;
using Nodux.Core;
using Nodux.PluginState;

namespace Nodux.PluginCounter
{
    [Serializable]
    [TypeSelectionEnable("Reducer")]
    public class IncrementReducer : IReducer
    {
        public State Reduce(State state, StateAction stateAction)
        {
            if (!(stateAction.Reducer is IncrementReducer)) return state;

            var value = state.GetValue<int>(stateAction.StateKey) + stateAction.GetValue<int>();
            state.Set(stateAction.StateKey, new Any(value));

            return state;
        }
    }
}