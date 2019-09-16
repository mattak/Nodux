using System;
using UnityEngine;
using UniRx;

namespace UnityLeaf.PluginLeaf
{
    public class NodeBehaviour : MonoBehaviour
    {
        public ChainNode ChainNode;
        private IDisposable disposable;

        public void Start()
        {
            ReSubscribe();
        }

        private void OnDestroy()
        {
            disposable?.Dispose();
        }

        [ContextMenu("ReSubscribe")]
        public void ReSubscribe()
        {
            disposable?.Dispose();
            disposable = this.ChainNode?.Subscribe(_ => { }, err => { }, () => { });
        }
    }
}