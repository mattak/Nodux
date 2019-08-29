using System;
using UnityEngine;
using UnityEngine.UI;
using UnityLeaf.PluginNode;
using UnityLeaf.PluginState;
using UnityLeaf.PluginLeaf;
using UnityLeaf.PluginScene;

namespace UnityLeaf.PluginLeaf
{
    public static class SceneTransit
    {
        [Serializable]
        public struct Render : ILeaf
        {
            public StoreHolder storeHolder;
            public MonoBehaviour behaviour;

            public IDisposable Subscribe()
            {
                var stateReaderNode = new StateReaderNode(storeHolder, "scene");
                var sceneWriterNode = new SceneWriter(stateReaderNode, behaviour);
                return sceneWriterNode.Subscribe();
            }
        }

        [Serializable]
        public struct ButtonTransit : ILeaf
        {
            public StoreHolder storeHolder;
            public Button button;
            public string sceneName;

            public IDisposable Subscribe()
            {
                var buttonNode = new ButtonNode(button);
                var stringNode = new StringNode(buttonNode, sceneName);
                var stateActionNode = new StateActionNode(stringNode, "scene", new SceneAddReducer());
                var stateWriterNode = new StateWriter(stateActionNode, storeHolder);
                return stateWriterNode.Subscribe();
            }
        }

        [Serializable]
        public struct Sync : ILeaf
        {
            public StoreHolder storeHolder;
            public OnStartTriggerBehaviour component;

            public IDisposable Subscribe()
            {
                var onStartNode = new OnStartNode(component);
                var stringNode = new ActiveSceneNode(onStartNode);
                var stateActionNode = new StateActionNode(stringNode, "scene", new SceneSyncReducer());
                var stateWriterNode = new StateWriter(stateActionNode, storeHolder);
                return stateWriterNode.Subscribe();
            }
        }
    }
}