using System;
using Nodux.Core;

namespace Nodux.PluginState
{
    public interface IStore
    {
        void Write(StateAction action);
        IObservable<Any> Read(string key);
    }
}