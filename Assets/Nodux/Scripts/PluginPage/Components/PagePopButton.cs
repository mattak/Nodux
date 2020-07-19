using Nodux.PluginPage.Nodes;
using Nodux.PluginState;
using Nodux.PluginUI;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace Nodux.PluginPage.Components
{
    [RequireComponent(typeof(Button))]
    public class PagePopButton : MonoBehaviour
    {
        public StoreHolder StoreHolder;

        private void Start()
        {
            var buttonNode = new OnClickButtonNode(this.GetComponent<Button>());
            new PagePopNode(buttonNode, this.StoreHolder)
                .Subscribe(_ => { }, Debug.LogException)
                .AddTo(this);
        }
    }
}