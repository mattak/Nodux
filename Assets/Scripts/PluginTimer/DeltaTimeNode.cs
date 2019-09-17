using System;
using UniRx;
using Nodux.Core;
using Nodux.PluginNode;
using UnityEngine;

namespace Nodux.PluginTimer
{
    [Serializable]
    [TypeSelectionEnable("Node")]
    public class DeltaTimeNode : Node
    {
        public DeltaTimeNode(INode parent) : base(parent)
        {
        }

        public override IDisposable Subscribe(IObserver<Any> observer)
        {
            var update = Observable.EveryUpdate().Select(it => new Any(Time.deltaTime));
            if (this.Parent == null) return update.Subscribe(observer);
            return this.Parent.SelectMany(_ => update).Subscribe(observer);
        }
    }
}