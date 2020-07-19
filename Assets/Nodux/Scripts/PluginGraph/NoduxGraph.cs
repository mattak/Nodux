using System;
using System.Linq;
using UnityEngine;
using UniRx;

namespace Nodux.PluginGraph
{
    [Obsolete("Please use NoduxLinkGraph")]
    public class NoduxGraph : MonoBehaviour
    {
        public ChainNode ChainNode => _chainNodes.FirstOrDefault();
        public GraphContainer GraphContainer => _graphContainer;

        [SerializeField] private ChainNode[] _chainNodes;
        [SerializeField] private GraphContainer _graphContainer; // FIXME: [HideInInspector]

        private IDisposable _disposable;

        public void Start()
        {
            Subscribe();
        }

        private void OnDestroy()
        {
            _disposable?.Dispose();
        }

        [ContextMenu("Subscribe")]
        public void Subscribe()
        {
            _disposable?.Dispose();
            _disposable = this.ChainNode?.Subscribe(_ => { }, Debug.LogException);
        }

        public void UpdateEditData(GraphContainer container, ChainNode[] nodes)
        {
            this._graphContainer = container;
            this._chainNodes = nodes;
        }
    }
}