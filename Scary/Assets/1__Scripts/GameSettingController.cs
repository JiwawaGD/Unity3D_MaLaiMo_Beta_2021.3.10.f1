using UnityEngine;
using UnityEngine.UI;

public class GameSettingController : MonoBehaviour
{
    [SerializeField] [Header("儲存按鈕")] Button SaveBtn;
    [SerializeField] [Header("儲存按鈕")] Button ReturnBtn;

    [SerializeField] [Header("影像 - 按鈕")] Button VisableMarkBtn;
    [SerializeField] [Header("影像 - 物件")] GameObject VisablePage;
    [SerializeField] [Header("影像 - Text")] Text VisableMarkText;

    [SerializeField] [Header("音效 - 按鈕")] Button VolumeMarkBtn;
    [SerializeField] [Header("音效 - 物件")] GameObject VolumePage;
    [SerializeField] [Header("音效 - Text")] Text VolumeMarkText;

    [SerializeField] [Header("操作 - 按鈕")] Button OperateMarkBtn;
    [SerializeField] [Header("操作 - 物件")] GameObject OperatePage;
    [SerializeField] [Header("操作 - Text")] Text OperateMarkText;

    [SerializeField] [Header("音樂 - Slider")] Slider MusicSlider;
    [SerializeField] [Header("音效 - Slider")] Slider SoundEffectSlider;
    [SerializeField] [Header("靈敏度 - Slider")] Slider SensitivitySlider;
    [SerializeField] [Header("畫質 - Dropdown")] Dropdown QualityDropDown;

    [SerializeField] [Header("MenuCtrlr")] MenuController MenuCtrlr;
    [SerializeField] [Header("PlayerCtrlr")] PlayerController PlayerCtrlr;

    [SerializeField] [Header("AUDManager")] AUDManager AUDManager;

    float fMusic;
    float fSoundEffect;
    float fSensitivity;
    float fQuality;

    void Start()
    {
        SaveBtn.onClick.AddListener(SettingSave);
        ReturnBtn.onClick.AddListener(SettingReturn);
        VisableMarkBtn.onClick.AddListener(() => { ShowSelectPage(0); });
        VolumeMarkBtn.onClick.AddListener(() => { ShowSelectPage(1); });
        OperateMarkBtn.onClick.AddListener(() => { ShowSelectPage(2); });

        ShowSelectPage(0);
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
        fSensitivity = SensitivitySlider.value;
    }

    public void SetPictureQuality()
    {
        fQuality = QualityDropDown.value;
    }

    void SettingSave()
    {
        GlobalDeclare.fMusic = fMusic;
        GlobalDeclare.fSoundEffect = fSoundEffect;
        GlobalDeclare.fSensitivity = fSensitivity;
        GlobalDeclare.fQuality = fQuality;

        GameObject SettingView = gameObject.transform.GetChild(1).gameObject;
        SettingView.SetActive(false);

        if (MenuCtrlr != null)
        {
            MenuCtrlr = GameObject.Find("MenuController").GetComponent<MenuController>();
            MenuCtrlr.ShowAllBtn();
        }

        if (PlayerCtrlr != null)
        {
            PlayerCtrlr.fSensitivityAmplifier = fSensitivity;
        }

        AUDManager.LoadVolume();
    }

    void SettingReturn()
    {
        GameObject SettingView = gameObject.transform.GetChild(1).gameObject;
        SettingView.SetActive(false);

        if (MenuCtrlr != null)
        {
            MenuCtrlr = GameObject.Find("MenuController").GetComponent<MenuController>();
            MenuCtrlr.ShowAllBtn();
        }
    }

    void ShowSelectPage(int r_iPage)
    {
        HideAllPage();
        DefaultFontStyleAndSize();

        switch (r_iPage)
        {
            case 0:
                VisablePage.SetActive(true);
                VisableMarkText.fontStyle = FontStyle.Bold;
                VisableMarkText.fontSize = 48;
                break;
            case 1:
                VolumePage.SetActive(true);
                VolumeMarkText.fontStyle = FontStyle.Bold;
                VolumeMarkText.fontSize = 48;
                break;
            case 2:
                OperatePage.SetActive(true);
                OperateMarkText.fontStyle = FontStyle.Bold;
                OperateMarkText.fontSize = 48;
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

    void DefaultFontStyleAndSize()
    {
        VisableMarkText.fontStyle = FontStyle.Normal;
        VisableMarkText.fontSize = 36;

        VolumeMarkText.fontStyle = FontStyle.Normal;
        VolumeMarkText.fontSize = 36;

        OperateMarkText.fontStyle = FontStyle.Normal;
        OperateMarkText.fontSize = 36;
    }
}