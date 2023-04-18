using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuController : MonoBehaviour
{
    Button Btn_StartGame;
    int iFirstSceneIndex = 1;

    void Start()
    {
        if (Btn_StartGame == null)
            Btn_StartGame = GameObject.Find("Canvas/EnterGame").GetComponent<Button>();

        Btn_StartGame.onClick.AddListener(() => LoadScene(iFirstSceneIndex));
    }

    void LoadScene(int r_iSceneIndex)
    {
        SceneManager.LoadScene(r_iSceneIndex);
    }
}