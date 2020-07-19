using System;
using Nodux.Core;
using UniRx;
using UnityEngine;

namespace Nodux.PluginNode
{
    [Serializable]
    [TypeSelectionEnable("Node")]
    public class ConstIntArrayNode : Node
    {
        [SerializeField] private int[] Value;

        public ConstIntArrayNode(INode parent, int[] value) : base(parent)
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