using System;
using Nodux.Core;
using UniRx;
using UnityEngine;

namespace Nodux.PluginNode
{
    [Serializable]
    [TypeSelectionEnable("Node")]
    public class ConstBoolNode : Node
    {
        [SerializeField] private bool Value;

        public ConstBoolNode(INode parent, bool value) : base(parent)
        {
            this.Value = value;
        }

        public override IDisposable Subscribe(IObserver<Any> observer)
        {
            if (this.Parent == null) return Observable.Return(new Any(this.Value)).Subscribe(observer);
            return this.Parent.Select(_ => new Any(this.Value)).Subscribe(observer);
        }
    }
}