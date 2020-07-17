using System;
using System.Collections.Generic;
using Nodux.Core;
using Nodux.Nodux.Scripts.PluginScene;
using Nodux.PluginPage;
using Nodux.PluginPage.Reducers;
using Nodux.PluginState;
using NUnit.Framework;

namespace Nodux.Tests.PluginPage.Reducers
{
    public class PageReplaceReducerTest
    {
        [Test]
        public void ReduceTest()
        {
            var reducer = new PageReplaceReducer();
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


            // move to page1
            {
                var page1 = new Page {Name = "Page1", Data = null};
                var action1 = new StateAction(reducer, "page", new Any(page1));
                state = reducer.Reduce(state, action1);

                var sceneMap = state.GetValue<IDictionary<string, bool>>(SceneConst.StateKey);
                Assert.That(sceneMap.Count, Is.EqualTo(3));
                Assert.That(sceneMap["Scene1"], Is.True);
                Assert.That(sceneMap["Scene2"], Is.True);
                Assert.That(sceneMap["Scene3"], Is.False);

                var pageStack = state.GetValue<PageStack>(PageConst.StateStackKey);
                Assert.That(pageStack.Count, Is.EqualTo(1));
                Assert.That(pageStack.Peek().Name, Is.EqualTo("Page1"));
            }

            // move to page2
            {
                var page2 = new Page {Name = "Page2", Data = null};
                var action2 = new StateAction(reducer, "page", new Any(page2));
                state = reducer.Reduce(state, action2);

                var sceneMap = state.GetValue<IDictionary<string, bool>>(SceneConst.StateKey);
                Assert.That(sceneMap.Count, Is.EqualTo(3));
                Assert.That(sceneMap["Scene1"], Is.True);
                Assert.That(sceneMap["Scene2"], Is.False);
                Assert.That(sceneMap["Scene3"], Is.True);

                var pageStack = state.GetValue<PageStack>(PageConst.StateStackKey);
                Assert.That(pageStack.Count, Is.EqualTo(1));
                Assert.That(pageStack.Peek().Name, Is.EqualTo("Page2"));
            }

            // assert
            {
                var action = new StateAction(reducer, "page", new Any("page"));
                Assert.Throws<ArgumentException>(() => { state = reducer.Reduce(state, action); });
            }
        }
    }
}