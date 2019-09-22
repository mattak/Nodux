using System;
using Nodux.Core;
using Nodux.PluginNode;
using UniRx;
using UnityEngine;

namespace Nodux.PluginUI
{
    [Serializable]
    [TypeSelectionEnable("Node")]
    public class GameObjectDestroyNode : Node
    {
        public Component Target;
        public float DelaySeconds;

        public GameObjectDestroyNode(INode parent) : base(parent)
        {
        }

        public override IDisposable Subscribe(IObserver<Any> observer)
        {
            return this.Parent.Subscribe(
                it =>
                {
                    GameObject.Destroy(this.Target.gameObject, this.DelaySeconds);
                    observer.OnNext(it);
                },
                observer.OnError,
                observer.OnCompleted
            );
        }
    }
}