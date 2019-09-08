using System;
using UnityEngine;
using UnityLeaf.Core;
using UnityLeaf.PluginNode;

namespace UnityLeaf.PluginLeaf
{
    [Serializable]
    [TypeSelectionEnable("Node")]
    public class SampleLeaf2 : Node
    {
        [SerializeField] private int IntValue;

        public SampleLeaf2(INode parent) : base(parent)
        {
        }
    }
}