using UnityLeaf.Core;

namespace UnityLeaf.PluginState
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

        public override string ToString()
        {
            return $"StateAction(Reducer:{Reducer}, StateKey:{StateKey}, Value:{Value})";
        }
    }
}