using System;
using Nodux.Core;
using Nodux.PluginNode;
using UniRx;
using UnityEngine;

namespace Nodux.PluginOperator
{
    [Serializable]
    [TypeSelectionEnable("Node")]
    public class FloatSubtractNode : Node
    {
        [SerializeField] private float from = default;

        public FloatSubtractNode(INode parent, float from) : base(parent)
        {
            this.from = from;
        }

        public override IDisposable Subscribe(IObserver<Any> observer)
        {
            return this.Parent
                .Select(it => new Any(from - it.Value<float>()))
                .Subscribe(observer);
        }
    }
}