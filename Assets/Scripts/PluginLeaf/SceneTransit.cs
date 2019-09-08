using System;
using UnityEngine;
using UnityEngine.UI;
using UnityLeaf.Core;
using UnityLeaf.PluginNode;
using UnityLeaf.PluginState;
using UnityLeaf.PluginLeaf;
using UnityLeaf.PluginScene;

namespace UnityLeaf.PluginLeaf
{
    public static class SceneTransit
    {
        [Serializable]
        public struct Render : INode
        {
            public StoreHolder storeHolder;
            public MonoBehaviour behaviour;
            public INode Parent { get; set; }

            public IDisposable Subscribe(IObserver<Any> observer)
            {
                var stateReaderNode = new StateReaderNode(storeHolder, "scene");
                var sceneWriterNode = new SceneWriter(stateReaderNode, behaviour);
                return sceneWriterNode.Subscribe(observer);
            }
        }

        [Serializable]
        public struct ButtonTransit : INode
        {
            public StoreHolder storeHolder;
            public Button button;
            public string sceneName;
            public INode Parent { get; set; }

            public IDisposable Subscribe(IObserver<Any> observer)
            {
                var buttonNode = new ButtonNode(button);
                var stringNode = new StringNode(buttonNode, sceneName);
                var stateActionNode = new StateActionNode(stringNode, "scene", new SceneAddReducer());
                var stateWriterNode = new StateWriterNode(stateActionNode, storeHolder);
                return stateWriterNode.Subscribe(observer);
            }
        }

        [Serializable]
        public struct Sync : INode
        {
            public StoreHolder storeHolder;
            public OnStartTriggerBehaviour component;
            public INode Parent { get; set; }

            public IDisposable Subscribe(IObserver<Any> observer)
            {
                var onStartNode = new OnStartNode(component);
                var stringNode = new ActiveSceneNode(onStartNode);
                var stateActionNode = new StateActionNode(stringNode, "scene", new SceneSyncReducer());
                var stateWriterNode = new StateWriterNode(stateActionNode, storeHolder);
                return stateWriterNode.Subscribe(observer);
            }
        }
    }
}