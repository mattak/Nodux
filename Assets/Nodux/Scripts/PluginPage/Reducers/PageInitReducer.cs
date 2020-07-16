using System;
using Nodux.Core;
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

            state.Set(definitionKey, new Any(definition));
            state.Set(stackKey, new Any(new PageStack()));

            return state;
        }
    }
}