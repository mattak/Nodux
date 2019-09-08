using System;
using UnityEngine;
using UnityEngine.UI;
using UnityLeaf.Core;
using UnityLeaf.PluginNode;

namespace UnityLeaf.PluginLeaf
{
    [TypeSelectionEnable("Node")]
    [Serializable]
    public class SampleLeaf : Node
    {
        [SerializeField] private string StateKey;
        [SerializeField] private Button Button;
        [SerializeField] private Text Text;

        public SampleLeaf(INode parent) : base(parent)
        {
        }
    }
}