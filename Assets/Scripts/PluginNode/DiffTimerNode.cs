using System;
using UniRx;
using Nodux.Core;
using UnityEngine;

namespace Nodux.PluginNode
{
    public class DiffTimerNode : RootNode
    {
        public override IDisposable Subscribe(IObserver<Any> observer)
        {
            return Observable.EveryUpdate().Select(it => new Any(Time.deltaTime)).Subscribe(observer);
        }
    }
}