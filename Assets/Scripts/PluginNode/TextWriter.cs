using System;
using UnityEngine.UI;
using UniRx;

namespace UnityLeaf.PluginNode
{
    public class TextWriter : LeafNode
    {
        private Text text;
        private string format;

        public TextWriter(INode parent, Text text, string format) : base(parent)
        {
            this.text = text;
            this.format = format;
        }

        public override IDisposable Subscribe()
        {
            return this.GetParent().GetObservable().Subscribe(it =>
                {
                    if (string.IsNullOrEmpty(this.format))
                    {
                        this.text.text = it.Object.ToString();
                    }
                    else
                    {
                        this.text.text = string.Format(this.format, it.Object?.ToString());
                    }
                }
            );
        }
    }
}