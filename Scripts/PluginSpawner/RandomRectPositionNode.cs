using System;
using Nodux.Core;
using Nodux.PluginNode;
using UniRx;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Nodux.PluginSpawner
{
    [Serializable]
    [TypeSelectionEnable("Node")]
    public class RandomRectPositionNode : Node
    {
        [SerializeField] private RectTransform RandomArea = null;

        public RandomRectPositionNode(INode parent) : base(parent)
        {
        }

        public override IDisposable Subscribe(IObserver<Any> observer)
        {
            return this.Parent.Select(it =>
            {
                if (!it.Is<GameObject>()) return it;
                var gameObject = it.Value<GameObject>();

                var rectTransform = gameObject.GetComponent<RectTransform>();
                if (rectTransform == null) return it;

                var rect = this.RandomArea.rect;
                var x = (rect.xMax - rect.xMin) * Random.value + rect.xMin;
                var y = (rect.yMax - rect.yMin) * Random.value + rect.yMin;

                rectTransform.anchoredPosition = new Vector2(x, y);
                return it;
            }).Subscribe(observer);
        }
    }
}