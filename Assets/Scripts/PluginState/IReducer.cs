namespace Nodux.PluginState
{
    public interface IReducer
    {
        State Reduce(State state, StateAction action);
    }
}