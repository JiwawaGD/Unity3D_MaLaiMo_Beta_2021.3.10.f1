using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    readonly string s_nextScene = "2_GrandmaHouse";

    AsyncOperation async = null;

    void Start()
    {
        StartCoroutine(nameof(LoadScene));
    }

    public void StartBtn()
    {
        if (async.progress < 0.9f)
            return;

        async.allowSceneActivation = true;
    }

    IEnumerator LoadScene()
    {
        async = SceneManager.LoadSceneAsync(s_nextScene);
        async.allowSceneActivation = false;

        float _prograssValue;

        while (!async.isDone)
        {
            _prograssValue = async.progress;

            if (_prograssValue >= 0.9f)
            {
                Debug.Log($"PreLoad scene {s_nextScene} complete");
                break;
            }
        }

        yield return null;
    }
}
