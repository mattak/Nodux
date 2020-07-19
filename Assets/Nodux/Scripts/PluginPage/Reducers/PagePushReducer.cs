using System;
using Nodux.Core;
using Nodux.PluginScene;
using Nodux.PluginState;

namespace Nodux.PluginPage.Reducers
{
    [Serializable]
    [TypeSelectionEnable("Reducer")]
    public class PagePushReducer : IReducer
    {
        public State Reduce(State state, StateAction action)
        {
            if (!(action.Reducer is PagePushReducer)) return state;
            if (!action.IsValueOf<Page>())
            {
                throw new ArgumentException($"StateAction.Value ({action.Value.Type}) is not type of Page");
            }

            var definition = state.GetValue<PageDefinition>(PageConst.StateDefinitionKey);
            var stack = state.GetValue<PageStack>(PageConst.StateStackKey);

            var nextPage = action.GetValue<Page>();
            var nextPageScene = definition.GetPageScene(nextPage?.Name);
            var previousPage = stack.Peek();
            var previousPageScene = definition.GetPageScene(previousPage?.Name);
            stack.Push(nextPage);

            if (previousPageScene != null)
            {
                PageReducerUtility.ChangeScenes(state, previousPageScene.Scenes, false);
            }
            else
            {
                var scenes =
                    PageReducerUtility.GetCleanUpScenes(state, nextPageScene.Scenes, definition.PermanentScenes);
                PageReducerUtility.ChangeScenes(state, scenes, false);
            }

            if (nextPageScene != null)
            {
                PageReducerUtility.ChangeScenes(state, nextPageScene.Scenes, true);
            }

            state.NotifyValue(SceneConst.StateKey);
            state.NotifyValue(PageConst.StateStackKey);

            return state;
        }
    }
}