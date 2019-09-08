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
        [SerializeField] private StoreHolder StoreHolder;
        [SerializeField] private string StateKey;

        public StateReaderNode(StoreHolder storeHolder, string stateKey)
        {
            this.StoreHolder = storeHolder;
            this.StateKey = stateKey;
        }

        public override IDisposable Subscribe(IObserver<Any> observer)
        {
            return this.StoreHolder.GetStore().Read(this.StateKey)
                .Subscribe(observer);
        }
    }
}