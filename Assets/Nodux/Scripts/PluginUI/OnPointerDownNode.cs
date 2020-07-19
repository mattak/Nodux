using System;
using Nodux.Core;
using Nodux.PluginNode;
using UniRx;
using UniRx.Triggers;
using UnityEngine.EventSystems;

namespace Nodux.PluginUI
{
    [Serializable]
    [TypeSelectionEnable("Node")]
    public class OnPointerDownNode : Node
    {
        public UIBehaviour Behaviour;

        public OnPointerDownNode(UIBehaviour behaviour) : base(null)
        {
            this.Behaviour = behaviour;
        }

        public override IDisposable Subscribe(IObserver<Any> observer)
        {
            return this.Behaviour.OnPointerDownAsObservable()
                .Select(it => new Any(it))
                .Subscribe(observer);
        }
    }
}