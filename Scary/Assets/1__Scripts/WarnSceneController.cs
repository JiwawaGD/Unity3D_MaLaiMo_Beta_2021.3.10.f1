using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class WarnSceneController : MonoBehaviour
{
    public bool bLoadSceneAsync;
    public int iNextSceneID;

    [SerializeField] Button Btn_confirmGame;

    AsyncOperation async = null;
    public GameObject TineLineAniObj;
    public Animation aniSubtitle_1;
    public Animation aniSubtitle_2;

    void Start()
    {

        if (Btn_confirmGame == null)
            Btn_confirmGame = GameObject.Find("Canvas/ConfirmGame").GetComponent<Button>();

        if (bLoadSceneAsync)
        {
            StartCoroutine(nameof(LoadSceneAsync));
            Btn_confirmGame.onClick.AddListener(() => AsyncActivate());
        }
        else
        {
            Btn_confirmGame.onClick.AddListener(() => LoadScene(iNextSceneID));
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
            yield return new WaitForEndOfFrame();
        }
    }

    void AsyncActivate()
    {
        async.allowSceneActivation = true;
    }
}