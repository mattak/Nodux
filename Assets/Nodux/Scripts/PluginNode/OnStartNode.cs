using System;
using UnityEngine;
using Nodux.Core;
using UniRx;

namespace Nodux.PluginNode
{
    [Serializable]
    [TypeSelectionEnable("Node")]
    public class OnStartNode : Node
    {
        [SerializeField] private OnStartTriggerBehaviour component;

        public OnStartNode(OnStartTriggerBehaviour component) : base(null)
        {
            this.component = component;
        }

        public override IDisposable Subscribe(IObserver<Any> observer)
        {
            return this.component.OnStartObservable.Select(it => new Any(it)).Subscribe(observer);
        }
    }
}