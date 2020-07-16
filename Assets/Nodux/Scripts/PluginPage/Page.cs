using System;

namespace Nodux.PluginPage
{
    [Serializable]
    public class Page
    {
        public string Name;
        public IPageData Data;

        public TData GetData<TData>() where TData : IPageData
        {
            return (TData) Data;
        }
    }
}