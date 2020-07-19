using System;
using Nodux.Core;
using Nodux.PluginNode;
using UniRx;

namespace Nodux.Nodux.Scripts.PluginTime
{
    [Serializable]
    [TypeSelectionEnable("Node")]
    public class EpochMillisNowNode : Node
    {
        public EpochMillisNowNode(INode parent) : base(parent)
        {
        }

        public override IDisposable Subscribe(IObserver<Any> observer)
        {
            if (this.Parent == null) return Observable.Return(Now()).Subscribe(observer);
            return this.Parent.Select(_ => Now()).Subscribe(observer);
        }

        private Any Now()
        {
            var millis = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
            return new Any(millis);
        }
    }
}