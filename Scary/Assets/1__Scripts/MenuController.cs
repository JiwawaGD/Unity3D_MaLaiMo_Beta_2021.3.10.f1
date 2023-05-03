using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuController : MonoBehaviour
{
    public bool bLoadSceneAsync;
    public int iNextSceneID;

    Button Btn_StartGame;
    AsyncOperation async = null;
    public Animation aniSubtitle_1;
    public Animation aniSubtitle_2;

    void Start()
    {
        if (Btn_StartGame == null)
            Btn_StartGame = GameObject.Find("Canvas/EnterGame").GetComponent<Button>();

        if (bLoadSceneAsync)
        {
            //StartCoroutine(nameof(LoadSceneAsync));
            //Btn_StartGame.onClick.AddListener(() => AsyncActivate());

            Btn_StartGame.onClick.AddListener(() => LoadScene(iNextSceneID));
        }
        else
        {
            Btn_StartGame.onClick.AddListener(() => LoadScene(iNextSceneID));
        }
    }

    void LoadScene(int r_iSceneIndex)
    {
        SceneManager.LoadScene(r_iSceneIndex);
    }

    IEnumerator LoadSceneAsync()
    {
        async = SceneManager.LoadSceneAsync(iNextSceneID);
        async.allowSceneActivation = false;

        while (async.progress < 0.9f)
        {
            if (async.progress > 0.3f)
            {
                aniSubtitle_1.Play();
            }

            yield return new WaitForEndOfFrame();
        }

        Debug.Log("Load Scene Finish");
    }

    void AsyncActivate()
    {
        Debug.Log("1233333");
        async.allowSceneActivation = true;
    }
}