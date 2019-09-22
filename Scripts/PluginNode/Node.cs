using System;
using UniRx;
using UnityEngine;
using Nodux.Core;

namespace Nodux.PluginNode
{
    [Serializable]
    public class Node : INode
    {
        [SerializeField] private INode parent = null;

        public INode Parent
        {
            get { return parent; }
            set { parent = value; }
        }

        public Node(INode parent)
        {
            this.Parent = parent;
        }

        public virtual IDisposable Subscribe(IObserver<Any> observer)
        {
            return this.Parent?.Subscribe() ?? Disposable.Empty;
        }
    }
}