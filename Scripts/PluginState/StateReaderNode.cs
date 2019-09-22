using System;
using UnityEngine;
using Nodux.Core;
using Nodux.PluginNode;
using UniRx;

namespace Nodux.PluginState
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
            var store = this.StoreHolder.GetStore();
            var read = store.Read(this.StateKey);

            if (store.HasKey(this.StateKey)) read = read.StartWith(store.Get(this.StateKey));

            return read.Subscribe(observer);
        }
    }
}