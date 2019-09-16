using System;
using UnityEngine;
using UniRx;

namespace UnityLeaf.PluginLeaf
{
    public class ChainNodeBehaviour : MonoBehaviour
    {
        public ChainNode ChainNode;
        private IDisposable disposable;

        public void Start()
        {
            Subscribe();
        }

        private void OnDestroy()
        {
            disposable?.Dispose();
        }

        [ContextMenu("Subscribe")]
        public void Subscribe()
        {
            disposable?.Dispose();
            disposable = this.ChainNode?.Subscribe(_ => { }, err => { }, () => { });
        }
    }
}