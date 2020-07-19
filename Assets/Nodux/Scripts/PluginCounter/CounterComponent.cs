using Nodux.PluginState;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace Nodux.PluginCounter
{
    public class CounterComponent : MonoBehaviour
    {
        public StoreHolder store;
        public Text text;

        void Start()
        {
            new CounterNode(null, store, "Clicks", text)
                .Subscribe(_ => { }, Debug.LogException)
                .AddTo(this);
        }
    }
}