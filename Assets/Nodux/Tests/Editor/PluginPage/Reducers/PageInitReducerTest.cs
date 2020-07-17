using System.Collections.Generic;
using Nodux.Core;
using Nodux.PluginPage;
using Nodux.PluginPage.Reducers;
using Nodux.PluginState;
using NUnit.Framework;

namespace Nodux.Tests.PluginPage.Reducers
{
    public class PageInitReducerTest
    {
        [Test]
        public void ReduceTest()
        {
            var reducer = new PageInitReducer();
            var state = new State();

            Assert.That(state.Get(PageConst.StateDefinitionKey).IsNull, Is.True);

            var definition = new PageDefinition()
            {
                PageScenes = new List<PageScene>()
                {
                    new PageScene
                    {
                        Name = "Page1",
                        Scenes = new[]
                        {
                            "Scene1",
                            "Scene2",
                        }
                    },
                    new PageScene
                    {
                        Name = "Page2",
                        Scenes = new[]
                        {
                            "Scene1",
                            "Scene3",
                        }
                    }
                }
            };
            var action = new StateAction(reducer, "page", new Any(definition));
            var newState = reducer.Reduce(state, action);

            var newStateDefinition = newState.GetValue<PageDefinition>(PageConst.StateDefinitionKey);
            Assert.That(newStateDefinition.PageScenes.Count, Is.EqualTo(2));
            Assert.That(newStateDefinition.PageScenes[0].Name, Is.EqualTo("Page1"));
            Assert.That(newStateDefinition.PageScenes[1].Name, Is.EqualTo("Page2"));
        }
    }
}