using System;
using System.Collections.Generic;
using Nodux.Core;
using Nodux.PluginNode;
using UniRx;
using UnityEngine.SceneManagement;

namespace Nodux.PluginScene.Nodes
{
    [Serializable]
    [TypeSelectionEnable("Node")]
    public class LoadListSceneNode : Node
    {
        public LoadListSceneNode(INode parent) : base(parent)
        {
        }

        public override IDisposable Subscribe(IObserver<Any> observer)
        {
            return this.Parent.Select(_ => new Any(ListUp()))
                .Subscribe(observer);
        }

        private IList<string> ListUp()
        {
            var list = new List<string>();

            for (var i = 0; i < SceneManager.sceneCount; i++)
            {
                var scene = SceneManager.GetSceneAt(i);
                list.Add(scene.name);
            }

            return list;
        }
    }
}