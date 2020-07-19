using System.Collections.Generic;
using Nodux.Core;
using Nodux.PluginScene;
using Nodux.PluginScene.Reducers;
using Nodux.PluginState;
using NUnit.Framework;

namespace Nodux.Tests.PluginScene.Reducers
{
    public class SceneAddReducerTest
    {
        [Test]
        public void ReduceTest()
        {
            var reducer = new SceneAddReducer();
            var state = new State();

            Assert.That(state.Get(SceneConst.StateKey).IsNull, Is.True);
            state.SetValue<IDictionary<string, bool>>(SceneConst.StateKey, new Dictionary<string, bool>()
            {
                {"Scene1", false},
                {"Scene2", false},
                {"Scene3", false},
            });

            {
                var action = new StateAction(reducer, "scene", new Any(new[] {"Scene1"}));
                state = reducer.Reduce(state, action);

                var map = state.GetValue<IDictionary<string, bool>>(SceneConst.StateKey);
                Assert.That(map.Count, Is.EqualTo(3));
                Assert.That(map["Scene1"], Is.True);
                Assert.That(map["Scene2"], Is.False);
                Assert.That(map["Scene3"], Is.False);
            }

            {
                var action = new StateAction(reducer, "scene", new Any(new[] {"Scene2", "Scene3"}));
                state = reducer.Reduce(state, action);

                var map = state.GetValue<IDictionary<string, bool>>(SceneConst.StateKey);
                Assert.That(map.Count, Is.EqualTo(3));
                Assert.That(map["Scene1"], Is.True);
                Assert.That(map["Scene2"], Is.True);
                Assert.That(map["Scene3"], Is.True);
            }
        }
    }
}