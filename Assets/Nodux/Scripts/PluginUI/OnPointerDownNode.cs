using System;
using Nodux.Core;
using Nodux.PluginNode;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Nodux.PluginUI
{
    [Serializable]
    [TypeSelectionEnable("Node")]
    public class OnPointerDownNode : Node
    {
        [SerializeField] private UIBehaviour behaviour;

        public OnPointerDownNode(UIBehaviour behaviour) : base(null)
        {
            this.behaviour = behaviour;
        }

        public override IDisposable Subscribe(IObserver<Any> observer)
        {
            return this.behaviour.OnPointerDownAsObservable()
                .Select(it => new Any(it))
                .Subscribe(observer);
        }
    }
}