using UnityEngine;
using UnityEngine.Audio;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;


public class AUDManager : MonoBehaviour
{
    List<AudioSource> audioSourceList = new List<AudioSource>();
    // 全局音效管理元件
    [SerializeField] AudioMixer audioMixer;
    public static AUDManager instance;


    [SerializeField] Dictionary<string, AssetReference> soundReferences = new Dictionary<string, AssetReference>();

    [SerializeField] string audioAssetBundleName;
    [SerializeField] AudioSource mainAudioSource;
    [SerializeField] AudioSource ScendAudioSource;
    // [SerializeField] GameObject playerObj;

    // 鬼影專用音效
    [SerializeField, Tooltip("鬼影尖銳刺耳音效")] AssetReference ghostly_shrill_sound_effects;

    // 遊戲事件音效
    [SerializeField, Tooltip("鏡子破碎聲")] AssetReference mirror_Breaking_Sound;
    [SerializeField, Tooltip("發現線索音效")] AssetReference get_Item_Sound;
    [SerializeField, Tooltip("墜落轉黑畫面時")] AssetReference falling_To_Black_Screen_Sound;

    #region 人物/物品聲 
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
    [SerializeField, Tooltip("門縫鬼影")] AssetReference ghost_In_The_Door;
    #endregion


    #region 房間  
    [SerializeField, Tooltip("開關燈")] AssetReference light_Switch_Sound;
    // <summary>
    /// /
    /// </summary>
    [SerializeField, Tooltip("抽屜")] AssetReference drawer_Opening_Sound;
    [SerializeField, Tooltip("床")] AssetReference getting_Out_Of_Bed;
    [SerializeField, Tooltip("衣櫃")] AssetReference the_Sound_Of_Opening_Wardrobes_And_Doors;
    [SerializeField, Tooltip("鑰匙")] AssetReference tet_Sound_Of_Get_The_Key;
    #endregion

    #region 客廳
    [SerializeField, Tooltip("摺紙聲")] AssetReference[] gold_Paper;
    [SerializeField, Tooltip("時鐘")] AssetReference clock;
    [SerializeField, Tooltip("鋼琴")] AssetReference piano;
    [SerializeField, Tooltip("孝簾拉開音效")] AssetReference filial_Piety_Curtain;
    [SerializeField, Tooltip("佛歌")] AssetReference buddhist_Song;
    #endregion

    [SerializeField, Tooltip("蠟燭燃燒")] AssetReference candle_Burning;
    #region 客廳+廁所
    [SerializeField, Tooltip("門被鎖起來打不開音效")] AssetReference the_door_is_locked_and_cannot_be_opened_with_sound_effects;
    [SerializeField, Tooltip("關門聲")] AssetReference door_Close;
    [SerializeField, Tooltip("老舊門開門聲")] AssetReference the_sound_of_the_old_door_opening;
    #endregion

    #region 廚房
    [SerializeField, Tooltip("腳尾飯")] AssetReference sound_Of_Something_Falling;
    #endregion

    [SerializeField, Header("廁所")] AssetReference bathroomSound;
    #region 廁所
    [SerializeField, Tooltip("廁所水滴聲")] AssetReference toilet_water_dripping_sound;
    [SerializeField, Tooltip("墜落後黑畫面")] AssetReference Falling_To_Black_Screen_Sound;
    [SerializeField, Tooltip("轉水龍頭聲")] AssetReference turn_The_Tap;
    [SerializeField, Tooltip("敲門聲")] AssetReference emergency_Knock_On_The_Door;
    #endregion

    #region 環境&其他
    [SerializeField, Tooltip("白噪音")] AssetReference white_Noise;
    [SerializeField, Tooltip("遊戲開始鐘鼓音效")] AssetReference game_start_bell_and_drum_sound_effect;
    [SerializeField, Tooltip("恐怖開始")] AssetReference horror_Start;
    [SerializeField, Tooltip("發現線索音效")] AssetReference soprano_Violin;
    [SerializeField, Tooltip("進入場景低沉音效")] AssetReference enter_Scene_Sound;
    [SerializeField, Tooltip("UI內文")] AssetReference ui_Context;
    #endregion

    // 音量調整
    public const string MUSIC_KEY = "musicVolume";
    public const string SFX_KEY = "sfxVolume";

    private void Awake()
    {
        Addressables.InitializeAsync();
        for (int i = 0; i < 3; i++)
        {
            var audioSource = gameObject.AddComponent<AudioSource>();
            audioSourceList.Add(audioSource);
        }
        mainAudioSource = GetComponent<AudioSource>();
        Transform childTransform = transform.Find("SecondAudioSource");
        ScendAudioSource = transform.Find("SecondAudioSource")?.GetComponent<AudioSource>();
        if (instance == null) instance = this;
    }
    private IEnumerator RestoreVolume(float originalVolume)
    {
        yield return new WaitForSeconds(mainAudioSource.clip.length); // 等待音效播放完畢

        mainAudioSource.volume = originalVolume; // 恢復原始音量
    }
    private AudioSource GetAudioSource(int index)
    {
        if (index >= 0 && index < audioSourceList.Count)
        {
            return audioSourceList[index];
        }
        return null;
    }
    public void Play(int index, string name, bool isLoop)
    {
        var audioSource = GetAudioSource(index);
        if (audioSource != null)
        {
            var clip = GetAudioClip(name);
            if (clip != null)
            {
                audioSource.clip = clip;
                audioSource.loop = isLoop;
                audioSource.Play();
            }
        }
    }
    void LoadAndPlaySound(string soundName, AssetReference soundReference, AudioSource targetAudioSource)
    {
        Addressables.LoadAssetAsync<AudioClip>(soundReference).Completed += (handle) => OnSoundLoaded(handle, soundName, targetAudioSource);
    }
    void OnSoundLoaded(AsyncOperationHandle<AudioClip> handle, string soundName, AudioSource targetAudioSource)
    {
        if (handle.Status == AsyncOperationStatus.Succeeded)
        {
            AudioClip audioClip = handle.Result;

            targetAudioSource.clip = audioClip;
            targetAudioSource.Play();

            Addressables.Release(handle);
        }
        else
        {
            Debug.LogError("Failed to load sound from Addressables: " + handle.DebugName);
        }
    }
    AudioClip GetAudioClip(string name)
    {
        if (soundReferences.ContainsKey(name))
        {
            LoadAndPlaySound(name, soundReferences[name], mainAudioSource);
            return null; // 因為我們使用非同步方式加載，因此這裡返回null
        }
        AssetReference soundReference;

        // 如果不是Addressable，保留原有的音效加載邏輯
        switch (name)
        {
            // 房間音效
            case "light_Switch_Sound":
                soundReference = light_Switch_Sound;
                break;

            case "drawer_Opening_Sound":
                soundReference = drawer_Opening_Sound;
                break;

            case "getting_Out_Of_Bed":
                soundReference = getting_Out_Of_Bed;
                break;

            case "the_Sound_Of_Opening_Wardrobes_And_Doors":
                soundReference = the_Sound_Of_Opening_Wardrobes_And_Doors;
                break;

            case "tet_Sound_Of_Get_The_Key":
                soundReference = tet_Sound_Of_Get_The_Key;
                break;

            // 客廳音效
            case "gold_Paper":
                soundReference = gold_Paper[Random.Range(0, gold_Paper.Length)];
                break;
            case "clock":
                soundReference = clock;
                break;

            case "piano":
                soundReference = piano;
                break;

            case "filial_Piety_Curtain":
                soundReference = filial_Piety_Curtain;
                break;

            case "buddhist_Song":
                soundReference = buddhist_Song;
                break;

            // 環境&其他音效
            case "candle_Burning":
                soundReference = candle_Burning;
                break;

            case "the_door_is_locked_and_cannot_be_opened_with_sound_effects":
                soundReference = the_door_is_locked_and_cannot_be_opened_with_sound_effects;
                break;

            case "door_Close":
                soundReference = door_Close;
                break;

            case "the_sound_of_the_old_door_opening":
                soundReference = the_sound_of_the_old_door_opening;
                break;

            case "sound_Of_Something_Falling":
                soundReference = sound_Of_Something_Falling;
                break;

            case "toilet_water_dripping_sound":
                soundReference = toilet_water_dripping_sound;
                break;

            case "Falling_To_Black_Screen_Sound":
                soundReference = Falling_To_Black_Screen_Sound;
                break;

            case "turn_The_Tap":
                soundReference = turn_The_Tap;
                break;

            case "emergency_Knock_On_The_Door":
                soundReference = emergency_Knock_On_The_Door;
                break;

            // 其他音效
            case "white_Noise":
                soundReference = white_Noise;
                break;

            case "game_start_bell_and_drum_sound_effect":
                soundReference = game_start_bell_and_drum_sound_effect;
                break;

            case "horror_Start":
                soundReference = horror_Start;
                break;

            case "soprano_Violin":
                soundReference = soprano_Violin;
                break;

            case "enter_Scene_Sound":
                soundReference = enter_Scene_Sound;
                break;

            case "ui_Context":
                soundReference = ui_Context;
                break;
        }

        return null;
    }
    public void GrandmaStrangeVoiceStop()
    {
        mainAudioSource.PlayOneShot(soundReferences["grandma_StrangeVoice"].Asset as AudioClip);
    }

    #region 音效 過去寫法
    // private IEnumerator RestoreVolume(float originalVolume) // 恢復音量
    // {
    //     yield return new WaitForSeconds(drawer_Opening_Sound.length); // 等待音效播放完畢

    //     mainAudioSource.volume = originalVolume; // 恢復原始音量
    // }



    // AudioClip GetAudioClip(string name)
    // {
    //     switch (name)
    //     {
    //         case "ghosting_Sound_Special":
    //             var ghosting_Sound_Special = ghostly_shrill_sound_effects.Asset.loa
    //         case "mirror_Breaking_Sound":
    //             return mirror_Breaking_Sound;
    //         case "get_Item_Sound":
    //             return get_Item_Sound;
    //         case "falling_To_Black_Screen_Sound":
    //             return falling_To_Black_Screen_Sound;
    //         case "the_sound_of_something_moving":
    //             return the_sound_of_something_moving;
    //         case "strange_noises_keep_coming":
    //             return children_laughing;
    //         case "grandma_StrangeVoice":
    //             return grandma_StrangeVoice;
    //         case "body_Twisting_Sound":
    //             return body_Twisting_Sound;
    //         case "muffled_Vocals":
    //             return muffled_Vocals;
    //         case "strained_Breathing":
    //             return nervous_Breathing;
    //         case "flashlight_Switch_Sound":
    //             return flashlight_Switch_Sound;
    //         case "ghosting_Sound":
    //             return the_hoarse_roar_of_the_ghost;
    //         case "ghost_Escape":
    //             return ghosts_appear_quickly_with_short_sound_effects;
    //         case "ghostIn_The_Door":
    //             return ghost_In_The_Door;


    //         case "light_Switch_Sound":
    //             return light_Switch_Sound.Asset as AudioClip;   // 這裡要改 用Addressable Asset System 來取得音效 但是要先把音效打包成AssetBundle 並且在AssetBundle中設定Asset Name


    //         case "drawer_Opening_Sound":
    //             return drawer_Opening_Sound;
    //         case "getting_Out_Of_Bed":
    //             return getting_Out_Of_Bed;
    //         case "the_Sound_Of_Opening_Wardrobes_And_Doors":
    //             return the_Sound_Of_Opening_Wardrobes_And_Doors;
    //         case "tet_Sound_Of_Get_The_Key":
    //             return tet_Sound_Of_Get_The_Key;
    //         case "gold_Paper":
    //             return gold_Paper[Random.Range(0, 2)];
    //         case "clock":
    //             return clock;
    //         case "piano":
    //             return piano;
    //         case "filial_Piety_Curtain":
    //             return filial_Piety_Curtain;
    //         case "buddhist_Song":
    //             return buddhist_Song;
    //         case "candle_Burning":
    //             return candle_Burning;
    //         case "door_Unlock_Sound":
    //             return the_door_is_locked_and_cannot_be_opened_with_sound_effects;
    //         case "door_Slam":
    //             return door_Close;
    //         case "door_Opening":
    //             return the_sound_of_the_old_door_opening;
    //         case "sound_Of_Something_Falling":
    //             return sound_Of_Something_Falling;
    //         case "dripping_Sound":
    //             return toilet_water_dripping_sound;
    //         case "black_Screen_After_Fall":
    //             return Falling_To_Black_Screen_Sound;
    //         case "turn_The_Tap":
    //             return turn_The_Tap;
    //         case "emergency_Knock_On_The_Door":
    //             return emergency_Knock_On_The_Door;
    //         case "white_Noise":
    //             return white_Noise;
    //         case "games_Start":
    //             return game_start_bell_and_drum_sound_effect;
    //         case "horror_Start":
    //             return horror_Start;
    //         case "soprano_Violin":
    //             return soprano_Violin;
    //         case "enter_Scene_Sound":
    //             return enter_Scene_Sound;
    //         case "ui_Context":
    //             return ui_Context;
    //     }
    //     return null;
    // }
    // public void GrandmaStrangeVoiceStop()
    // {
    //     mainAudioSource.PlayOneShot(grandma_StrangeVoice);
    // }


    // public void OpenTheDrawerSFX()
    // {
    //     float originalVolume = mainAudioSource.volume;
    //     mainAudioSource.volume = originalVolume * 0.5f;
    //     mainAudioSource.PlayOneShot(drawer_Opening_Sound);

    //     StartCoroutine(RestoreVolume(originalVolume));
    // }
    // public void PlayerLotusPaperSFX()
    // {
    //     mainAudioSource.PlayOneShot(gold_Paper[Random.Range(0, 2)]);
    // }
    // public void PlayerGameEventSFX()
    // {
    //     mainAudioSource.PlayOneShot(ui_Context);
    // }
    // public void BodyTwistingSound()
    // {
    //     mainAudioSource.PlayOneShot(body_Twisting_Sound);
    // }

    // public void TheSoundOfSomethingMoving()
    // {
    //     mainAudioSource.PlayOneShot(the_sound_of_something_moving);
    // }
    #endregion

    public void TheSoundOfOpeningWardrobesAndDoors()
    {
        mainAudioSource.PlayOneShot(light_Switch_Sound.Asset as AudioClip);
    }


    /// <summary>
    /// 存取紀錄
    /// </summary>
    public void LoadVolume()
    {
        float musicVolume = PlayerPrefs.GetFloat(MUSIC_KEY, 1f);
        float sfxVolume = PlayerPrefs.GetFloat(SFX_KEY, 1f);

        audioMixer.SetFloat(AudSetting.MIXER_MUSIC, Mathf.Log10(musicVolume) * 20);
        audioMixer.SetFloat(AudSetting.MIXER_SFX, Mathf.Log10(sfxVolume) * 20);
    }
}

