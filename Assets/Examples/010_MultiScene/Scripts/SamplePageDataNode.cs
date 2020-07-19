using System;
using Nodux.Core;
using Nodux.PluginNode;
using Nodux.PluginPage;
using UniRx;

namespace Example.MultiScene
{
    [Serializable]
    [TypeSelectionEnable("Node")]
    public class SamplePageDataNode : Node
    {
        public string PageName;
        public SamplePageData PageData;

        public SamplePageDataNode(INode parent, string pageName, SamplePageData data) : base(parent)
        {
            this.PageName = pageName;
            this.PageData = data;
        }

        public override IDisposable Subscribe(IObserver<Any> observer)
        {
            return this.Parent
                .Select(x => new Page {Name = this.PageName, Data = this.PageData})
                .Select(x => new Any(x))
                .Subscribe(observer);
        }
    }
}