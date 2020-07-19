using System;
using Nodux.Core;
using Nodux.PluginNode;
using Nodux.PluginState;
using UnityEngine.UI;

namespace Nodux.PluginCounter
{
    public class CounterNode : Node
    {
        public StoreHolder storeHolder;
        public string stateKey;
        public Text text;

        public CounterNode(INode parent, StoreHolder holder, string stateKey, Text text) : base(parent)
        {
            this.storeHolder = holder;
            this.stateKey = stateKey;
            this.text = text;
        }

        public override IDisposable Subscribe(IObserver<Any> observer)
        {
            var stateReaderNode = new StateReaderNode(storeHolder, stateKey);
            var textWriterNode = new TextRenderNode(stateReaderNode, text, "{0}");
            return textWriterNode.Subscribe(observer);
        }
    }
}