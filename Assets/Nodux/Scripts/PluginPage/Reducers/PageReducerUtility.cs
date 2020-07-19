using System.Collections.Generic;
using System.Linq;
using Nodux.PluginScene;
using Nodux.PluginState;

namespace Nodux.PluginPage.Reducers
{
    public static class PageReducerUtility
    {
        public static State ChangeScenes(State state, IEnumerable<string> changeScenes, bool active)
        {
            var scenes = state.GetValueOrSetDefault<IDictionary<string, bool>>(
                SceneConst.StateKey,
                () => new Dictionary<string, bool>()
            );
            foreach (var scene in changeScenes ?? new string[0]) scenes[scene] = active;
            return state;
        }

        public static IEnumerable<string> GetCleanUpScenes(
            State state,
            IEnumerable<string> nextScenes,
            IEnumerable<string> permanentScenes
        )
        {
            var allScenes = state.GetValue<IDictionary<string, bool>>(SceneConst.StateKey)
                .Select(it => it.Key)
                .ToList();

            var result = new Dictionary<string, bool>();
            foreach (var scene in allScenes) result[scene] = true;
            foreach (var scene in nextScenes ?? new string[0]) result[scene] = false;
            foreach (var scene in permanentScenes ?? new string[0]) result[scene] = false;

            return result
                .Where(x => x.Value)
                .Select(x => x.Key);
        }
    }
}