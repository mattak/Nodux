using System;
using Nodux.Core;
using UniRx;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Nodux.PluginNode
{
    [Serializable]
    [TypeSelectionEnable("Node")]
    public class ConstUnityObjectNode : Node
    {
        [SerializeField] private Object Value;

        public ConstUnityObjectNode(INode parent, Object value) : base(parent)
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