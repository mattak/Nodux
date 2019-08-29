using System.Collections.Generic;
using UnityLeaf.Core;
using UnityLeaf.PluginState;

namespace UnityLeaf.PluginScene
{
    public class SceneAddReducer : IReducer
    {
        private readonly string ActionKey = typeof(SceneAddReducer).FullName;

        public State Reduce(State state, StateAction action)
        {
            if (action.ActionKey != ActionKey) return state;

            var value = state.Get(action.StateKey).Value<IDictionary<string, bool>>();
            if (value == default(IDictionary<string, bool>)) value = new Dictionary<string, bool>();

            var sceneName = action.Value.Value<string>();
            value[sceneName] = true;

            state.Set(action.StateKey, new Any(value));

            return state;
        }
    }
}