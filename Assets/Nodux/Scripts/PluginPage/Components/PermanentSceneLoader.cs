using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Nodux.PluginPage.Components
{
    public class PermanentSceneLoader : MonoBehaviour
    {
        public PageSetting PageSetting;

        void Start()
        {
            if (PageSetting == null)
            {
                Debug.LogWarning("Missing PageSettings");
                return;
            }

            StartCoroutine(LoadMissingPermanentScenes(PageSetting.PermanentScenes));
        }

        private IEnumerator LoadMissingPermanentScenes(string[] permanentScenes)
        {
            var loadedScenes = new Dictionary<string, bool>();
            for (var i = 0; i < SceneManager.sceneCount; i++)
            {
                loadedScenes[SceneManager.GetSceneAt(i).name] = true;
            }

            foreach (var permanentScene in permanentScenes)
            {
                if (loadedScenes.ContainsKey(permanentScene)) continue;

                yield return SceneManager.LoadSceneAsync(permanentScene, LoadSceneMode.Additive);
            }
        }
    }
}