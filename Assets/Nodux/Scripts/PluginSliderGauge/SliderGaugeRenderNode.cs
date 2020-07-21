using System;
using Nodux.Core;
using Nodux.PluginNode;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace Nodux.PluginSliderGauge
{
    [Serializable]
    [TypeSelectionEnable("Node")]
    public class SliderGaugeRenderNode : Node
    {
        [SerializeField] private Slider slider;

        public SliderGaugeRenderNode(INode parent, Slider slider) : base(parent)
        {
            this.slider = slider;
        }

        public override IDisposable Subscribe(IObserver<Any> observer)
        {
            return this.Parent.Subscribe(
                it =>
                {
                    var value = it.Value<float>();
                    value = value < 0f ? 0f : value;
                    value = value > 1f ? 1f : value;
                    this.slider.value = value;
                    observer.OnNext(it);
                },
                error => observer.OnError(error),
                () => observer.OnCompleted());
        }
    }
}