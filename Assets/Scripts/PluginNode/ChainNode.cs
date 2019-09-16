using System;
using System.Collections.Generic;
using System.Linq;
using UniRx;
using UnityEngine;
using Nodux.Core;

namespace Nodux.PluginNode
{
    [Serializable]
    public class ChainNode : Node
    {
        // NOTE: this fields hold unity objects reference including file id which is used on NodeList.
        [SerializeField] private List<UnityEngine.Object> UnityObjectList = new List<UnityEngine.Object>();

        [TypeSelectionFilter("Node")] [SerializeField]
        private List<TypeSelection> NodeList = new List<TypeSelection>();

        private IList<INode> List = new List<INode>();

        public ChainNode(INode parent) : base(parent)
        {
        }

        public override IDisposable Subscribe(IObserver<Any> observer)
        {
            if (!this.ConnectNodes()) return Disposable.Empty;

            if (this.List.Count < 1)
            {
                UnityEngine.Debug.LogWarning("Node map is empty");
                return Disposable.Empty;
            }

            if (!this.ConnectNodes())
            {
                UnityEngine.Debug.LogWarning("Node connection failure");
                return Disposable.Empty;
            }

            if (this.Parent == null) return this.List.Last().Subscribe(observer);
            return this.Parent.SelectMany(it => this.List.Last()).Subscribe(observer);
        }

        public bool ConnectNodes()
        {
            this.List = new List<INode>();

            for (var i = 0; i < this.NodeList.Count; i++)
            {
                if (!this.NodeList[i].Restore())
                {
                    UnityEngine.Debug.LogWarning($"Node[{i}] cannot be restored");
                    return false;
                }

                this.List.Add(this.NodeList[i].Get<INode>());
            }

            for (var i = 0; i < this.List.Count - 1; i++)
            {
                this.List[i + 1].Parent = this.List[i];
            }

            return true;
        }
    }
}