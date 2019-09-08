using UnityLeaf.Core;

namespace UnityLeaf.PluginState
{
    public class IncrementReducer : IReducer
    {
        private readonly string ActionKey = typeof(IncrementReducer).FullName;

        public State Reduce(State state, StateAction stateAction)
        {
            if (stateAction.ActionKey != this.ActionKey) return state;

            var value = state.Get(stateAction.StateKey).Value<int>() + stateAction.Value.Value<int>();
            state.Set(stateAction.StateKey, new Any(value));

            return state;
        }
    }
}