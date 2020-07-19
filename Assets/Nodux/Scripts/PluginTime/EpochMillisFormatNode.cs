using System;
using Nodux.Core;
using Nodux.PluginNode;
using UniRx;

namespace Nodux.Nodux.Scripts.PluginTime
{
    [Serializable]
    [TypeSelectionEnable("Node")]
    public class EpochMillisFormatNode : Node
    {
        public string Format = "yy-MM-dd HH:mm:ss zz";

        public EpochMillisFormatNode(INode parent, string format) : base(parent)
        {
            this.Format = format;
        }

        public override IDisposable Subscribe(IObserver<Any> observer)
        {
            return this.Parent
                .Select(epochMillis => epochMillis.Value<long>())
                .Select(FormatToString)
                .Select(it => new Any(it))
                .Subscribe(observer);
        }

        private string FormatToString(long epochMillis)
        {
            var time = DateTimeOffset.FromUnixTimeMilliseconds(epochMillis);
            return time.ToString(this.Format);
        }
    }
}