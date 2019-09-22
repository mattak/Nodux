using UnityEngine;

namespace Nodux.PluginState
{
    public class SingleStore : MonoBehaviour
    {
        public static SingleStore Instance
        {
            get
            {
                // NOTE: DO not create GameObject on edit mode
                if (!Application.isPlaying) return null;

                if (instance == null)
                {
                    var obj = new GameObject("SingleStore");
                    instance = obj.AddComponent<SingleStore>();
                    DontDestroyOnLoad(obj);
                }

                return instance;
            }
        }

        private static SingleStore instance = null;
        private IStore store = default(Store);

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