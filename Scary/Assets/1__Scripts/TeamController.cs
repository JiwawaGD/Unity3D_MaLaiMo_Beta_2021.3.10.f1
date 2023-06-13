using UnityEngine;
using UnityEngine.UI;

public class TeamController : MonoBehaviour
{
    [SerializeField] Button ReturnBtn;
    [SerializeField] Button GroupMarkBtn;
    [SerializeField] Button SponsorMarkBtn;

    [SerializeField] GameObject GroupPage;
    [SerializeField] GameObject SponsorPage;

    [SerializeField] MenuController MenuCtlr;

    void Start()
    {
        ReturnBtn.onClick.AddListener(SettingReturn);
        GroupMarkBtn.onClick.AddListener(() => { ShowSelectPage(0); });
        SponsorMarkBtn.onClick.AddListener(() => { ShowSelectPage(1); });
    }

    void SettingReturn()
    {
        GameObject TeamView = gameObject.transform.GetChild(1).gameObject;
        TeamView.SetActive(false);

        if (MenuCtlr != null)
        {
            MenuCtlr = GameObject.Find("MenuController").GetComponent<MenuController>();
            MenuCtlr.ShowAllBtn();
        }
    }

    void ShowSelectPage(int r_iPage)
    {
        HideAllPage();

        switch (r_iPage)
        {
            case 0:
                GroupPage.SetActive(true);
                break;
            case 1:
                SponsorPage.SetActive(true);
                break;
            default:
                break;
        }
    }

    void HideAllPage()
    {
        GroupPage.SetActive(false);
        SponsorPage.SetActive(false);
    }
}