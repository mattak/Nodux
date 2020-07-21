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
        [SerializeField] private float intervalSeconds;

        public IntervalTimerNode(INode parent, float intervalSeconds) : base(parent)
        {
            this.intervalSeconds = intervalSeconds;
        }

        public override IDisposable Subscribe(IObserver<Any> observer)
        {
            if (this.intervalSeconds <= 0f) throw new ArgumentException("interval must be greater than 0");

            return Observable.EveryUpdate()
                .Select(_ => Time.deltaTime)
                .Scan(0f, (sum, it) => sum + it)
                .Select(value => (int) Math.Floor(value / intervalSeconds))
                .DistinctUntilChanged()
                .Select(it => new Any(it))
                .Subscribe(observer);
        }
    }
}