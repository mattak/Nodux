using System.Collections.Generic;
using Nodux.Core;

namespace Nodux.PluginState
{
    public interface IStoreAccessor
    {
        IDictionary<string, Any> GetRawData();
        void Replace(IDictionary<string, Any> dictionary);
        void Notify(string key);
    }
}