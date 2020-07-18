using System;
using Nodux.Core;
using UniRx;
using UnityEngine;

namespace Nodux.PluginNode
{
    [Serializable]
    [TypeSelectionEnable("Node")]
    public class ConstVector2Node : Node
    {
        [SerializeField] private Vector2 Value;

        public ConstVector2Node(INode parent, Vector2 value) : base(parent)
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