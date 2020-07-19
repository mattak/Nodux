using System;
using Nodux.Core;
using Nodux.PluginScene;
using Nodux.PluginState;

namespace Nodux.PluginPage.Reducers
{
    [Serializable]
    [TypeSelectionEnable("Reducer")]
    public class PageInitReducer : IReducer
    {
        public State Reduce(State state, StateAction action)
        {
            if (!(action.Reducer is PageInitReducer)) return state;

            var definitionKey = PageConst.StateDefinitionKey;
            var stackKey = PageConst.StateStackKey;

            var definition = action.GetValue<PageDefinition>();

            if (state.Get(stackKey).IsNull() || state.Get(definitionKey).IsNull())
            {
                state.Set(definitionKey, new Any(definition));
                state.Set(stackKey, new Any(new PageStack()));

                // load permanent scenes
                PageReducerUtility.ChangeScenes(state, definition.PermanentScenes, true);
                state.NotifyValue(SceneConst.StateKey);
            }

            return state;
        }
    }
}