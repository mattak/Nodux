using UnityEngine;

namespace Nodux.PluginState
{
    public class StoreHolder : MonoBehaviour
    {
        public IStore GetStore()
        {
            return SingleStore.Instance.GetStore();
        }

        public IStoreAccessor GetStoreAccessor()
        {
            return SingleStore.Instance?.GetStoreAccessor();
        }
    }
}