using System;
using UniRx;
using UnityEngine;
using Nodux.Core;
using Nodux.PluginNode;
using UnityEngine.UI;

namespace Nodux.PluginUI
{
    [Serializable]
    [TypeSelectionEnable("Node")]
    public class OnClickButtonNode : RootNode
    {
        [SerializeField] private Button Button;

        public OnClickButtonNode(Button button)
        {
            this.Button = button;
        }

        public override IDisposable Subscribe(IObserver<Any> observer)
        {
            return this.Button.OnClickAsObservable().Select(it => new Any(it)).Subscribe(observer);
        }
    }
}