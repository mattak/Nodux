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
        [SerializeField] private GameObject Prefab = default;
        [SerializeField] private Transform ParentTransform = default;

        public PrefabInstantiateNode(INode parent, GameObject prefab, Transform parentTransform) : base(parent)
        {
            this.Prefab = prefab;
            this.ParentTransform = parentTransform;
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