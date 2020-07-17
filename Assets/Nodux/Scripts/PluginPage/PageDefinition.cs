using System;

namespace Nodux.PluginPage
{
    [Serializable]
    public class PageDefinition
    {
        public string[] PermanentScenes;
        public PageScene[] PageScenes;

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