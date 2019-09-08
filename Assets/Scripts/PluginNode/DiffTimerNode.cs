using System;
using UniRx;
using UnityLeaf.Core;
using UnityEngine;

namespace UnityLeaf.PluginNode
{
    public class DiffTimerNode : RootNode
    {
        public override IDisposable Subscribe(IObserver<Any> observer)
        {
            return Observable.EveryUpdate().Select(it => new Any(Time.deltaTime)).Subscribe(observer);
        }
    }
}