using System;
using UniRx;
using UnityLeaf.Core;
using UnityEngine;

namespace UnityLeaf.PluginNode
{
    public class LeafNode : ILeaf, INode
    {
        private INode parent;

        public LeafNode(INode parent)
        {
            this.parent = parent;
        }

        public INode GetParent()
        {
            return this.parent;
        }

        public virtual IObservable<Any> GetObservable()
        {
            return this.GetParent().GetObservable();
        }

        public virtual IDisposable Subscribe()
        {
            return this.GetObservable().Subscribe(_ => { });
        }
    }
}