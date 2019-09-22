using System;
using Nodux.Core;
using Nodux.PluginNode;
using UniRx;
using UnityEngine;

namespace Nodux.PluginOperator
{
    [Serializable]
    [TypeSelectionEnable("Node")]
    public class GreaterEqualNode : Node
    {
        [SerializeField] private float Threshold = 0f;

        public GreaterEqualNode(INode parent) : base(parent)
        {
        }

        public override IDisposable Subscribe(IObserver<Any> observer)
        {
            return this.Parent
                .Select(it => new Any(it.Value<float>() >= this.Threshold))
                .Subscribe(observer);
        }
    }
}