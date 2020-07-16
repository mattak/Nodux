using System;
using System.Collections.Generic;
using Nodux.Core;
using Nodux.PluginState;

namespace Nodux.PluginScene.Reducers
{
    [Serializable]
    [TypeSelectionEnable("Reducer")]
    public class SceneRemoveReducer : IReducer
    {
        public State Reduce(State state, StateAction action)
        {
            if (!(action.Reducer is SceneRemoveReducer)) return state;

            var value = state.GetValue<IDictionary<string, bool>>(action.StateKey);
            if (value == default(IDictionary<string, bool>)) value = new Dictionary<string, bool>();

            var sceneNames = action.GetValue<IList<string>>();
            foreach (var sceneName in sceneNames) value[sceneName] = false;

            state.Set(action.StateKey, new Any(value));
            return state;
        }
    }
}