using System;
using System.Collections.Generic;
using Nodux.Core;
using Nodux.Nodux.Scripts.PluginScene;
using Nodux.PluginState;

namespace Nodux.PluginPage.Reducers
{
    [Serializable]
    [TypeSelectionEnable("Reducer")]
    public class PageReplaceReducer : IReducer
    {
        public State Reduce(State state, StateAction action)
        {
            if (!(action.Reducer is PageReplaceReducer)) return state;
            if (!action.IsValueOf<Page>())
            {
                throw new ArgumentException($"StateAction.Value ({action.Value.Type}) is not type of Page");
            }

            var definition = state.GetValue<PageDefinition>(PageConst.StateDefinitionKey);
            var stack = state.GetValue<PageStack>(PageConst.StateStackKey);

            var nextPage = action.GetValue<Page>();
            var nextPageScene = definition.GetPageScene(nextPage?.Name);
            var previousPage = stack.Pop();
            var previousPageScene = definition.GetPageScene(previousPage?.Name);
            stack.Push(nextPage);

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