using System;
using UniRx;
using UnityEngine;
using Nodux.Core;
using Nodux.PluginNode;
using UnityEngine.Serialization;

namespace Nodux.PluginState
{
    [Serializable]
    [TypeSelectionEnable("Node")]
    public class StateWriterNode : Node
    {
       [SerializeField] private StoreHolder storeHolder;

        public StateWriterNode(INode parent, StoreHolder storeHolder) : base(parent)
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