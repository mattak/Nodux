using Nodux.Core;
using Nodux.PluginState;

namespace Nodux.PluginStopwatch
{
    [TypeSelectionEnable("Reducer")]
    public class StopwatchResetReducer : IReducer
    {
        public State Reduce(State state, StateAction action)
        {
            if (!(action.Reducer is StopwatchResetReducer)) return state;

            var stopwatch = state.Get(action.StateKey);
            var timer = stopwatch.Value<StopwatchValue>();

            timer.ElapsedTime = 0f;

            state.Set(action.StateKey, new Any(timer));
            return state;
        }
    }
}