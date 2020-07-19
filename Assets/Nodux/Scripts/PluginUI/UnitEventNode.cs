using System;
using Nodux.Core;
using Nodux.PluginNode;
using UniRx;

namespace Nodux.PluginUI
{
    [Serializable]
    [TypeSelectionEnable("Node")]
    public class UnitEventNode : Node
    {
        private IObservable<Unit> trigger;

        public UnitEventNode(INode parent, IObservable<Unit> trigger) : base(parent)
        {
            this.trigger = trigger;
        }

        public override IDisposable Subscribe(IObserver<Any> observer)
        {
            if (this.Parent == null) return this.trigger.Select(it => new Any(it)).Subscribe(observer);
            return this.Parent.Select(_ => this.trigger).Switch().Select(it => new Any(it)).Subscribe(observer);
        }
    }
}