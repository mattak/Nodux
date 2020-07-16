using Nodux.PluginPage.Nodes;
using Nodux.PluginState;
using UniRx;
using UnityEngine;

namespace Nodux.PluginPage.Components
{
    public class PageInitComponent : MonoBehaviour
    {
        public StoreHolder StoreHolder;
        public PageSetting PageSetting;

        private void Start()
        {
            SetUpPageInit();
        }

        private void SetUpPageInit()
        {
            new PageInitNode(this.StoreHolder, this.PageSetting.Definition)
                .Subscribe(_ => { }, Debug.LogException)
                .AddTo(this);
        }
    }
}