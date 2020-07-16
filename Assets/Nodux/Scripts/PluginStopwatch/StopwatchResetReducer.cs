using System;
using Nodux.Core;
using Nodux.PluginState;

namespace Nodux.PluginStopwatch
{
    [Serializable]
    [TypeSelectionEnable("Reducer")]
    public class StopwatchResetReducer : IReducer
    {
        public State Reduce(State state, StateAction action)
        {
            if (!(action.Reducer is StopwatchResetReducer)) return state;

            var timer = state.GetValue<StopwatchValue>(action.StateKey);

            timer.ElapsedTime = 0f;

            state.Set(action.StateKey, new Any(timer));
            return state;
        }
    }
}