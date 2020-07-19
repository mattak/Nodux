using System;
using Nodux.PluginPage;

namespace Example.WhackAMole
{
    [Serializable]
    public class MolePage : Page
    {
        public IPageData Data = new SamplePageData();
    }
}