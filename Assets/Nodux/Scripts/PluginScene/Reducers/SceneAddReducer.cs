using System;
using System.Collections.Generic;
using Nodux.Core;
using Nodux.PluginScene;
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

            var value = state.GetValue<IDictionary<string, bool>>(SceneConst.StateKey);
            if (value == default(IDictionary<string, bool>)) value = new Dictionary<string, bool>();

            var sceneNames = action.GetValue<IList<string>>();
            foreach (var sceneName in sceneNames) value[sceneName] = true;

            state.Set(action.StateKey, new Any(value));

            return state;
        }
    }
}