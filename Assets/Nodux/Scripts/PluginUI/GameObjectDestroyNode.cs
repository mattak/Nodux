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
        [SerializeField] private Component target;
        [SerializeField] private float delaySeconds;

        public GameObjectDestroyNode(INode parent, Component component, float delaySeconds) : base(parent)
        {
            this.target = component;
            this.delaySeconds = delaySeconds;
        }

        public override IDisposable Subscribe(IObserver<Any> observer)
        {
            return this.Parent.Subscribe(
                it =>
                {
                    GameObject.Destroy(this.target.gameObject, this.delaySeconds);
                    observer.OnNext(it);
                },
                observer.OnError,
                observer.OnCompleted
            );
        }
    }
}