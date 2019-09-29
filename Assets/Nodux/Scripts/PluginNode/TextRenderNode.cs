using System;
using UnityEngine.UI;
using UniRx;
using UnityEngine;
using Nodux.Core;

namespace Nodux.PluginNode
{
    [Serializable]
    [TypeSelectionEnable("Node")]
    public class TextRenderNode : Node
    {
        [SerializeField] private Text Text;
        [SerializeField] private string Format;

        public TextRenderNode(INode parent, Text text, string format) : base(parent)
        {
            this.Text = text;
            this.Format = format;
        }

        public override IDisposable Subscribe(IObserver<Any> observer)
        {
            return this.Parent
                .Do(it =>
                {
                    this.Text.text = string.IsNullOrEmpty(this.Format)
                        ? it.Object.ToString()
                        : string.Format(this.Format, it.Object);
                }).Subscribe(observer);
        }
    }
}