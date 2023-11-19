using UnityEngine;
using UnityEngine.Audio;
using System.Collections;
using Fungus;
using OldBrickHouse;
using System.Collections.Generic;

public class AUDManager : MonoBehaviour
{
    List<AudioSource> audioSourceList = new List<AudioSource>();
    // 全局音效管理元件
    [SerializeField] AudioMixer audioMixer;
    public static AUDManager instance;

    [SerializeField] string audioAssetBundleName;
    [SerializeField] AudioSource mainAudioSource;
    [SerializeField] AudioSource ScendAudioSource;
    // [SerializeField] GameObject playerObj;

    // 鬼影專用音效
    [Tooltip("鬼影尖銳刺耳音效")] public AudioClip ghostly_shrill_sound_effects;

    // 遊戲事件音效
    [Tooltip("鏡子破碎聲")] public AudioClip mirror_Breaking_Sound;
    [Tooltip("發現線索音效")] public AudioClip get_Item_Sound;
    [Tooltip("墜落轉黑畫面時")] public AudioClip falling_To_Black_Screen_Sound;

    #region 人物/物品聲 
    [Tooltip("東西竄動的聲音")] public AudioClip the_sound_of_something_moving;
    [Tooltip("小朋友笑聲")] public AudioClip children_laughing;
    [Tooltip("奶奶開始向前")] public AudioClip grandma_Starts_Walking;
    [Tooltip("奶奶詭異聲")] public AudioClip grandma_StrangeVoice;
    [Tooltip("扭動身體的音效")] public AudioClip body_Twisting_Sound;
    [Tooltip("模糊不清的人聲音")] public AudioClip muffled_Vocals;
    [Tooltip("緊張呼吸聲")] public AudioClip nervous_Breathing;
    [Tooltip("手電筒開關聲")] public AudioClip flashlight_Switch_Sound;
    [Tooltip("鬼怪沙啞嘶吼聲")] public AudioClip the_hoarse_roar_of_the_ghost;
    [Tooltip("鬼影快速出現短暫音效")] public AudioClip ghosts_appear_quickly_with_short_sound_effects;
    [Tooltip("門縫鬼影")] public AudioClip ghost_In_The_Door;
    #endregion


    #region 房間  
    [SerializeField, Tooltip("開關燈")] AudioClip light_Switch_Sound;
    [SerializeField, Tooltip("抽屜")] AudioClip drawer_Opening_Sound;
    [SerializeField, Tooltip("床")] AudioClip getting_Out_Of_Bed;
    [SerializeField, Tooltip("衣櫃")] AudioClip the_Sound_Of_Opening_Wardrobes_And_Doors;
    [SerializeField, Tooltip("鑰匙")] AudioClip tet_Sound_Of_Get_The_Key;
    #endregion

    #region 客廳
    [SerializeField, Tooltip("摺紙聲")] AudioClip[] gold_Paper;
    [SerializeField, Tooltip("時鐘")] AudioClip clock;
    [SerializeField, Tooltip("鋼琴")] AudioClip piano;
    [SerializeField, Tooltip("孝簾拉開音效")] AudioClip filial_Piety_Curtain;
    [SerializeField, Tooltip("佛歌")] public AudioClip buddhist_Song;
    #endregion

    [SerializeField, Tooltip("蠟燭燃燒")] public AudioClip candle_Burning;
    #region 客廳+廁所
    [SerializeField, Tooltip("門被鎖起來打不開音效")] AudioClip the_door_is_locked_and_cannot_be_opened_with_sound_effects;
    [SerializeField, Tooltip("關門聲")] AudioClip door_Close;
    [SerializeField, Tooltip("老舊門開門聲")] AudioClip the_sound_of_the_old_door_opening;
    #endregion

    #region 廚房
    [SerializeField, Tooltip("腳尾飯")] AudioClip sound_Of_Something_Falling;
    #endregion

    [SerializeField, Header("廁所")] AudioSource bathroomSound;
    #region 廁所
    [SerializeField, Tooltip("廁所水滴聲")] AudioClip toilet_water_dripping_sound;
     [SerializeField, Tooltip("墜落後黑畫面")] AudioClip Falling_To_Black_Screen_Sound;
    [SerializeField, Tooltip("轉水龍頭聲")] AudioClip turn_The_Tap;
    [SerializeField, Tooltip("敲門聲")] public AudioClip emergency_Knock_On_The_Door;
    #endregion

    #region 環境&其他
    [SerializeField, Tooltip("白噪音")] AudioClip white_Noise;
    [SerializeField, Tooltip("遊戲開始鐘鼓音效")] AudioClip game_start_bell_and_drum_sound_effect;
    [SerializeField, Tooltip("恐怖開始")] AudioClip horror_Start;
    [SerializeField, Tooltip("發現線索音效")] AudioClip soprano_Violin;
    [SerializeField, Tooltip("進入場景低沉音效")] AudioClip enter_Scene_Sound;
    [SerializeField, Tooltip("UI內文")] AudioClip ui_Context;
    #endregion

    // 音量調整
    public const string MUSIC_KEY = "musicVolume";
    public const string SFX_KEY = "sfxVolume";

    private void Awake()
    {
        for (int i = 0; i < 3; i++)
        {
            var audioSource = gameObject.AddComponent<AudioSource>();
            audioSourceList.Add(audioSource);
        }
        mainAudioSource = GetComponent<AudioSource>();
        Transform childTransform = transform.Find("SecondAudioSource");
        if (childTransform != null)
            ScendAudioSource = childTransform.GetComponent<AudioSource>();
        if (instance == null) instance = this;
    }

    private IEnumerator RestoreVolume(float originalVolume)
    {
        yield return new WaitForSeconds(drawer_Opening_Sound.length); // 等待音效播放完畢

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

    AudioClip GetAudioClip(string name)
    {
        switch (name)
        {
            case "ghosting_Sound_Special":
                return ghostly_shrill_sound_effects;
            case "mirror_Breaking_Sound":
                return mirror_Breaking_Sound;
            case "get_Item_Sound":
                return get_Item_Sound;
            case "falling_To_Black_Screen_Sound":
                return falling_To_Black_Screen_Sound;
            case "the_sound_of_something_moving":
                return the_sound_of_something_moving;
            case "strange_noises_keep_coming":
                return children_laughing;
            case "grandma_StrangeVoice":
                return grandma_StrangeVoice;
            case "body_Twisting_Sound":
                return body_Twisting_Sound;
            case "muffled_Vocals":
                return muffled_Vocals;
            case "strained_Breathing":
                return nervous_Breathing;
            case "flashlight_Switch_Sound":
                return flashlight_Switch_Sound;
            case "ghosting_Sound":
                return the_hoarse_roar_of_the_ghost;
            case "ghost_Escape":
                return ghosts_appear_quickly_with_short_sound_effects;
            case "ghostIn_The_Door":
                return ghost_In_The_Door;
            case "light_Switch_Sound":
                return light_Switch_Sound;
            case "drawer_Opening_Sound":
                return drawer_Opening_Sound;
            case "getting_Out_Of_Bed":
                return getting_Out_Of_Bed;
            case "the_Sound_Of_Opening_Wardrobes_And_Doors":
                return the_Sound_Of_Opening_Wardrobes_And_Doors;
            case "tet_Sound_Of_Get_The_Key":
                return tet_Sound_Of_Get_The_Key;
            case "gold_Paper":
                return gold_Paper[Random.Range(0, 2)];
            case "clock":
                return clock;
            case "piano":
                return piano;
            case "filial_Piety_Curtain":
                return filial_Piety_Curtain;
            case "buddhist_Song":
                return buddhist_Song;
            case "candle_Burning":
                return candle_Burning;
            case "door_Unlock_Sound":
                return the_door_is_locked_and_cannot_be_opened_with_sound_effects;
            case "door_Slam":
                return door_Close;
            case "door_Opening":
                return the_sound_of_the_old_door_opening;
            case "sound_Of_Something_Falling":
                return sound_Of_Something_Falling;
            case "dripping_Sound":
                return toilet_water_dripping_sound;
            case "black_Screen_After_Fall":
                return Falling_To_Black_Screen_Sound;
            case "turn_The_Tap":
                return turn_The_Tap;
            case "emergency_Knock_On_The_Door":
                return emergency_Knock_On_The_Door;
            case "white_Noise":
                return white_Noise;
            case "games_Start":
                return game_start_bell_and_drum_sound_effect;
            case "horror_Start":
                return horror_Start;
            case "soprano_Violin":
                return soprano_Violin;
            case "enter_Scene_Sound":
                return enter_Scene_Sound;
            case "ui_Context":
                return ui_Context;
        }
        return null;
    }
    public void GrandmaStrangeVoiceStop()
    {
        mainAudioSource.PlayOneShot(grandma_StrangeVoice);
    }


    public void OpenTheDrawerSFX()
    {
        float originalVolume = mainAudioSource.volume;
        mainAudioSource.volume = originalVolume * 0.5f;
        mainAudioSource.PlayOneShot(drawer_Opening_Sound);

        StartCoroutine(RestoreVolume(originalVolume));
    }
    public void PlayerLotusPaperSFX()
    {
        mainAudioSource.PlayOneShot(gold_Paper[Random.Range(0, 2)]);
    }
    public void PlayerGameEventSFX()
    {
        mainAudioSource.PlayOneShot(ui_Context);
    }
    public void BodyTwistingSound()
    {
        mainAudioSource.PlayOneShot(body_Twisting_Sound);
    }

    public void TheSoundOfSomethingMoving()
    {
        mainAudioSource.PlayOneShot(the_sound_of_something_moving);
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

