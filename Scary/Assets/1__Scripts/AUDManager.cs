using UnityEngine;
using UnityEngine.Audio;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class AUDManager : MonoBehaviour
{
    List<AudioSource> audioSourceList = new List<AudioSource>();
    [SerializeField] AudioMixer audioMixer;
    public static AUDManager instance;

    [SerializeField] Dictionary<string, AssetReference> soundReferences = new Dictionary<string, AssetReference>();
    [SerializeField] string audioAssetBundleName;
    [SerializeField] AudioSource mainAudioSource;

    [SerializeField, Tooltip("鬼影專用音效")] AssetReference ghostly_shrill_sound_effects;
    [SerializeField, Tooltip("鏡子破碎聲")] AssetReference mirror_Breaking_Sound;
    [SerializeField, Tooltip("發現線索音效")] AssetReference get_Item_Sound;
    [SerializeField, Tooltip("墜落轉黑畫面時")] AssetReference falling_To_Black_Screen_Sound;

    [SerializeField, Tooltip("東西竄動的聲音")] AssetReference the_sound_of_something_moving;
    [SerializeField, Tooltip("小朋友笑聲")] AssetReference children_laughing;
    [SerializeField, Tooltip("奶奶開始向前")] AssetReference grandma_Starts_Walking;
    [SerializeField, Tooltip("奶奶詭異聲")] AssetReference grandma_StrangeVoice;
    [SerializeField, Tooltip("扭動身體的音效")] AssetReference body_Twisting_Sound;
    [SerializeField, Tooltip("模糊不清的人聲音")] AssetReference muffled_Vocals;
    [SerializeField, Tooltip("緊張呼吸聲")] AssetReference nervous_Breathing;
    [SerializeField, Tooltip("手電筒開關聲")] AssetReference flashlight_Switch_Sound;
    [SerializeField, Tooltip("鬼怪沙啞嘶吼聲")] AssetReference the_hoarse_roar_of_the_ghost;
    [SerializeField, Tooltip("鬼影快速出現短暫音效")] AssetReference ghosts_appear_quickly_with_short_sound_effects;
    [SerializeField, Tooltip("門縫鬼影")] AssetReference Crying_in_the_bathroom;

    [SerializeField, Tooltip("開關燈")] AssetReference light_Switch_Sound;
    [SerializeField, Tooltip("抽屜")] AssetReference drawer_Opening_Sound;
    [SerializeField, Tooltip("床")] AssetReference getting_Out_Of_Bed;
    [SerializeField, Tooltip("衣櫃")] AssetReference the_Sound_Of_Opening_Wardrobes_And_Doors;
    [SerializeField, Tooltip("鑰匙")] AssetReference tet_Sound_Of_Get_The_Key;

    [SerializeField, Tooltip("摺紙聲")] AssetReference[] gold_Paper;
    [SerializeField, Tooltip("時鐘")] AssetReference clock;
    [SerializeField, Tooltip("鋼琴")] AssetReference piano;
    [SerializeField, Tooltip("孝簾拉開音效")] AssetReference filial_Piety_Curtain;
    [SerializeField, Tooltip("佛歌")] AssetReference buddhist_Song;

    [SerializeField, Tooltip("蠟燭燃燒")] AssetReference candle_Burning;

    [SerializeField, Tooltip("門被鎖起來打不開音效")] AssetReference the_door_is_locked_and_cannot_be_opened_with_sound_effects;
    [SerializeField, Tooltip("關門聲")] AssetReference door_Close;
    [SerializeField, Tooltip("老舊門開門聲")] AssetReference the_sound_of_the_old_door_opening;

    [SerializeField, Tooltip("腳尾飯")] AssetReference sound_Of_Something_Falling;

    [SerializeField, Header("廁所")] AssetReference bathroomSound;
    [SerializeField, Tooltip("廁所水滴聲")] AssetReference toilet_water_dripping_sound;
    [SerializeField, Tooltip("鬼手推門")] AssetReference Falling_To_Black_Screen_Sound;
    [SerializeField, Tooltip("轉水龍頭聲")] AssetReference turn_The_Tap;
    [SerializeField, Tooltip("敲門聲")] AssetReference emergency_Knock_On_The_Door;

    [SerializeField, Tooltip("白噪音")] AssetReference white_Noise;
    [SerializeField, Tooltip("遊戲開始鐘鼓音效")] AssetReference game_start_bell_and_drum_sound_effect;
    [SerializeField, Tooltip("恐怖開始")] AssetReference Opening_Scene;
    [SerializeField, Tooltip("發現線索音效")] AssetReference soprano_Violin;
    [SerializeField, Tooltip("進入場景低沉音效")] AssetReference enter_Scene_Sound;
    [SerializeField, Tooltip("UI內文")] AssetReference ui_Context;
    [SerializeField] AssetReference At_the_end_it_is_found_that_Acuan_has_mostly_disappeared_and_Acuan_has_climbed_up;

    public const string MUSIC_KEY = "musicVolume";
    public const string SFX_KEY = "sfxVolume";

    void Update()
    {
        Play("ghost_Escape", false);
        //Debug.Log("ghost_Escape" + soundReferences.ContainsKey("ghost_Escape"));
    }

    public void Play(string name, bool isLoop)
    {
        if (mainAudioSource != null)
        {
            StartCoroutine(GetAudioClip(name, isLoop));
        }
        else
        {
            Debug.LogError("Main audio source is null!");
        }
    }

    IEnumerator GetAudioClip(string name, bool isLoop)
    {
        if (soundReferences.ContainsKey(name))
        {
            AssetReference soundReference = soundReferences[name];
            var handle = Addressables.LoadAssetAsync<AudioClip>(soundReference);
            yield return handle;

            if (handle.Status == AsyncOperationStatus.Succeeded)
            {
                AudioClip audioClip = handle.Result;

                if (audioClip != null)
                {
                    mainAudioSource.clip = audioClip;
                    mainAudioSource.loop = isLoop;
                    mainAudioSource.Play();
                }
                else
                {
                    Debug.LogError("Loaded AudioClip is null.");
                }

                Addressables.Release(handle);
            }
            else
            {
                Debug.LogError("Failed to load sound from Addressables: " + handle.DebugName);
            }
        }
        else
        {
            AssetReference soundRef;

            switch (name)
            {
                case "At_the_end_it_is_found_that_Acuan_has_mostly_disappeared_and_Acuan_has_climbed_up":
                    soundRef = At_the_end_it_is_found_that_Acuan_has_mostly_disappeared_and_Acuan_has_climbed_up;
                    break;
                case "body_Twisting_Sound":
                    soundRef = body_Twisting_Sound;
                    break;
                case "grandma_StrangeVoice":
                    soundRef = grandma_StrangeVoice;
                    break;
                case "grandma_Starts_Walking":
                    soundRef = grandma_Starts_Walking;
                    break;
                case "flashlight_Switch_Sound":
                    soundRef = flashlight_Switch_Sound;
                    break;
                case "Crying_in_the_bathroom":
                    soundRef = Crying_in_the_bathroom;
                    break;
                case "get_Item_Sound":
                    soundRef = get_Item_Sound;
                    break;
                // Room sounds...
                case "mirror_Breaking_Sound":
                    soundRef = mirror_Breaking_Sound;
                    break;
                case "light_Switch_Sound":
                    soundRef = light_Switch_Sound;
                    break;

                case "drawer_Opening_Sound":
                    soundRef = drawer_Opening_Sound;
                    break;

                case "getting_Out_Of_Bed":
                    soundRef = getting_Out_Of_Bed;
                    break;

                case "the_Sound_Of_Opening_Wardrobes_And_Doors":
                    soundRef = the_Sound_Of_Opening_Wardrobes_And_Doors;
                    break;

                case "tet_Sound_Of_Get_The_Key":
                    soundRef = tet_Sound_Of_Get_The_Key;
                    break;

                // Living room sounds...
                case "gold_Paper":
                    soundRef = gold_Paper[Random.Range(0, gold_Paper.Length)];
                    break;

                case "clock":
                    soundRef = clock;
                    break;

                case "piano":
                    soundRef = piano;
                    break;

                case "filial_Piety_Curtain":
                    soundRef = filial_Piety_Curtain;
                    break;

                case "buddhist_Song":
                    soundRef = buddhist_Song;
                    break;

                // Other sounds...
                case "candle_Burning":
                    soundRef = candle_Burning;
                    break;

                case "the_door_is_locked_and_cannot_be_opened_with_sound_effects":
                    soundRef = the_door_is_locked_and_cannot_be_opened_with_sound_effects;
                    break;

                case "door_Close":
                    soundRef = door_Close;
                    break;

                case "the_sound_of_the_old_door_opening":
                    soundRef = the_sound_of_the_old_door_opening;
                    break;

                case "sound_Of_Something_Falling":
                    soundRef = sound_Of_Something_Falling;
                    break;

                case "toilet_water_dripping_sound":
                    soundRef = toilet_water_dripping_sound;
                    break;

                case "Falling_To_Black_Screen_Sound":
                    soundRef = Falling_To_Black_Screen_Sound;
                    break;

                case "turn_The_Tap":
                    soundRef = turn_The_Tap;
                    break;

                case "emergency_Knock_On_The_Door":
                    soundRef = emergency_Knock_On_The_Door;
                    break;

                case "white_Noise":
                    soundRef = white_Noise;
                    break;

                case "game_start_bell_and_drum_sound_effect":
                    soundRef = game_start_bell_and_drum_sound_effect;
                    break;

                case "Opening_Scene":
                    soundRef = Opening_Scene;
                    break;

                case "soprano_Violin":
                    soundRef = soprano_Violin;
                    break;

                case "enter_Scene_Sound":
                    soundRef = enter_Scene_Sound;
                    break;

                case "ui_Context":
                    soundRef = ui_Context;
                    break;

                default:
                    soundRef = null;
                    break;
            }

            if (soundRef != null)
            {
                var handle = Addressables.LoadAssetAsync<AudioClip>(soundRef);
                yield return handle;

                if (handle.Status == AsyncOperationStatus.Succeeded)
                {
                    AudioClip audioClip = handle.Result;

                    if (audioClip != null)
                    {
                        mainAudioSource.clip = audioClip;
                        mainAudioSource.loop = isLoop;
                        mainAudioSource.Play();
                    }
                    else
                    {
                        Debug.LogError("Loaded AudioClip is null.");
                    }

                    Addressables.Release(handle);
                }
                else
                {
                    Debug.LogError("Failed to load sound from Addressables: " + handle.DebugName);
                }
            }
        }
    }

    public void GrandmaStrangeVoiceStop()
    {
        Play("At_the_end_it_is_found_that_Acuan_has_mostly_disappeared_and_Acuan_has_climbed_up", false);
    }

    public void TheSoundOfOpeningWardrobesAndDoors()
    {
        mainAudioSource.PlayOneShot(light_Switch_Sound.Asset as AudioClip);
    }

    public void LoadVolume()
    {
        float musicVolume = PlayerPrefs.GetFloat(MUSIC_KEY, 1f);
        float sfxVolume = PlayerPrefs.GetFloat(SFX_KEY, 1f);

        audioMixer.SetFloat(AudSetting.MIXER_MUSIC, Mathf.Log10(musicVolume) * 20);
        audioMixer.SetFloat(AudSetting.MIXER_SFX, Mathf.Log10(sfxVolume) * 20);
    }
}
