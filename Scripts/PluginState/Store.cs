using System;
using System.Collections.Generic;
using Nodux.Core;

namespace Nodux.PluginState
{
    [Serializable]
    public class Store : IStore, IStoreAccessor
    {
        private State state = new State();

        public void Write(StateAction action)
        {
            state = action.Reducer.Reduce(state, action);
        }

        public IObservable<Any> Read(string key)
        {
            return state.GetObservable(key);
        }

        public bool HasKey(string key)
        {
            return state.GetRaw().ContainsKey(key);
        }

        public Any Get(string key)
        {
            return state.GetRaw()[key];
        }

        public IDictionary<string, Any> GetRawData()
        {
            return this.state.GetRaw();
        }

        public void Replace(IDictionary<string, Any> dictionary)
        {
            this.state.Replace(dictionary);
        }

        public void Notify(string key)
        {
            this.state.Notify(key, this.state.Get(key));
        }
    }
}