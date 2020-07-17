using System.Collections.Generic;
using Nodux.Core;
using Nodux.Nodux.Scripts.PluginScene;
using Nodux.PluginPage;
using Nodux.PluginPage.Reducers;
using Nodux.PluginState;
using NUnit.Framework;

namespace Nodux.Tests.PluginPage.Reducers
{
    public class PagePopReducerTest
    {
        [Test]
        public void ReduceTest()
        {
            var reducer = new PagePopReducer();
            var state = new State();

            state.SetValue<PageStack>(PageConst.StateStackKey, new PageStack());
            state.SetValue<PageDefinition>(PageConst.StateDefinitionKey, new PageDefinition
            {
                PageScenes = new List<PageScene>
                {
                    new PageScene
                    {
                        Name = "Page1",
                        Scenes = new[]
                        {
                            "Scene1",
                            "Scene2"
                        },
                    },
                    new PageScene
                    {
                        Name = "Page2",
                        Scenes = new[]
                        {
                            "Scene1",
                            "Scene3"
                        },
                    }
                }
            });
            state.SetValue<IDictionary<string, bool>>(SceneConst.StateKey, new Dictionary<string, bool>()
            {
                {"Scene1", true},
                {"Scene2", false},
                {"Scene3", false},
            });

            {
                var pageStack = new PageStack();
                pageStack.Push(new Page {Name = "Page1", Data = null});
                pageStack.Push(new Page {Name = "Page2", Data = null});
                state.SetValue<PageStack>(PageConst.StateStackKey, pageStack);
            }

            // pop1
            {
                var action = new StateAction(reducer, "page", new Any());
                state = reducer.Reduce(state, action);

                var stack = state.GetValue<PageStack>(PageConst.StateStackKey);
                Assert.That(stack.Count, Is.EqualTo(1));
                Assert.That(stack.Peek().Name, Is.EqualTo("Page1"));

                var sceneMap = state.GetValue<IDictionary<string, bool>>(SceneConst.StateKey);
                Assert.That(sceneMap.Count, Is.EqualTo(3));
                Assert.That(sceneMap["Scene1"], Is.True);
                Assert.That(sceneMap["Scene2"], Is.True);
                Assert.That(sceneMap["Scene3"], Is.False);
            }

            // pop is failed if stack has only an element
            {
                var action = new StateAction(reducer, "page", new Any());
                state = reducer.Reduce(state, action);

                var stack = state.GetValue<PageStack>(PageConst.StateStackKey);
                Assert.That(stack.Count, Is.EqualTo(1));
                Assert.That(stack.Peek().Name, Is.EqualTo("Page1"));

                var sceneMap = state.GetValue<IDictionary<string, bool>>(SceneConst.StateKey);
                Assert.That(sceneMap.Count, Is.EqualTo(3));
                Assert.That(sceneMap["Scene1"], Is.True);
                Assert.That(sceneMap["Scene2"], Is.True);
                Assert.That(sceneMap["Scene3"], Is.False);
            }
        }
    }
}