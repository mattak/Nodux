using System;
using Nodux.Core;
using Nodux.PluginNode;
using Nodux.PluginState;
using UnityEngine.UI;

namespace Nodux.PluginCounter
{
    public class CounterNode : INode
    {
        public StoreHolder storeHolder;
        public string stateKey;
        public Text text;
        public INode Parent { get; set; }


        public IDisposable Subscribe(IObserver<Any> observer)
        {
            var stateReaderNode = new StateReaderNode(storeHolder, stateKey);
            var textWriterNode = new TextRenderNode(stateReaderNode, text, "{0}");
            return textWriterNode.Subscribe(observer);
        }
    }
}