using System;
using System.Collections.Generic;

namespace Nodux.PluginPage
{
    [Serializable]
    public class PageDefinition
    {
        public List<PageScene> PageScenes;

        public PageScene GetPageScene(string name)
        {
            foreach (var pageScene in PageScenes)
            {
                if (pageScene != null && pageScene.Name.Equals(name))
                {
                    return pageScene;
                }
            }

            return null;
        }
    }
}