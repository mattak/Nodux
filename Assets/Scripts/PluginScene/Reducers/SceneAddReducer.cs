using System;
using System.Collections.Generic;
using Nodux.Core;
using Nodux.PluginState;

namespace Nodux.PluginScene.Reducers
{
    [Serializable]
    [TypeSelectionEnable("Reducer")]
    public class SceneAddReducer : IReducer
    {
        public State Reduce(State state, StateAction action)
        {
            if (!(action.Reducer is SceneAddReducer)) return state;

            var value = state.Get(action.StateKey).Value<IDictionary<string, bool>>();
            if (value == default(IDictionary<string, bool>)) value = new Dictionary<string, bool>();

            var sceneName = action.Value.Value<string>();
            value[sceneName] = true;

            state.Set(action.StateKey, new Any(value));

            return state;
        }
    }
}