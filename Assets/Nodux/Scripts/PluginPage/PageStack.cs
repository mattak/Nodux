using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Nodux.PluginPage
{
    [Serializable]
    public class PageStack
    {
        [SerializeField] private List<Page> Pages = new List<Page>();

        public int Count => this.Pages.Count;
        public bool IsEmpty => Count < 1;

        public void Push(Page page)
        {
            this.Pages.Add(page);
        }

        public Page Peek()
        {
            return Pages?.Count > 0 ? this.Pages[this.Pages.Count - 1] : null;
        }

        public Page Pop()
        {
            if (Pages == default || Pages.Count < 1) return null;
            var lastIndex = this.Pages.Count - 1;
            var page = this.Pages[lastIndex];
            this.Pages.RemoveAt(lastIndex);
            return page;
        }
    }
}