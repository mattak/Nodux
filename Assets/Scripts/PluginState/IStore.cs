using System;
using UnityLeaf.Core;

namespace UnityLeaf.PluginState
{
    public interface IStore
    {
        void Write(StateAction action);
        IObservable<Any> Read(string key);
    }
}