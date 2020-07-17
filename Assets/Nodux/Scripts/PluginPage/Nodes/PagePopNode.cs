using System;
using Nodux.Core;
using Nodux.PluginNode;
using Nodux.PluginPage.Reducers;
using Nodux.PluginState;

namespace Nodux.PluginPage.Nodes
{
    [Serializable]
    [TypeSelectionEnable("Node")]
    public class PagePopNode : Node
    {
        public StoreHolder StoreHolder;

        public PagePopNode(INode parent, StoreHolder holder) : base(parent)
        {
            this.StoreHolder = holder;
        }

        public override IDisposable Subscribe(IObserver<Any> observer)
        {
            var reducer = new PagePopReducer();
            var valueNode = new UnitNode(this.Parent);
            var stateActionWriterNode = new StateActionWriterNode(
                valueNode, "page", StoreHolder, reducer
            );
            return stateActionWriterNode.Subscribe(observer);
        }
    }
}