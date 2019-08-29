using System;
using System.Collections.Generic;
using UniRx;
using UnityLeaf.Core;
using UnityEngine.SceneManagement;

namespace UnityLeaf.PluginNode
{
    public class ActiveSceneNode : Node
    {
        public ActiveSceneNode(INode parent) : base(parent)
        {
        }

        public override IObservable<Any> GetObservable()
        {
            return this.GetParent().GetObservable().SelectMany(it => Observable.Return(new Any(ListUp())));
        }

        private IDictionary<string, bool> ListUp()
        {
            var dictionary = new Dictionary<string, bool>();
            for (var i = 0; i < SceneManager.sceneCount; i++)
            {
                var scene = SceneManager.GetSceneAt(i);
                dictionary[scene.name] = scene.isLoaded;
            }

            return dictionary;
        }
    }
}