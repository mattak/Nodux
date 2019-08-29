using System;
using UnityLeaf.Core;

namespace UnityLeaf.PluginNode
{
    public interface INode
    {
        INode GetParent();

        IObservable<Any> GetObservable();
    }
}