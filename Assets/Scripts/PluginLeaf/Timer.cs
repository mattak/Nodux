using System;
using UniRx;
using UnityLeaf.Core;
using UnityLeaf.PluginState;
using UnityLeaf.PluginNode;

namespace UnityLeaf.PluginLeaf
{
    public static class Timer
    {
        [Serializable]
        public struct Trigger : INode
        {
            public StoreHolder store;
            public string stateKey;
            public int maxTime;
            public INode Parent { get; set; }

            public IDisposable Subscribe(IObserver<Any> observer)
            {
                var timer = new DiffTimerNode();
                return timer.Aggregate((a, b) => new Any(a.Value<float>() + b.Value<float>())).Subscribe(observer);
//                throw new Exception("Not Implemented");
            }
        }
    }
}