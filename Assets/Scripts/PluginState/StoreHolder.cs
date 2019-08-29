using UnityEngine;

namespace UnityLeaf.PluginState
{
    public class StoreHolder : MonoBehaviour
    {
        private static IStore store = default(Store);

        public IStore GetStore()
        {
            return store ?? (store = new Store());
        }

        public IStoreAccessor GetStoreAccessor()
        {
            return store as IStoreAccessor;
        }
    }
}