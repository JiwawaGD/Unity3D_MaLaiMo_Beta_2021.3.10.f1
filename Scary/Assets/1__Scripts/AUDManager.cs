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
    [SerializeField] GameObject playerObj;

    // 鬼影專用音效
    [Tooltip("鬼影專用背景音效")] public AudioClip ghosting_Sound_Special;

    // 遊戲事件音效
    [Tooltip("鏡子破碎聲")] public AudioClip mirror_Breaking_Sound;
    [Tooltip("獲取物品音效")] public AudioClip get_Item_Sound;
    [Tooltip("被鬼手推到墜落進入黑畫面時")] public AudioClip falling_To_Black_Screen_Sound;

    // 玩家聲音效
    [SerializeField, Header("玩家聲音效")] AudioSource PlayerSound;
    [SerializeField, Tooltip("玩家走路聲")] public AudioClip walkingSound;

    // 人物/物品聲音效
    #region 人物/物品聲 
    [Tooltip("東西竄動的聲音")] public AudioClip the_sound_of_something_moving;
    [Tooltip("廁所有奇怪持續的聲音")] public AudioClip strange_noises_keep_coming;
    [Tooltip("奶奶開始向前")] public AudioClip grandma_Starts_Walking;
    [Tooltip("奶奶詭異聲")] public AudioClip grandma_StrangeVoice;
    [Tooltip("扭動身體的音效")] public AudioClip body_Twisting_Sound;
    [Tooltip("模糊不清的人聲音")] public AudioClip muffled_Vocals;
    [Tooltip("腳步聲")] public AudioClip walking;
    [Tooltip("緊張呼吸聲")] public AudioClip strained_Breathing;
    [Tooltip("手電筒開關聲")] public AudioClip flashlight_Switch_Sound;
    [Tooltip("鬼影出現聲")] public AudioClip ghosting_Sound;
    [Tooltip("鬼魂逃跑")] public AudioClip ghost_Escape;
    [Tooltip("門縫鬼影")] public AudioClip ghostIn_The_Door;
    #endregion

    // 房間音效
    [SerializeField, Header("房間")] AudioSource[] roomSound;
    #region 房間  
    [SerializeField, Tooltip("開關燈")] AudioClip light_Switch_Sound;
    [SerializeField, Tooltip("抽屜")] AudioClip drawer_Opening_Sound;
    [SerializeField, Tooltip("床")] AudioClip getting_Out_Of_Bed;
    [SerializeField, Tooltip("衣櫃")] AudioClip the_Sound_Of_Opening_Wardrobes_And_Doors;
    [SerializeField, Tooltip("鑰匙")] AudioClip tet_Sound_Of_Get_The_Key;
    #endregion

    // 客廳音效
    [SerializeField, Header("客廳")] AudioSource[] livingRoomSound;
    #region 客廳
    [SerializeField, Tooltip("金紙")] AudioClip[] gold_Paper;
    [SerializeField, Tooltip("時鐘")] AudioClip clock;
    [SerializeField, Tooltip("鋼琴")] AudioClip piano;
    [SerializeField, Tooltip("孝簾")] AudioClip filial_Piety_Curtain;
    [SerializeField, Tooltip("佛歌")] public AudioClip buddhist_Song;
    [SerializeField, Tooltip("佛歌中斷")] AudioClip buddhist_Song_Stop;
    #endregion

    // 客廳加廚房音效
    [SerializeField, Header("客廳加廚房")] AudioSource livingRoomPlusKitchen;
    #region 客廳&廚房
    [SerializeField, Tooltip("蠟燭燃燒")] public AudioClip candle_Burning;
    [SerializeField, Tooltip("蠟燭吹襲熄聲")] public AudioClip candle_Blowing_Sound;
    #endregion

    // 門音效
    [SerializeField, Header("門")] AudioSource doorSound;
    #region 客廳+廁所
    [SerializeField, Tooltip("門解鎖聲")] AudioClip door_Unlock_Sound;
    [SerializeField, Tooltip("關門聲")] AudioClip door_Slam;
    [SerializeField, Tooltip("開門聲")] AudioClip door_Opening;
    #endregion

    // 廚房音效
    [SerializeField, Header("廚房")] AudioSource footRiceSound;
    #region 廚房
    [SerializeField, Tooltip("腳尾飯")] AudioClip sound_Of_Something_Falling;
    #endregion

    // 廁所音效
    [SerializeField, Header("廁所")] AudioSource bathroomSound;
    #region 廁所
    [SerializeField, Tooltip("水滴聲")] AudioClip dripping_Sound;
    [SerializeField, Tooltip("鬼手抓玩家聲")] AudioClip ghost_Hand_Catch_Player_Sound;
    [SerializeField, Tooltip("墜落聲")] AudioClip falling_Sound;
    [SerializeField, Tooltip("墜落後黑畫面")] AudioClip black_Screen_After_Fall;
    [SerializeField, Tooltip("轉水龍頭聲")] AudioClip turn_The_Tap;
    [SerializeField, Tooltip("緊湊敲門聲")] public AudioClip emergency_Knock_On_The_Door;
    #endregion

    // 環境/其他音效
    [SerializeField, Header("環境/其他")] AudioSource environmentOtherSound;
    #region 環境&其他
    [SerializeField, Tooltip("白噪音")] AudioClip white_Noise;
    [SerializeField, Tooltip("選單背景音樂")] AudioClip menu_Background_Music;
    [SerializeField, Tooltip("恐怖白噪音")] AudioClip horror_White_Noise;
    [SerializeField, Tooltip("遊戲開始")] AudioClip games_Start;
    [SerializeField, Tooltip("恐怖開始")] AudioClip horror_Start;
    [SerializeField, Tooltip("高音小提琴聲")] AudioClip soprano_Violin;
    [SerializeField, Tooltip("進入場景聲")] AudioClip enter_Scene_Sound;
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
                return ghosting_Sound_Special;
            case "mirror_Breaking_Sound":
                return mirror_Breaking_Sound;
            case "get_Item_Sound":
                return get_Item_Sound;
            case "falling_To_Black_Screen_Sound":
                return falling_To_Black_Screen_Sound;
            case "the_sound_of_something_moving":
                return the_sound_of_something_moving;
            case "strange_noises_keep_coming":
                return strange_noises_keep_coming;
            case "grandma_Starts_Walking":
                return grandma_Starts_Walking;
            case "grandma_StrangeVoice":
                return grandma_StrangeVoice;
            case "body_Twisting_Sound":
                return body_Twisting_Sound;
            case "muffled_Vocals":
                return muffled_Vocals;
            case "walking":
                return walking;
            case "strained_Breathing":
                return strained_Breathing;
            case "flashlight_Switch_Sound":
                return flashlight_Switch_Sound;
            case "ghosting_Sound":
                return ghosting_Sound;
            case "ghost_Escape":
                return ghost_Escape;
            case "ghostIn_The_Door":
                return ghostIn_The_Door;
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
            case "buddhist_Song_Stop":
                return buddhist_Song_Stop;
            case "candle_Burning":
                return candle_Burning;
            case "candle_Blowing_Sound":
                return candle_Blowing_Sound;
            case "door_Unlock_Sound":
                return door_Unlock_Sound;
            case "door_Slam":
                return door_Slam;
            case "door_Opening":
                return door_Opening;
            case "sound_Of_Something_Falling":
                return sound_Of_Something_Falling;
            case "dripping_Sound":
                return dripping_Sound;
            case "ghost_Hand_Catch_Player_Sound":
                return ghost_Hand_Catch_Player_Sound;
            case "falling_Sound":
                return falling_Sound;
            case "black_Screen_After_Fall":
                return black_Screen_After_Fall;
            case "turn_The_Tap":
                return turn_The_Tap;
            case "emergency_Knock_On_The_Door":
                return emergency_Knock_On_The_Door;
            case "white_Noise":
                return white_Noise;
            case "menu_Background_Music":
                return menu_Background_Music;
            case "horror_White_Noise":
                return horror_White_Noise;
            case "games_Start":
                return games_Start;
            case "horror_Start":
                return horror_Start;
            case "soprano_Violin":
                return soprano_Violin;
            case "enter_Scene_Sound":
                return enter_Scene_Sound;
            case "ui_Context":
                return ui_Context;
            case "player_Walking":
                return walkingSound;

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