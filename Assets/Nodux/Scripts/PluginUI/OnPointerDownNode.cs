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
    public class OnPointerDownNode : RootNode
    {
        public UIBehaviour Behaviour;

        public override IDisposable Subscribe(IObserver<Any> observer)
        {
            return this.Behaviour.OnPointerDownAsObservable()
                .Select(it => new Any(it))
                .Subscribe(observer);
        }
    }
}