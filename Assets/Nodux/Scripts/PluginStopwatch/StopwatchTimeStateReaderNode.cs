using System;
using Nodux.Core;
using Nodux.PluginNode;
using Nodux.PluginState;
using UniRx;
using UnityEngine;

namespace Nodux.PluginStopwatch
{
    [Serializable]
    [TypeSelectionEnable("Node")]
    public class StopwatchTimeStateReaderNode : Node
    {
        [SerializeField] private string stateKey;
        [SerializeField] private StoreHolder storeHolder;

        public StopwatchTimeStateReaderNode(INode parent, string stateKey, StoreHolder holder) : base(parent)
        {
            this.stateKey = stateKey;
            this.storeHolder = holder;
        }

        public override IDisposable Subscribe(IObserver<Any> observer)
        {
            var reader = this.storeHolder.GetStore().Read(this.stateKey)
                .Select(it => new Any(it.Value<StopwatchValue>().elapsedTime));
            return reader.Subscribe(observer);
        }
    }
}