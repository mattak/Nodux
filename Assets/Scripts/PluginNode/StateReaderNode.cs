using System;
using UnityEngine;
using UnityLeaf.Core;
using UnityLeaf.PluginState;

namespace UnityLeaf.PluginNode
{
    [Serializable]
    [TypeSelectionEnable("Node")]
    public class StateReaderNode : RootNode
    {
        [SerializeField] private StoreHolder storeHolder;
        [SerializeField] private string stateKey;

        public StateReaderNode(StoreHolder storeHolder, string stateKey)
        {
            this.storeHolder = storeHolder;
            this.stateKey = stateKey;
        }

        public override IDisposable Subscribe(IObserver<Any> observer)
        {
            return this.storeHolder.GetStore().Read(this.stateKey)
                .Subscribe(observer);
        }
    }
}