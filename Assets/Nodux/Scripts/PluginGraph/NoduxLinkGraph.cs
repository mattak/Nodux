using System;
using Nodux.Core;
using Nodux.PluginNode;
using UniRx;
using UnityEngine;

namespace Nodux.PluginGraph
{
    public class NoduxLinkGraph : MonoBehaviour, INode
    {
        public GraphContainer GraphContainer => _graphContainer;
        public INode Parent { get; set; } = null;

        [SerializeReference, SerializeField]
        private INode[] _nodes;

        [SerializeField] private GraphContainer _graphContainer;

        private CompositeDisposable _disposable = new CompositeDisposable();

        public void Start()
        {
            Subscribe();
        }

        private void OnDestroy()
        {
            _disposable?.Dispose();
        }

        [ContextMenu("Subscribe")]
        public IDisposable Subscribe()
        {
            return this.Subscribe(new EmptyAnyObserver());
        }

        public void UpdateEditData(GraphContainer container, INode[] nodes)
        {
            this._graphContainer = container;
            this._nodes = nodes;
        }

        public IDisposable Subscribe(IObserver<Any> observer)
        {
            _disposable?.Clear();
            _disposable = new CompositeDisposable();

            foreach (var node in _nodes)
            {
                if (this.Parent == null) _disposable.Add(node.Subscribe(observer));
                else _disposable.Add(this.Parent.SelectMany(_ => node).Subscribe()); // XXX: may contains bug
            }

            return _disposable;
        }

        private class EmptyAnyObserver : IObserver<Any>
        {
            public void OnCompleted()
            {
            }

            public void OnError(Exception error)
            {
                Debug.LogException(error);
            }

            public void OnNext(Any value)
            {
            }
        }
    }
}