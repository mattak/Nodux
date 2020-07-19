using System;
using Nodux.Core;
using Nodux.PluginNode;
using Nodux.PluginState;
using Nodux.PluginUI;
using UnityEngine.UI;

namespace Nodux.PluginCounter
{
    [Serializable]
    public class CounterTrigger : Node
    {
        public Button button;
        public string stateKey;
        public StoreHolder storeHolder;

        public CounterTrigger(INode parent, Button button, string stateKey, StoreHolder holder) : base(parent)
        {
            this.button = button;
            this.stateKey = stateKey;
            this.storeHolder = holder;
        }

        public override IDisposable Subscribe(IObserver<Any> observer)
        {
            var buttonNode = new OnClickButtonNode(button);
            var intNode = new ConstIntNode(buttonNode, 1);
            var stateActionNode = new StateActionNode(
                intNode,
                stateKey,
                new IncrementReducer()
            );
            var stateWriterNode = new StateWriterNode(stateActionNode, this.storeHolder);
            return stateWriterNode.Subscribe(observer);
        }
    }
}