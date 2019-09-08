using System;
using UnityLeaf.Core;

namespace UnityLeaf.PluginNode
{
    [Serializable]
    [TypeSelectionEnable("Node")]
    public abstract class RootNode : INode
    {
        public INode Parent { get; set; } = null;

        public abstract IDisposable Subscribe(IObserver<Any> observer);
    }
}