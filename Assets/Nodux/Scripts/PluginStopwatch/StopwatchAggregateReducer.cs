using System;
using Nodux.Core;
using Nodux.PluginState;

namespace Nodux.PluginStopwatch
{
    [Serializable]
    [TypeSelectionEnable("Reducer")]
    public class StopwatchAggregateReducer : IReducer
    {
        public State Reduce(State state, StateAction action)
        {
            if (!(action.Reducer is StopwatchAggregateReducer)) return state;

            var time = state.GetValue<StopwatchValue>(action.StateKey);

            if (time.isPlaying)
            {
                var value = time.elapsedTime + action.GetValue<float>();
                time.elapsedTime = value;
                state.Set(action.StateKey, new Any(time));
            }

            return state;
        }
    }
}