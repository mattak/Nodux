using System;
using Nodux.Core;
using Nodux.PluginNode;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace Nodux.PluginUI
{
    [Serializable]
    [TypeSelectionEnable("Node")]
    public class OnClickButtonNode : Node
    {
        [SerializeField] private Button button;

        public OnClickButtonNode(Button button) : base(null)
        {
            this.button = button;
        }

        public override IDisposable Subscribe(IObserver<Any> observer)
        {
            return this.button.OnClickAsObservable().Select(it => new Any(it)).Subscribe(observer);
        }
    }
}