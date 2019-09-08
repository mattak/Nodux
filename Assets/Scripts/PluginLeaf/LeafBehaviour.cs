using System;
using UnityEngine;
using UnityLeaf.Core;
using UnityLeaf.PluginNode;

namespace UnityLeaf.PluginLeaf
{
    public abstract class LeafBehaviour : MonoBehaviour, INode
    {
        public INode Parent { get; set; }
        private IDisposable disposable;
        public IDisposable Subscribe(IObserver<Any> observer)
        {
            throw new NotImplementedException();
        }
        
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