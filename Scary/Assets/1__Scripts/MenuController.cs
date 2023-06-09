using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuController : MonoBehaviour
{
    public bool bLoadSceneAsync;
    [SerializeField] int iNextSceneID;

    [SerializeField] Button Btn_EnterGame;
    [SerializeField] Button Btn_GameSetting;
    [SerializeField] Button Btn_Team;
    [SerializeField] Button Btn_TeamReturn;
    [SerializeField] Button Btn_EndGame;

    public GameObject TineLineAniObj;

    void Start()
    {
        if (Btn_EnterGame == null)
            Btn_EnterGame = GameObject.Find("Canvas/EnterGame").GetComponent<Button>();

        if (Btn_GameSetting == null)
            Btn_GameSetting = GameObject.Find("Canvas/Setting/SettingBtn").GetComponent<Button>();

        if (Btn_Team == null)
            Btn_Team = GameObject.Find("Canvas/Team/TeamBtn").GetComponent<Button>();

        if (Btn_EndGame == null)
            Btn_EndGame = GameObject.Find("Canvas/EndGame").GetComponent<Button>();

        Btn_EnterGame.onClick.AddListener(() => LoadScene(iNextSceneID));
        Btn_GameSetting.onClick.AddListener(() => ShowGameSetting());
        Btn_Team.onClick.AddListener(() => ShowTeam());
        Btn_TeamReturn.onClick.AddListener(TeamReturn);
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
        //GameSettingController settingCtlr = Btn_GameSetting.gameObject.transform.parent.GetChild(0).GetComponent<GameSettingController>();
        //settingCtlr.SendMessage("");
    }

    void ShowTeam()
    {
        HideAllBtn();
        Btn_TeamReturn = GameObject.Find("Canvas/Team/View/Return").GetComponent<Button>();
        GameObject TeamView = Btn_Team.gameObject.transform.parent.GetChild(1).gameObject;
        TeamView.SetActive(true);
    }

    void TeamReturn()
    {
        ShowAllBtn();
        Btn_TeamReturn = null;
        GameObject TeamView = Btn_Team.gameObject.transform.parent.GetChild(1).gameObject;
        TeamView.SetActive(false);
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