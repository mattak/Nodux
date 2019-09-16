using Nodux.Core;

namespace Nodux.PluginState
{
    [TypeSelectionEnable("Reducer")]
    public class AssignmentReducer : IReducer
    {
        public State Reduce(State state, StateAction action)
        {
            if (!(action.Reducer is AssignmentReducer)) return state;

            state.Set(action.StateKey, action.Value);
            return state;
        }
    }
}