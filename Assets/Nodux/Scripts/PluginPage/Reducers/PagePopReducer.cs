using System;
using System.Collections.Generic;
using Nodux.Core;
using Nodux.Nodux.Scripts.PluginScene;
using Nodux.PluginState;

namespace Nodux.PluginPage.Reducers
{
    [Serializable]
    [TypeSelectionEnable("Reducer")]
    public class PagePopReducer : IReducer
    {
        public State Reduce(State state, StateAction action)
        {
            if (!(action.Reducer is PagePopReducer)) return state;

            var definition = state.GetValue<PageDefinition>(PageConst.StateDefinitionKey);
            var stack = state.GetValue<PageStack>(PageConst.StateStackKey);

            // do nothing, if last page
            if (stack.Count <= 1) return state;

            var previousPage = stack.Pop();
            var previousPageScene = definition.GetPageScene(previousPage?.Name);
            var nextPage = stack.Peek();
            var nextPageScene = definition.GetPageScene(nextPage?.Name);

            if (previousPageScene != null) ChangeScenes(state, previousPageScene, false);
            if (nextPageScene != null) ChangeScenes(state, nextPageScene, true);

            state.NotifyValue(SceneConst.StateKey);
            state.NotifyValue(PageConst.StateStackKey);

            return state;
        }

        private State ChangeScenes(State state, PageScene pageScene, bool active)
        {
            var scenes = state.GetValue<IDictionary<string, bool>>(SceneConst.StateKey);
            foreach (var scene in pageScene.Scenes) scenes[scene] = active;
            return state;
        }
    }
}