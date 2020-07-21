using System;
using UnityEngine;
using Nodux.Core;
using Nodux.PluginNode;
using UniRx;
using UnityEngine.Serialization;

namespace Nodux.PluginState
{
    [Serializable]
    [TypeSelectionEnable("Node")]
    public class StateReaderNode : Node
    {
        [SerializeField] private StoreHolder storeHolder;
        [SerializeField] private string stateKey;

        public StateReaderNode(StoreHolder storeHolder, string stateKey) : base(null)
        {
            this.storeHolder = storeHolder;
            this.stateKey = stateKey;
        }

        public override IDisposable Subscribe(IObserver<Any> observer)
        {
            var store = this.storeHolder.GetStore();
            var read = store.Read(this.stateKey);

            if (store.HasKey(this.stateKey)) read = read.StartWith(store.Get(this.stateKey));

            return read.Subscribe(observer);
        }
    }
}