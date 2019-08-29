using System;
using UnityLeaf.Core;

namespace UnityLeaf.PluginNode
{
    public abstract class RootNode : INode
    {
        public INode GetParent()
        {
            return null;
        }

        public abstract IObservable<Any> GetObservable();
    }
}