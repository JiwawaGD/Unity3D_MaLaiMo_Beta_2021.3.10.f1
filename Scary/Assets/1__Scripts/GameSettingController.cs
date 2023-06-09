using UnityEngine;
using UnityEngine.UI;

public class GameSettingController : MonoBehaviour
{
    [SerializeField] Button SaveBtn;
    [SerializeField] Button ReturnBtn;
    [SerializeField] Slider MusicSlider;
    [SerializeField] Slider VolumeSlider;
    [SerializeField] Dropdown QualityDropDown;

    [SerializeField] MenuController MenuCtlr;

    float fMusic;
    float fVolume;
    float fQuality;

    void Start()
    {
        SaveBtn.onClick.AddListener(SettingSave);
        ReturnBtn.onClick.AddListener(SettingReturn);
    }

    public void SetMusic()
    {
        fMusic = MusicSlider.value;
    }

    public void SetVolume()
    {
        fVolume = VolumeSlider.value;
    }

    public void SetPictureQuality()
    {
        fQuality = QualityDropDown.value;
    }

    public void SettingSave()
    {
        GlobalDeclare.fQuality = fQuality;
        GlobalDeclare.fMusic = fMusic;
        GlobalDeclare.fVolume = fVolume;

        GameObject SettingView = gameObject.transform.GetChild(1).gameObject;
        SettingView.SetActive(false);

        if (MenuCtlr != null)
        {
            MenuCtlr = GameObject.Find("MenuController").GetComponent<MenuController>();
            MenuCtlr.ShowAllBtn();
        }
    }

    public void SettingReturn()
    {
        GameObject SettingView = gameObject.transform.GetChild(1).gameObject;
        SettingView.SetActive(false);

        if (MenuCtlr != null)
        {
            MenuCtlr = GameObject.Find("MenuController").GetComponent<MenuController>();
            MenuCtlr.ShowAllBtn();
        }
    }
}