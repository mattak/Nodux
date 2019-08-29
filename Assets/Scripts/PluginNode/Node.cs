using System;
using UnityLeaf.Core;

namespace UnityLeaf.PluginNode
{
    public class Node : INode
    {
        private INode parent;

        public Node(INode parent)
        {
            this.parent = parent;
        }

        public INode GetParent()
        {
            return this.parent;
        }

        public virtual IObservable<Any> GetObservable()
        {
            return this.parent.GetObservable();
        }
    }
}