using System;
using Nodux.Core;
using Nodux.PluginNode;
using UniRx;
using UnityEngine;

namespace Nodux.PluginOperator
{
    [Serializable]
    [TypeSelectionEnable("Node")]
    public class BoolFilterNode : Node
    {
        [SerializeField] private bool FilterValue = true;
        
        public BoolFilterNode(INode parent) : base(parent)
        {
        }

        public override IDisposable Subscribe(IObserver<Any> observer)
        {
            return this.Parent
                .Where(it => it.Value<bool>() == this.FilterValue)
                .Subscribe(observer);
        }
    }
}