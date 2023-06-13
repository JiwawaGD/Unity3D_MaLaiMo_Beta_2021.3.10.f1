using UnityEngine;
using UnityEngine.UI;

public class GameSettingController : MonoBehaviour
{
    [SerializeField] Button SaveBtn;
    [SerializeField] Button ReturnBtn;

    [SerializeField] Button VisableMarkBtn;
    [SerializeField] Button VolumeMarkBtn;
    [SerializeField] Button OperateMarkBtn;

    [SerializeField] GameObject VisablePage;
    [SerializeField] GameObject VolumePage;
    [SerializeField] GameObject OperatePage;

    [SerializeField] Slider MusicSlider;
    [SerializeField] Slider SoundEffectSlider;
    [SerializeField] Slider SensitivitySlider;
    [SerializeField] Dropdown QualityDropDown;

    [SerializeField] MenuController MenuCtlr;

    float fMusic;
    float fSoundEffect;
    float fQuality;
    float fSensitivity;

    void Start()
    {
        SaveBtn.onClick.AddListener(SettingSave);
        ReturnBtn.onClick.AddListener(SettingReturn);
        VisableMarkBtn.onClick.AddListener(() => { ShowSelectPage(0); });
        VolumeMarkBtn.onClick.AddListener(() => { ShowSelectPage(1); });
        OperateMarkBtn.onClick.AddListener(() => { ShowSelectPage(2); });
    }

    public void SetMusic()
    {
        fMusic = MusicSlider.value;
    }

    public void SetSoundEffect()
    {
        fSoundEffect = SoundEffectSlider.value;
    }

    public void SetSensitivity()
    {
        fSensitivity = SensitivitySlider.value; ;
    }

    public void SetPictureQuality()
    {
        fQuality = QualityDropDown.value;
    }

    void SettingSave()
    {
        GlobalDeclare.fQuality = fQuality;
        GlobalDeclare.fMusic = fMusic;
        GlobalDeclare.fSoundEffect = fSoundEffect;
        GlobalDeclare.fSensitivity = fSensitivity;

        GameObject SettingView = gameObject.transform.GetChild(1).gameObject;
        SettingView.SetActive(false);

        if (MenuCtlr != null)
        {
            MenuCtlr = GameObject.Find("MenuController").GetComponent<MenuController>();
            MenuCtlr.ShowAllBtn();
        }
    }

    void SettingReturn()
    {
        GameObject SettingView = gameObject.transform.GetChild(1).gameObject;
        SettingView.SetActive(false);

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
                VisablePage.SetActive(true);
                break;
            case 1:
                VolumePage.SetActive(true);
                break;
            case 2:
                OperatePage.SetActive(true);
                break;
            default:
                break;
        }
    }

    void HideAllPage()
    {
        VisablePage.SetActive(false);
        VolumePage.SetActive(false);
        OperatePage.SetActive(false);
    }
}