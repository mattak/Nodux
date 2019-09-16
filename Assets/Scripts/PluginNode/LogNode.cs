using System;
using UniRx;
using Nodux.Core;

namespace Nodux.PluginNode
{
    [Serializable]
    [TypeSelectionEnable("Node")]
    public class LogNode : Node
    {
        public LogNode(INode parent) : base(parent)
        {
        }

        public override IDisposable Subscribe(IObserver<Any> observer)
        {
            return this.Parent.Subscribe(
                it =>
                {
                    UnityEngine.Debug.Log(it);
                    observer.OnNext(it);
                },
                err =>
                {
                    UnityEngine.Debug.LogError(err);
                    observer.OnError(err);
                },
                () =>
                {
                    UnityEngine.Debug.Log("OnCompleted");
                    observer.OnCompleted();
                });
        }
    }
}