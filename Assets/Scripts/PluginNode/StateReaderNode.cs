using System;
using UnityLeaf.Core;
using UnityLeaf.PluginState;

namespace UnityLeaf.PluginNode
{
    public class StateReaderNode : RootNode
    {
        private StoreHolder storeHolder;
        private string stateKey;

        public StateReaderNode(StoreHolder storeHolder, string stateKey)
        {
            this.storeHolder = storeHolder;
            this.stateKey = stateKey;
        }

        public override IObservable<Any> GetObservable()
        {
            return this.storeHolder.GetStore().Read(this.stateKey);
        }
    }
}