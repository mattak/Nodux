using System;
using Nodux.Core;
using Nodux.PluginNode;
using Nodux.PluginPage.Reducers;
using Nodux.PluginState;

namespace Nodux.PluginPage.Nodes
{
    [Serializable]
    [TypeSelectionEnable("Node")]
    public class PageReplaceNode : Node
    {
        public StoreHolder StoreHolder;
        public Page Page;

        public PageReplaceNode(INode parent, StoreHolder holder, Page page) : base(parent)
        {
            this.StoreHolder = holder;
            this.Page = page;
        }

        public override IDisposable Subscribe(IObserver<Any> observer)
        {
            var reducer = new PageReplaceReducer();
            var valueNode = new ValueNode(this.Parent, this.Page);
            var stateActionWriterNode = new StateActionWriterNode(
                valueNode, "page", StoreHolder, reducer
            );
            return stateActionWriterNode.Subscribe(observer);
        }
    }
}