using System;
using System.Collections.Generic;
using UniRx;
using UnityEditor;
using Nodux.Core;
using Nodux.PluginNode;
using UnityEngine.SceneManagement;

namespace Nodux.PluginScene
{
    [Serializable]
    [TypeSelectionEnable("Node")]
    public class ActiveSceneNode : Node
    {
        public ActiveSceneNode(INode parent) : base(parent)
        {
        }

        public override IDisposable Subscribe(IObserver<Any> observer)
        {
            return this.Parent.Select(_ => new Any(ListUp()))
                .Subscribe(observer);
        }

        private IDictionary<string, bool> ListUp()
        {
            var dictionary = new Dictionary<string, bool>();

            foreach (var editorScene in EditorBuildSettings.scenes)
            {
                var name = System.IO.Path.GetFileNameWithoutExtension(editorScene.path);
                dictionary[name] = false;
            }

            for (var i = 0; i < SceneManager.sceneCount; i++)
            {
                var scene = SceneManager.GetSceneAt(i);
                dictionary[scene.name] = scene.isLoaded;
            }

            return dictionary;
        }
    }
}