using System;
using System.Collections.Generic;
using System.Linq;
using Nodux.Core;
using Nodux.PluginState;

namespace Nodux.PluginScene
{
    [Serializable]
    [TypeSelectionEnable("Reducer")]
    public class SceneSyncReducer : IReducer
    {
        public State Reduce(State state, StateAction action)
        {
            if (!(action.Reducer is SceneSyncReducer)) return state;
            if (!(action.Value.Is<IDictionary<string, bool>>()))
            {
                UnityEngine.Debug.LogWarning("StateAction value is not dictionary");
                return state;
            }

            // init
            var value = state.Get(action.StateKey).Value<IDictionary<string, bool>>();
            if (value == default(IDictionary<string, bool>)) value = new Dictionary<string, bool>();

            // update
            var map = action.Value.Value<IDictionary<string, bool>>();
            foreach (var scene in value.Keys.ToList()) value[scene] = false;
            foreach (var scene in map.Keys.ToList()) value[scene] = map[scene];
            state.Set(action.StateKey, new Any(value));

            return state;
        }
    }
}