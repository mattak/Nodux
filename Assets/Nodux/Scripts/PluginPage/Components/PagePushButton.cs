using Nodux.PluginPage.Nodes;
using Nodux.PluginState;
using Nodux.PluginUI;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace Nodux.PluginPage.Components
{
    [RequireComponent(typeof(Button))]
    public class PagePushButton : MonoBehaviour
    {
        public string PageName;
        public StoreHolder StoreHolder;

        private void Start()
        {
            var buttonNode = new OnClickButtonNode(this.GetComponent<Button>());
            new PagePushNode(buttonNode, this.StoreHolder, new Page {Name = PageName})
                .Subscribe(_ => { }, Debug.LogException)
                .AddTo(this);
        }
    }
}