using System;
using UnityEngine.UI;
using UnityLeaf.PluginNode;
using UnityLeaf.PluginState;

namespace UnityLeaf.PluginLeaf.Counter
{
    public class CounterRenderer : LeafBehaviour
    {
        public StoreHolder storeHolder;
        public string stateKey;
        public Text text;

        public override IDisposable Subscribe()
        {
            var stateReaderNode = new StateReaderNode(storeHolder, stateKey);
            var textWriterNode = new TextWriter(stateReaderNode, text, "{0}");
            return textWriterNode.Subscribe();
        }
    }
}