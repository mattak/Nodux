using System;
using Nodux.Core;

namespace Nodux.PluginNode
{
    public interface INode : IObservable<Any>
    {
        INode Parent { get; set; }
    }
}