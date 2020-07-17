using System.Collections.Generic;
using Nodux.Core;
using Nodux.PluginScene;
using Nodux.PluginScene.Reducers;
using Nodux.PluginState;
using NUnit.Framework;

namespace Nodux.Tests.PluginScene.Reducers
{
    public class SceneSyncReducerTest
    {
        [Test]
        public void ReduceTest()
        {
            var reducer = new SceneSyncReducer();
            var state = new State();

            Assert.That(state.Get(SceneConst.StateKey).IsNull, Is.True);
            state.SetValue<IDictionary<string, bool>>(SceneConst.StateKey, new Dictionary<string, bool>()
            {
                {"Scene1", false},
                {"Scene2", false},
                {"Scene3", true},
                {"Scene4", true},
            });

            {
                var action = new StateAction(reducer, "scene", new Any(new Dictionary<string, bool>
                {
                    {"Scene1", false},
                    {"Scene2", true},
                    {"Scene3", false},
                }));
                state = reducer.Reduce(state, action);

                var map = state.GetValue<IDictionary<string, bool>>(SceneConst.StateKey);
                Assert.That(map.Count, Is.EqualTo(4));
                Assert.That(map["Scene1"], Is.False);
                Assert.That(map["Scene2"], Is.True);
                Assert.That(map["Scene3"], Is.False);
                Assert.That(map["Scene3"], Is.False);
            }
        }
    }
}