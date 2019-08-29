using System;
using UniRx;
using UnityLeaf.Core;

namespace UnityLeaf.PluginNode
{
    public class IntNode : Node
    {
        private int Value;

        public IntNode(INode parent, int value) : base(parent)
        {
            this.Value = value;
        }

        public override IObservable<Any> GetObservable()
        {
            if (this.GetParent() == null) return Observable.Return(new Any(this.Value));
            return this.GetParent().GetObservable().Select(_ => new Any(this.Value));
        }
    }
}