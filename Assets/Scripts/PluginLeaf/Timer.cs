using System;
using UnityLeaf.PluginState;
using UnityLeaf.PluginNode;

namespace UnityLeaf.PluginLeaf
{
    public static class Timer
    {
        [Serializable]
        public struct Trigger : ILeaf
        {
            public StoreHolder store;
            public string stateKey;
            public int maxTime;

            public IDisposable Subscribe()
            {
                var timer = new DiffTimerNode();
                throw new Exception("Not Implemented");
            }
        }
    }
}