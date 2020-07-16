using System;
using Nodux.Core;
using Nodux.PluginNode;
using Nodux.PluginPage.Reducers;
using Nodux.PluginState;
using UnityEngine.Serialization;

namespace Nodux.PluginPage.Nodes
{
    [Serializable]
    [TypeSelectionEnable("Node")]
    public class PageInitNode : Node
    {
        public StoreHolder StoreHolder;
        [FormerlySerializedAs("Config")] public PageDefinition definition;

        public PageInitNode(StoreHolder holder, PageDefinition definition) : base(null)
        {
            this.StoreHolder = holder;
            this.definition = definition;
        }

        public override IDisposable Subscribe(IObserver<Any> observer)
        {
            var reducer = new PageInitReducer();
            var configNode = new ValueNode(this.definition);
            var actionWriterNode =
                new StateActionWriterNode(configNode, "page", StoreHolder, reducer, new TypeSelection(reducer));
            return actionWriterNode.Subscribe(observer);
        }
    }
}