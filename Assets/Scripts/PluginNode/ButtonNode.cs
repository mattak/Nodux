using System;
using UniRx;
using UnityEngine;
using Nodux.Core;
using UnityEngine.UI;

namespace Nodux.PluginNode
{
    [Serializable]
    [TypeSelectionEnable("Node")]
    public class ButtonNode : RootNode
    {
        [SerializeField] private Button Button;

        public ButtonNode(Button button)
        {
            this.Button = button;
        }

        public override IDisposable Subscribe(IObserver<Any> observer)
        {
            return this.Button.OnClickAsObservable().Select(it => new Any(it)).Subscribe(observer);
        }
    }
}