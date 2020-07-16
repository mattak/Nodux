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

            var time = state.GetValue<StopwatchValue>(action.StateKey);

            time.IsPlaying = action.GetValue<bool>();
            state.Set(action.StateKey, new Any(time));
            return state;
        }
    }
}