using System;
using Nodux.Core;
using Nodux.PluginNode;
using Nodux.PluginState;
using UniRx;

namespace Nodux.PluginStopwatch
{
    [Serializable]
    [TypeSelectionEnable("Node")]
    public class StopwatchTimeStateReaderNode : Node
    {
        public string StateKey;
        public StoreHolder StoreHolder;

        public StopwatchTimeStateReaderNode(INode parent) : base(parent)
        {
        }

        public override IDisposable Subscribe(IObserver<Any> observer)
        {
            var reader = this.StoreHolder.GetStore().Read(this.StateKey)
                .Select(it => new Any(it.Value<StopwatchValue>().ElapsedTime));
            return reader.Subscribe(observer);
        }
    }
}