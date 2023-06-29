using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class AudSetting : MonoBehaviour
{
    [SerializeField] AudioMixer audioMixer;
    [SerializeField] Slider musicSlider;
    [SerializeField] Slider sfxSlider;

    const string MIXER_MUSIC = "MusicVolume";
    const string MIXER_SFX = "SFXVolume";

    private void Awake()
    {
        musicSlider.onValueChanged.AddListener(SetMusicVolume);
        sfxSlider.onValueChanged.AddListener(SetSFXVolume);
    }
    void SetMusicVolume(float value)
    {
        audioMixer.SetFloat(MIXER_MUSIC, Mathf.Log10(value) * 20);
    }
    void SetSFXVolume(float value)
    {
        audioMixer.SetFloat(MIXER_SFX, Mathf.Log10(value) * 20);
    }
}
