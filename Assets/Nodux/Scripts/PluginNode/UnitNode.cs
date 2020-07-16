using System;
using Nodux.Core;
using UniRx;

namespace Nodux.PluginNode
{
    [Serializable]
    [TypeSelectionEnable("Node")]
    public class UnitNode : Node
    {
        public UnitNode() : base(null)
        {
        }

        public UnitNode(INode parent) : base(parent)
        {
        }

        public override IDisposable Subscribe(IObserver<Any> observer)
        {
            if (Parent == null) return Observable.Return(new Any(Unit.Default)).Subscribe(observer);
            return Parent.Select(_ => new Any(Unit.Default)).Subscribe(observer);
        }
    }
}