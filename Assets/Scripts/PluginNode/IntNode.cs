using System;
using UniRx;
using UnityEngine;
using UnityLeaf.Core;

namespace UnityLeaf.PluginNode
{
    [Serializable]
    [TypeSelectionEnable("Node")]
    public class IntNode : Node
    {
        [SerializeField] private int Value;

        public IntNode(INode parent, int value) : base(parent)
        {
            this.Value = value;
        }

        public override IDisposable Subscribe(IObserver<Any> observer)
        {
            if (this.Parent == null)
            {
                observer.OnNext(new Any(this.Value));
                return Disposable.Empty;
            }

            return this.Parent.Select(_ => new Any(this.Value)).Subscribe(observer);
        }
    }
}