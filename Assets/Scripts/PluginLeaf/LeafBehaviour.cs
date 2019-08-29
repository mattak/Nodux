using System;
using UnityEngine;
using UnityLeaf.PluginNode;

namespace UnityLeaf.PluginLeaf
{
    public abstract class LeafBehaviour : MonoBehaviour, ILeaf
    {
        private IDisposable disposable;
        
        public void Start()
        {
            disposable = this.Subscribe();
        }

        private void OnDestroy()
        {
            disposable?.Dispose();
        }

        public abstract IDisposable Subscribe();

        [ContextMenu("ReSubscribe")]
        public void ReSubscribe()
        {
            disposable?.Dispose();
            disposable = this.Subscribe();
        }
    }
}