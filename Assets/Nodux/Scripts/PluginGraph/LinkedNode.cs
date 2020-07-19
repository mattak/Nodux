using System;
using System.Collections.Generic;
using System.Linq;
using Nodux.Core;
using Nodux.PluginNode;
using UniRx;
using UnityEngine;

namespace Nodux.PluginGraph
{
    [Serializable]
    public class LinkedNode : Node
    {
        [SerializeReference] [SerializeField] private INode[] Nodes;

        public LinkedNode(INode parent, IEnumerable<INode> nodes) : base(parent)
        {
            this.Nodes = nodes.ToArray();
        }

        public override IDisposable Subscribe(IObserver<Any> observer)
        {
            Connect();
            if (this.Parent == null) return this.Nodes.Last().Subscribe(observer);
            return this.Parent.SelectMany(it => this.Nodes.Last()).Subscribe(observer);
        }

        private void Connect()
        {
            var previousParent = this.Parent;
            foreach (var node in this.Nodes)
            {
                node.Parent = previousParent;
                previousParent = node;
            }
        }
    }
}