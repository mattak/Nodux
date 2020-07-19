using System;
using UnityEngine;

namespace Nodux.PluginPage
{
    [Serializable]
    public class Page
    {
        public string Name;
        [SerializeReference] public IPageData Data;

        public TData GetData<TData>() where TData : IPageData
        {
            return (TData) Data;
        }
    }
}