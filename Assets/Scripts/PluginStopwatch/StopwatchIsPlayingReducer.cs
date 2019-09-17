using System;
using Nodux.Core;
using Nodux.PluginState;

namespace Nodux.PluginStopwatch
{
    [Serializable]
    [TypeSelectionEnable("Reducer")]
    public class StopwatchIsPlayingReducer : IReducer
    {
        public State Reduce(State state, StateAction action)
        {
            if (!(action.Reducer is StopwatchIsPlayingReducer)) return state;

            var timeValue = state.Get(action.StateKey);
            var time = timeValue.Value<StopwatchValue>();

            time.IsPlaying = action.Value.Value<bool>();
            state.Set(action.StateKey, new Any(time));
            return state;
        }
    }
}