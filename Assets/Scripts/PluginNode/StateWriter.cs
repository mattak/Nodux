using System;
using UnityLeaf.PluginState;
using UniRx;
using UnityEngine;
using UnityLeaf.Core;

namespace UnityLeaf.PluginNode
{
    public class StateWriter : Node
    {
       [SerializeField] private StoreHolder storeHolder;

        public StateWriter(INode parent, StoreHolder storeHolder) : base(parent)
        {
            this.storeHolder = storeHolder;
        }

        public override IDisposable Subscribe(IObserver<Any> observer)
        {
            return this.Parent.Subscribe(
                it =>
                {
                    var action = it.Value<StateAction>();
                    storeHolder.GetStore().Write(action);
                    observer.OnNext(it);
                },
                err => observer.OnError(err),
                () => observer.OnCompleted());
        }
    }
}