using System;
using UnityLeaf.Core;

namespace UnityLeaf.PluginNode
{
    public interface INode : IObservable<Any>
    {
        INode Parent { get; set; }
    }
}