using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuController : MonoBehaviour
{
    [SerializeField] int iNextSceneID;

    [SerializeField] [Header("進入遊戲 按鈕")] Button Btn_EnterGame;
    [SerializeField] [Header("遊戲設定 按鈕")] Button Btn_GameSetting;
    [SerializeField] [Header("製作人員 按鈕")] Button Btn_Team;
    [SerializeField] [Header("離開按鈕")] Button Btn_EndGame;

    void Start()
    {
        if (Btn_EnterGame == null)
            Btn_EnterGame = GameObject.Find("MenuCanvas/EnterGame").GetComponent<Button>();

        if (Btn_GameSetting == null)
            Btn_GameSetting = GameObject.Find("MenuCanvas/Setting/SettingBtn").GetComponent<Button>();

        if (Btn_Team == null)
            Btn_Team = GameObject.Find("MenuCanvas/Team/TeamBtn").GetComponent<Button>();

        if (Btn_EndGame == null)
            Btn_EndGame = GameObject.Find("MenuCanvas/EndGame").GetComponent<Button>();

        Btn_EnterGame.onClick.AddListener(() => LoadScene(iNextSceneID));
        Btn_GameSetting.onClick.AddListener(() => ShowGameSetting());
        Btn_Team.onClick.AddListener(() => ShowTeam());
        Btn_EndGame.onClick.AddListener(() => EndGame());
    }

    void LoadScene(int r_iSceneIndex)
    {
        SceneManager.LoadScene(r_iSceneIndex);
    }

    void ShowGameSetting()
    {
        HideAllBtn();
        GameObject SettingView = Btn_GameSetting.gameObject.transform.parent.GetChild(1).gameObject;
        SettingView.SetActive(true);
    }

    void ShowTeam()
    {
        HideAllBtn();
        GameObject TeamView = Btn_Team.gameObject.transform.parent.GetChild(1).gameObject;
        TeamView.SetActive(true);
    }

    void EndGame()
    {
        Application.Quit();
    }

    public void ShowAllBtn()
    {
        Btn_EnterGame.gameObject.SetActive(true);
        Btn_GameSetting.gameObject.SetActive(true);
        Btn_Team.gameObject.SetActive(true);
        Btn_EndGame.gameObject.SetActive(true);
    }

    public void HideAllBtn()
    {
        Btn_EnterGame.gameObject.SetActive(false);
        Btn_GameSetting.gameObject.SetActive(false);
        Btn_Team.gameObject.SetActive(false);
        Btn_EndGame.gameObject.SetActive(false);
    }
}