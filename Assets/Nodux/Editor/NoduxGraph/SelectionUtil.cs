using System.Collections.Generic;
using UnityEditor;
using UnityEngine.SceneManagement;

namespace Nodux.PluginEditor.NoduxGraph
{
    public static class SelectionUtil
    {
        public static string GetSceneGameObjectPath()
        {
            var scene = SceneManager.GetActiveScene().name;
            var list = new List<string>();
            list.Add(scene);
            list.AddRange(GetGameObjectPath());
            return string.Join(">", list);
        }

        public static IEnumerable<string> GetGameObjectPath()
        {
            var current = Selection.activeTransform;
            if (current == null) return new string[0];

            var root = current.root;
            var names = new List<string>();

            while (current != null)
            {
                names.Add(current.gameObject.name);
                current = current.parent;
            }

            names.Reverse();
            return names;
        }
    }
}