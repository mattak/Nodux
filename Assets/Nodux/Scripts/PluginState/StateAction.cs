using Nodux.Core;

namespace Nodux.PluginState
{
    public struct StateAction
    {
        public IReducer Reducer;
        public string StateKey;
        public Any Value;

        public StateAction(IReducer reducer, string stateKey, Any value)
        {
            this.Reducer = reducer;
            this.StateKey = stateKey;
            this.Value = value;
        }

        public TValue GetValue<TValue>() => this.Value.Value<TValue>();

        public bool IsValueOf<TValue>() => this.Value.Is<TValue>();

        public override string ToString()
        {
            return $"StateAction(Reducer:{Reducer}, StateKey:{StateKey}, Value:{Value})";
        }
    }
}