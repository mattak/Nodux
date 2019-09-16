using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Nodux.Core;
using Nodux.PluginNode;
using UnityEngine;
using UnityEngine.SceneManagement;
using UniRx;

namespace Nodux.PluginScene
{
    [Serializable]
    [TypeSelectionEnable("Node")]
    public class SceneWriteNode : Node
    {
        [SerializeField] private MonoBehaviour component;

        public SceneWriteNode(INode parent, MonoBehaviour component) : base(parent)
        {
            this.component = component;
        }

        public override IDisposable Subscribe(IObserver<Any> observer)
        {
            return this.Parent.Subscribe(
                it =>
                {
                    this.component.StartCoroutine(this.Render(it));
                    observer.OnNext(it);
                },
                it => observer.OnError(it),
                () => observer.OnCompleted()
            );
        }

        public IEnumerator Render(Any value)
        {
            if (!value.Is<IDictionary<string, bool>>()) yield break;

            var expectMap = value.Value<IDictionary<string, bool>>();
            var currentMap = this.GetCurrentMap();
            var additionalScenes = new HashSet<string>();
            var removalScenes = new HashSet<string>();

            foreach (var key in currentMap.Keys.ToList())
            {
                if (currentMap[key] && (!expectMap.ContainsKey(key) || !expectMap[key]))
                {
                    removalScenes.Add(key);
                }
            }

            foreach (var key in expectMap.Keys.ToList())
            {
                if (expectMap[key] && (!currentMap.ContainsKey(key) || !currentMap[key]))
                {
                    additionalScenes.Add(key);
                }
            }

            foreach (var scene in additionalScenes)
            {
                yield return SceneManager.LoadSceneAsync(scene, LoadSceneMode.Additive);
            }

            foreach (var scene in removalScenes)
            {
                yield return SceneManager.UnloadSceneAsync(scene);
            }
        }

        private IDictionary<string, bool> GetCurrentMap()
        {
            var currentMap = new Dictionary<string, bool>();

            for (var i = 0; i < SceneManager.sceneCount; i++)
            {
                var scene = SceneManager.GetSceneAt(i);
                currentMap[scene.name] = scene.isLoaded;
            }

            return currentMap;
        }
    }
}