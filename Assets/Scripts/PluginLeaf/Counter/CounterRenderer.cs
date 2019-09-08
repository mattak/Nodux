using System;
using UnityEngine.UI;
using UnityLeaf.Core;
using UnityLeaf.PluginNode;
using UnityLeaf.PluginState;

namespace UnityLeaf.PluginLeaf.Counter
{
    public class CounterRenderer : INode
    {
        public StoreHolder storeHolder;
        public string stateKey;
        public Text text;
        public INode Parent { get; set; }

        public IDisposable Subscribe(IObserver<Any> observer)
        {
            var stateReaderNode = new StateReaderNode(storeHolder, stateKey);
            var textWriterNode = new TextWriter(stateReaderNode, text, "{0}");
            return textWriterNode.Subscribe(observer);
        }
    }
}