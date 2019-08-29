using System;
using UniRx;
using UnityLeaf.Core;
using UnityEngine.UI;

namespace UnityLeaf.PluginNode
{
    public class ButtonNode : RootNode
    {
        public Button Button;

        public ButtonNode(Button button)
        {
            this.Button = button;
        }

        public override IObservable<Any> GetObservable()
        {
            return this.Button.OnClickAsObservable().Select(it => new Any(it));
        }
    }
}