using System;
using System.Collections.Generic;
using System.Linq;
using UniRx;
using UnityEngine;
using Nodux.Core;
using Nodux.PluginNode;

namespace Nodux.PluginGraph
{
    [Obsolete("Please use LinkedNode")]
    [Serializable]
    public class ChainNode : Node
    {
        [TypeSelectionFilter("Node")] [SerializeField]
        private List<TypeSelection> NodeList = new List<TypeSelection>();

        private IList<INode> ConnectionList = new List<INode>();

        public ChainNode(INode parent) : base(parent)
        {
        }

        public ChainNode(INode parent, IEnumerable<TypeSelection> selections) : base(parent)
        {
            this.NodeList = selections.ToList();
        }

        public override IDisposable Subscribe(IObserver<Any> observer)
        {
            if (!this.ConnectNodes()) return Disposable.Empty;

            if (this.ConnectionList.Count < 1)
            {
                UnityEngine.Debug.LogWarning("Node map is empty");
                return Disposable.Empty;
            }

            if (!this.ConnectNodes())
            {
                UnityEngine.Debug.LogWarning("Node connection failure");
                return Disposable.Empty;
            }

            if (this.Parent == null) return this.ConnectionList.Last().Subscribe(observer);
            return this.Parent.SelectMany(it => this.ConnectionList.Last()).Subscribe(observer);
        }

        public bool ConnectNodes()
        {
            this.ConnectionList = new List<INode>();

            for (var i = 0; i < this.NodeList.Count; i++)
            {
                if (!this.NodeList[i].Restore())
                {
                    UnityEngine.Debug.LogWarning($"Node[{i}] cannot be restored");
                    return false;
                }

                this.ConnectionList.Add(this.NodeList[i].Get<INode>());
            }

            for (var i = 0; i < this.ConnectionList.Count - 1; i++)
            {
                this.ConnectionList[i + 1].Parent = this.ConnectionList[i];
            }

            return true;
        }
    }
}