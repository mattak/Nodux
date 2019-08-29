using System;

namespace UnityLeaf.PluginNode
{
    public interface ILeaf
    {
        IDisposable Subscribe();
    }
}