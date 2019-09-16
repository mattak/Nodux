using System;
using Nodux.Core;
using Nodux.PluginNode;
using UnityEngine.UI;
using UniRx;

namespace Nodux.PluginSliderGauge
{
    [Serializable]
    [TypeSelectionEnable("Node")]
    public class SliderGaugeRenderNode : Node
    {
        public Slider Slider;

        public SliderGaugeRenderNode(INode parent) : base(parent)
        {
        }

        public override IDisposable Subscribe(IObserver<Any> observer)
        {
            return this.Parent.Subscribe(
                it =>
                {
                    var value = it.Value<float>();
                    value = value < 0f ? 0f : value;
                    value = value > 1f ? 1f : value;
                    this.Slider.value = value;
                    observer.OnNext(it);
                },
                error => observer.OnError(error),
                () => observer.OnCompleted());
        }
    }
}