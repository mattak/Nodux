using System;
using UniRx;
using UnityLeaf.Core;
using UnityEngine;

namespace UnityLeaf.PluginNode
{
    public class DiffTimerNode : RootNode
    {
        public override IObservable<Any> GetObservable()
        {
            return Observable.EveryUpdate().Select(it => new Any(Time.deltaTime));
        }
    }
}