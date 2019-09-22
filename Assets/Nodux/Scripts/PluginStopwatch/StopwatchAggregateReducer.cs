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

            var timeValue = state.Get(action.StateKey);
            var time = timeValue.Value<StopwatchValue>();

            if (time.IsPlaying)
            {
                var value = time.ElapsedTime + action.Value.Value<float>();
                time.ElapsedTime = value;
                state.Set(action.StateKey, new Any(time));
            }

            return state;
        }
    }
}