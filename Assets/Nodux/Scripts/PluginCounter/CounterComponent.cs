using Nodux.PluginState;
using UniRx;
using UnityEditor.Experimental;
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
            new CounterNode()
            {
                storeHolder = store,
                stateKey = "Clicks",
                text = text,
            }.Subscribe().AddTo(this);
        }
    }
}