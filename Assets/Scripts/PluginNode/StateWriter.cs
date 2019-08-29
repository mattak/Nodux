using System;
using UnityLeaf.PluginState;
using UniRx;

namespace UnityLeaf.PluginNode
{
    public class StateWriter : LeafNode
    {
        private StoreHolder storeHolder;

        public StateWriter(INode parent, StoreHolder storeHolder) : base(parent)
        {
            this.storeHolder = storeHolder;
        }

        public override IDisposable Subscribe()
        {
            return this.GetParent().GetObservable().Subscribe(it =>
            {
                var action = it.Value<StateAction>();
                storeHolder.GetStore().Write(action);
            });
        }
    }

}