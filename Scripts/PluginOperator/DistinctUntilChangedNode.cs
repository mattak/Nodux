using System;
using Nodux.Core;
using Nodux.PluginNode;
using UniRx;

namespace Nodux.PluginOperator
{
    [Serializable]
    [TypeSelectionEnable("Node")]
    public class DistinctUntilChangedNode : Node
    {
        public DistinctUntilChangedNode(INode parent) : base(parent)
        {
        }

        public override IDisposable Subscribe(IObserver<Any> observer)
        {
            return this.Parent.DistinctUntilChanged(it => it.Object).Subscribe(observer);
        }
    }
}