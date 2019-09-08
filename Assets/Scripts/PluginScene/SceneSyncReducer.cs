using System;
using System.Collections.Generic;
using System.Linq;
using UnityLeaf.Core;
using UnityLeaf.PluginState;

namespace UnityLeaf.PluginScene
{
    [Serializable]
    [TypeSelectionEnable("Reducer")]
    public class SceneSyncReducer : IReducer
    {
        public State Reduce(State state, StateAction action)
        {
            if (!(action.Reducer is SceneSyncReducer)) return state;

            // init
            var value = state.Get(action.StateKey).Value<IDictionary<string, bool>>();
            if (value == default(IDictionary<string, bool>)) value = new Dictionary<string, bool>();

            // update
            var list = action.Value.Value<IList<string>>();
            foreach (var scene in value.Keys.ToList()) value[scene] = false;
            foreach (var scene in list) value[scene] = true;

            return state;
        }
    }
}