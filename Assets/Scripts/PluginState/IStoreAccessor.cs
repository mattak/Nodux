using System.Collections.Generic;
using UnityLeaf.Core;

namespace UnityLeaf.PluginState
{
    public interface IStoreAccessor
    {
        IDictionary<string, Any> GetRawData();
        void Replace(IDictionary<string, Any> dictionary);
        void Notify(string key);
    }
}