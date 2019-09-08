using System;
using UniRx;
using UnityEngine;
using UnityLeaf.Core;
using UnityLeaf.PluginLeaf;

namespace UnityLeaf.PluginNode
{
    [Serializable]
    [TypeSelectionEnable("Node")]
    public class OnStartNode : RootNode
    {
        [SerializeField] private OnStartTriggerBehaviour component;

        public OnStartNode(OnStartTriggerBehaviour component)
        {
            this.component = component;
        }

        public override IDisposable Subscribe(IObserver<Any> observer)
        {
            return this.component.OnStartObservable.Select(it => new Any(it)).Subscribe(observer);
        }
    }
}