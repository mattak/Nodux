using System;
using Nodux.Core;
using UniRx;

namespace Nodux.PluginNode
{
    [Serializable]
    [TypeSelectionEnable("Node")]
    public class ValueNode : Node
    {
        private Any value;

        public ValueNode(object value) : this(null, value)
        {
        }

        public ValueNode(INode parent, object value) : base(parent)
        {
            this.value = new Any(value);
        }

        public override IDisposable Subscribe(IObserver<Any> observer)
        {
            if (Parent == null) return Observable.Return(value).Subscribe(observer);
            return Parent.Select(_ => value).Subscribe(observer);
        }
    }
}