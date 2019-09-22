using System;
using Nodux.Core;
using Nodux.PluginNode;
using UniRx;
using UnityEngine;

namespace Nodux.PluginSpawner
{
    [Serializable]
    [TypeSelectionEnable("Node")]
    public class PrefabInstantiateNode : Node
    {
        [SerializeField] private GameObject Prefab;
        [SerializeField] private Transform ParentTransform;

        public PrefabInstantiateNode(INode parent) : base(parent)
        {
        }

        public override IDisposable Subscribe(IObserver<Any> observer)
        {
            return this.Parent.Select(_ =>
            {
                var obj = GameObject.Instantiate(this.Prefab, this.ParentTransform, false);
                return new Any(obj);
            }).Subscribe(observer);
        }
    }
}