using System;
using UnityEngine;

namespace Nodux.PluginPage
{
    [Serializable]
    [CreateAssetMenu(fileName = "PageSetting", menuName = "Nodux/Page/PageSetting", order = 100)]
    public class PageSetting : ScriptableObject
    {
        public string[] PermanentScenes;
        public PageDefinition Definition;
    }
}