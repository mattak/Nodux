using System;
using Nodux.Core;
using Nodux.PluginNode;
using Nodux.PluginState;
using UnityEngine.UI;

namespace Nodux.PluginCounter
{
    [Serializable]
    public class CounterTrigger : INode
    {
        public Button button;
        public string stateKey;
        public StoreHolder storeHolder;
        public INode Parent { get; set; }

        public IDisposable Subscribe(IObserver<Any> observer)
        {
            var buttonNode = new ButtonNode(button);
            var intNode = new IntNode(buttonNode, 1);
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