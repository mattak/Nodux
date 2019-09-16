namespace Nodux.PluginState
{
    public class CombinationReducer : IReducer
    {
        public IReducer[] Reducers;

        public State Reduce(State state, StateAction action)
        {
            foreach (var reducer in Reducers)
            {
                state = reducer.Reduce(state, action);
            }

            return state;
        }
    }
}