using System;
using Nodux.Core;
using Nodux.PluginNode;
using Nodux.PluginPage;
using Nodux.PluginState;
using UniRx;

namespace Example.MultiScene
{
    [Serializable]
    [TypeSelectionEnable("Node")]
    public class SampleHeaderTitleReadNode : Node
    {
        public StoreHolder StoreHolder;

        public SampleHeaderTitleReadNode(INode parent, StoreHolder holder) : base(parent)
        {
            this.StoreHolder = holder;
        }

        public override IDisposable Subscribe(IObserver<Any> observer)
        {
            var data = new SamplePageData();
            var pageStackNode = new StateReaderNode(this.StoreHolder, PageConst.StateStackKey);

            return pageStackNode
                .Select(it => it.Value<PageStack>().Peek().GetData<SamplePageData>())
                .Select(it => it != null ? it.Title : "Header")
                .Select(x => new Any(x))
                .Subscribe(observer);
        }
    }
}
