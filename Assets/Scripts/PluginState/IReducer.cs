namespace UnityLeaf.PluginState
{
    public interface IReducer
    {
        State Reduce(State state, StateAction action);
    }
}