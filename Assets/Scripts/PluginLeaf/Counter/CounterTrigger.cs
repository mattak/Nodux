using System;
using UnityEngine.UI;
using UnityLeaf.PluginNode;
using UnityLeaf.PluginState;

namespace UnityLeaf.PluginLeaf.Counter
{
    [Serializable]
    public class CounterTrigger : LeafBehaviour
    {
        public Button button;
        public string stateKey;
        public StoreHolder storeHolder;

        public override IDisposable Subscribe()
        {
            var buttonNode = new ButtonNode(button);
            var intNode = new IntNode(buttonNode, 1);
            var stateActionNode = new StateActionNode(
                intNode,
                stateKey,
                new IncrementReducer()
            );
            var stateWriterNode = new StateWriter(stateActionNode, this.storeHolder);
            return stateWriterNode.Subscribe();
        }
    }
}