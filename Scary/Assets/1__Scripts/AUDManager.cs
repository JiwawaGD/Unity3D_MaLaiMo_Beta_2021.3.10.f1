using UnityEngine;
using UnityEngine.Audio;
using System.Collections;
using Fungus;
using OldBrickHouse;

public class AUDManager : MonoBehaviour
{
    [SerializeField] AudioMixer audioMixer;
    public static AUDManager instance;

    [SerializeField] AudioSource mainAudioSource;
    [SerializeField] AudioSource ScendAudioSource;
    [SerializeField] GameObject playerObj;

    [SerializeField, Header("玩家聲音效")] AudioSource PlayerSound;
    [SerializeField, Header("人物/物品聲音效")] AudioSource grandmaSound;
    #region 人物/物品聲 
    [Tooltip("東西竄動的聲音")] public AudioClip the_sound_of_something_moving;
    [Tooltip("廁所有奇怪持續的聲音")] public AudioClip strange_noises_keep_coming;
    [Tooltip("廁所有奇怪持續的聲音")] public AudioClip strange_noises_in_the_toilet;
    [Tooltip("奶奶開始向前")] public AudioClip grandma_Starts_Walking;
    [Tooltip("奶奶詭異聲")] public AudioClip grandma_StrangeVoice;
    [Tooltip("扭動身體的音效")] public AudioClip body_Twisting_Sound;
    [Tooltip("模糊不清的人聲音")] public AudioClip muffled_Vocals;
    [Tooltip("腳步聲")] public AudioClip walking;
    [Tooltip("緊張呼吸聲")] public AudioClip strained_Breathing;
    [Tooltip("手電筒開關聲")] public AudioClip flashlight_Switch_Sound;
    [Tooltip("鬼影出現聲")] public AudioClip ghosting_Sound;
    [Tooltip("鬼影音效")] public AudioClip ghost_Sound;
    [Tooltip("鬼魂逃跑")] public AudioClip ghost_Escape;
    [Tooltip("門縫鬼影")] public AudioClip ghostIn_The_Door;
    #endregion  

    [SerializeField, Header("房間")] AudioSource[] roomSound;
    #region 房間  
    [SerializeField, Tooltip("開關燈")] AudioClip light_Switch_Sound;
    [SerializeField, Tooltip("抽屜")] AudioClip drawer_Opening_Sound;
    [SerializeField, Tooltip("床")] AudioClip getting_Out_Of_Bed;
    [SerializeField, Tooltip("衣櫃")] AudioClip the_Sound_Of_Opening_Wardrobes_And_Doors;
    [SerializeField, Tooltip("鑰匙")] AudioClip tet_Sound_Of_Get_The_Key;
    #endregion

    [SerializeField, Header("客廳")] AudioSource[] livingRoomSound;
    #region 客廳
    [SerializeField, Tooltip("金紙")] AudioClip[] gold_Paper;
    [SerializeField, Tooltip("時鐘")] AudioClip clock;
    [SerializeField, Tooltip("鋼琴")] AudioClip piano;
    [SerializeField, Tooltip("孝簾")] AudioClip filial_Piety_Curtain;
    [SerializeField, Tooltip("佛歌")] AudioClip buddhist_Song;
    [SerializeField, Tooltip("佛歌中斷")] AudioClip buddhist_Song_Stop;
    #endregion

    [SerializeField, Header("客廳加廚房")] AudioSource livingRoomPlusKitchen;
    #region 客廳&廚房
    [SerializeField, Tooltip("蠟燭燃燒")] AudioClip candle_Burning;
    [SerializeField, Tooltip("蠟燭吹襲熄聲")] AudioClip candle_Blowing_Sound;
    #endregion

    [SerializeField, Header("門")] AudioSource doorSound;
    #region 客廳+廁所
    [SerializeField, Tooltip("門解鎖聲")] AudioClip door_Unlock_Sound;
    [SerializeField, Tooltip("關門聲")] AudioClip door_Slam;
    [SerializeField, Tooltip("開門聲")] AudioClip door_Opening;
    #endregion

    [SerializeField, Header("廚房")] AudioSource footRiceSound;
    #region 廚房
    [SerializeField, Tooltip("腳尾飯")] AudioClip sound_Of_Something_Falling;
    #endregion

    [SerializeField, Header("廁所")] AudioSource bathroomSound;
    #region 廁所
    [SerializeField, Tooltip("水滴聲")] AudioClip dripping_Sound;
    [SerializeField, Tooltip("鬼手抓玩家聲")] AudioClip ghost_Hand_Catch_Player_Sound;
    [SerializeField, Tooltip("墜落聲")] AudioClip falling_Sound;
    [SerializeField, Tooltip("墜落後黑畫面")] AudioClip black_Screen_After_Fall;
    [SerializeField, Tooltip("轉水龍頭聲")] AudioClip turn_The_Tap;
    [SerializeField, Tooltip("緊湊敲門聲")] public AudioClip emergency_Knock_On_The_Door;
    #endregion

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
    [SerializeField, Tooltip("墜落轉黑畫面聲")] AudioClip falling_To_Black_Screen_Sound;
    #endregion

    public const string MUSIC_KEY = "musicVolume";
    public const string SFX_KEY = "sfxVolume";

    private void Awake()
    {
        mainAudioSource = GetComponent<AudioSource>();
        Transform childTransform = transform.Find("SecondAudioSource");
        if (childTransform != null)
        {
            // 使用 GetComponent 方法獲取子物件上的 AudioSource 元件
            ScendAudioSource = childTransform.GetComponent<AudioSource>();
        }
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private IEnumerator RestoreVolume(float originalVolume)
    {
        yield return new WaitForSeconds(drawer_Opening_Sound.length); // 等待音效播放完畢

        mainAudioSource.volume = originalVolume; // 恢復原始音量
    }
    public void OpenTheDrawerSFX()
    {
        float originalVolume = mainAudioSource.volume;
        mainAudioSource.volume = originalVolume * 0.5f;
        mainAudioSource.PlayOneShot(drawer_Opening_Sound);

        StartCoroutine(RestoreVolume(originalVolume));
    }

    public void GetTheKeySFX()
    {
        ScendAudioSource.PlayOneShot(tet_Sound_Of_Get_The_Key);
    }

    public void PlayerDoorOpenSFX()
    {
        mainAudioSource.PlayOneShot(door_Opening);
    }

    public void PlayerDoorLockSFX()
    {
        mainAudioSource.PlayOneShot(door_Unlock_Sound);
    }

    public void PlayerLotusPaperSFX()
    {
        mainAudioSource.PlayOneShot(gold_Paper[Random.Range(0, 2)]);
    }

    public void PlayerLightSwitchSFX()
    {
        mainAudioSource.PlayOneShot(light_Switch_Sound);
    }

    public void PlayerFlashlighSFX()
    {
        mainAudioSource.PlayOneShot(flashlight_Switch_Sound);
    }

    public void PlayerGrandmaRushSFX()
    {
        mainAudioSource.PlayOneShot(grandma_Starts_Walking);
    }

    public void PlayerGameEventSFX()
    {
        mainAudioSource.PlayOneShot(ui_Context);
    }

    public void PlayerWhiteTentSFX()
    {
        mainAudioSource.PlayOneShot(filial_Piety_Curtain);
    }

    public void EmergencyKnockOnTheDoor()
    {
        mainAudioSource.PlayOneShot(emergency_Knock_On_The_Door);
    }

    public void CloseDoor()
    {
        mainAudioSource.PlayOneShot(door_Slam);
    }

    public void BodyTwistingSound()
    {
        mainAudioSource.PlayOneShot(body_Twisting_Sound);
    }

    public void ThereIsAStrangeContinuousSoundInTheToilet()
    {
        mainAudioSource.PlayOneShot(strange_noises_keep_coming);
    }

    public void StrangeNoisesInTheToilet()
    {
        mainAudioSource.PlayOneShot(strange_noises_in_the_toilet);
    }

    public void TheSoundOfSomethingMoving()
    { 
        mainAudioSource.PlayOneShot(the_sound_of_something_moving);
    }

    public void GrandmaStrangeVoice()
    {
        mainAudioSource.PlayOneShot(grandma_StrangeVoice);
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