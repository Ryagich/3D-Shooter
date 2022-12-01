using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    [SerializeField] private Image _bar;

    public void Load(string loadScene)
    {
        StartCoroutine(LoadAsync(loadScene));
    }

    private IEnumerator LoadAsync(string loadScene)
    {
        var asyngLoad = SceneManager.LoadSceneAsync(loadScene);
        while (!asyngLoad.isDone)
        {
            if (_bar)
                _bar.fillAmount = asyngLoad.progress / 0.95f;
            yield return new WaitForEndOfFrame();
        }
    }
}
