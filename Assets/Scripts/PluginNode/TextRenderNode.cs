using System;
using UnityEngine.UI;
using UniRx;
using UnityEngine;
using UnityLeaf.Core;

namespace UnityLeaf.PluginNode
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
            return this.Parent.Subscribe(it =>
                {
                    if (string.IsNullOrEmpty(this.Format))
                    {
                        this.Text.text = it.Object.ToString();
                    }
                    else
                    {
                        this.Text.text = string.Format(this.Format, it.Object?.ToString());
                    }

                    observer.OnNext(it);
                },
                err => observer.OnError(err),
                () => observer.OnCompleted()
            );
        }
    }
}