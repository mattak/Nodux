using System;
using UniRx;
using UnityLeaf.Core;
using UnityLeaf.PluginNode;
using UnityLeaf.PluginLeaf;

namespace UnityLeaf.PluginNode
{
    public class OnStartNode : RootNode
    {
        private OnStartTriggerBehaviour component;

        public OnStartNode(OnStartTriggerBehaviour component)
        {
            this.component = component;
        }

        public override IObservable<Any> GetObservable()
        {
            return this.component.OnStartObservable.Select(it => new Any(it));
        }
    }
}