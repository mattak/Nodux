using System;
using Nodux.Core;
using Nodux.PluginNode;
using UniRx;
using UnityEngine;

namespace Nodux.PluginOperator
{
    [Serializable]
    [TypeSelectionEnable("Node")]
    public class MathFloorNode : Node
    {
        public MathFloorNode(INode parent) : base(parent)
        {
        }

        public override IDisposable Subscribe(IObserver<Any> observer)
        {
            return this.Parent
                .Select(it => new Any(Mathf.Floor(it.Value<float>())))
                .Subscribe(observer);
        }
    }
}