using System;
using Nodux.Core;
using Nodux.PluginNode;
using Nodux.PluginState;
using UniRx;

namespace Nodux.PluginGraph
{
    [Serializable]
    public class ReducerSelectionSample : Node
    {
        [TypeSelectionFilter("Reducer")] public IReducer Reducer;

        public ReducerSelectionSample(INode parent) : base(parent)
        {
        }

        public override IDisposable Subscribe(IObserver<Any> observer)
        {
            return Observable.ReturnUnit().Select(x => new Any(x)).Subscribe(observer);
        }
    }
}