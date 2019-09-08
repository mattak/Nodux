using System;
using UnityLeaf.PluginState;
using UniRx;
using UnityEngine;
using UnityLeaf.Core;

namespace UnityLeaf.PluginNode
{
    [Serializable]
    [TypeSelectionEnable("Node")]
    public class StateWriterNode : Node
    {
       [SerializeField] private StoreHolder StoreHolder;

        public StateWriterNode(INode parent, StoreHolder storeHolder) : base(parent)
        {
            this.StoreHolder = storeHolder;
        }

        public override IDisposable Subscribe(IObserver<Any> observer)
        {
            return this.Parent.Subscribe(
                it =>
                {
                    var action = it.Value<StateAction>();
                    StoreHolder.GetStore().Write(action);
                    observer.OnNext(it);
                },
                err => observer.OnError(err),
                () => observer.OnCompleted());
        }
    }
}