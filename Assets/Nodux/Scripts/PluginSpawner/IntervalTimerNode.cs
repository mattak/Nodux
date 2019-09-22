using System;
using Nodux.Core;
using Nodux.PluginNode;
using UniRx;
using UnityEngine;

namespace Nodux.PluginSpawner
{
    [Serializable]
    [TypeSelectionEnable("Node")]
    public class IntervalTimerNode : Node
    {
        [SerializeField] private float IntervalSeconds = 1.0f;

        public IntervalTimerNode(INode parent) : base(parent)
        {
        }

        public override IDisposable Subscribe(IObserver<Any> observer)
        {
            if (this.IntervalSeconds <= 0f) throw new ArgumentException("interval must be greater than 0");

            return Observable.EveryUpdate()
                .Select(_ => Time.deltaTime)
                .Scan(0f, (sum, it) => sum + it)
                .Select(value => (int) Math.Floor(value / IntervalSeconds))
                .DistinctUntilChanged()
                .Select(it => new Any(it))
                .Subscribe(observer);
        }
    }
}