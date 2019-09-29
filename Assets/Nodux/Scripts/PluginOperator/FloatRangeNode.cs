using System;
using Nodux.Core;
using Nodux.PluginNode;
using UniRx;
using UnityEngine;

namespace Nodux.PluginOperator
{
    [Serializable]
    [TypeSelectionEnable("Node")]
    public class FloatRangeNode : Node
    {
        [SerializeField] private float min = 0f;
        [SerializeField] private float max = 1f;

        public FloatRangeNode(INode parent, float min, float max) : base(parent)
        {
            this.min = min;
            this.max = max;
        }

        public override IDisposable Subscribe(IObserver<Any> observer)
        {
            return this.Parent
                .Select(it =>
                {
                    var f = it.Value<float>();
                    f = Mathf.Max(min, f);
                    f = Mathf.Min(max, f);
                    return new Any(f);
                })
                .Subscribe(observer);
        }
    }
}