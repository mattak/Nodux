using System;
using Nodux.Core;
using Nodux.PluginNode;
using UniRx;
using UnityEngine;

namespace Nodux.Nodux.Scripts.PluginTime
{
    [Serializable]
    [TypeSelectionEnable("Node")]
    public class EpochMillisFormatNode : Node
    {
        [SerializeField] private string format = "yy-MM-dd HH:mm:ss zz";

        public EpochMillisFormatNode(INode parent, string format) : base(parent)
        {
            this.format = format;
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
            return time.ToString(this.format);
        }
    }
}