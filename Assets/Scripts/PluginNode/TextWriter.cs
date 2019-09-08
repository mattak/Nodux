using System;
using UnityEngine.UI;
using UniRx;
using UnityEngine;
using UnityLeaf.Core;

namespace UnityLeaf.PluginNode
{
    [Serializable]
    [TypeSelectionEnable("Node")]
    public class TextWriter : Node
    {
        [SerializeField] private Text text;
        [SerializeField] private string format;

        public TextWriter(INode parent, Text text, string format) : base(parent)
        {
            this.text = text;
            this.format = format;
        }

        public override IDisposable Subscribe(IObserver<Any> observer)
        {
            return this.Parent.Subscribe(it =>
                {
                    if (string.IsNullOrEmpty(this.format))
                    {
                        this.text.text = it.Object.ToString();
                    }
                    else
                    {
                        this.text.text = string.Format(this.format, it.Object?.ToString());
                    }

                    observer.OnNext(it);
                },
                err => observer.OnError(err),
                () => observer.OnCompleted()
            );
        }
    }
}