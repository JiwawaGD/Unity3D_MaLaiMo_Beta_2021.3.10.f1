using UnityEngine;
using UnityEngine.Audio;

public class AUDManager : MonoBehaviour
{
    [SerializeField] AudioMixer audioMixer;
    public static AUDManager instance;

    //[SerializeField] AudioSource mainAudioSource;

    [SerializeField, Header("玩家聲音效")] AudioSource PlayerSound;
    [SerializeField, Header("人物/物品聲音效")] AudioSource grandmaSound;
    #region 人物/物品聲    
    [SerializeField, Tooltip("奶奶開始向前")] public AudioClip grandma_Starts_Walking;
    [SerializeField, Tooltip("奶奶詭異聲")] public AudioClip grandma_StrangeVoice;
    [SerializeField, Tooltip("扭動身體的音效")] public AudioClip body_Twisting_Sound;
    [SerializeField, Tooltip("模糊不清的人聲音")] public AudioClip muffled_Vocals;
    //[SerializeField,Tooltip("腳步聲")] 
    public AudioClip walking;
    [SerializeField, Tooltip("緊張呼吸聲")] public AudioClip strained_Breathing;
    [SerializeField, Tooltip("手電筒開關聲")] public AudioClip flashlight_Switch_Sound;
    [SerializeField, Tooltip("鬼影出現聲")] public AudioClip ghosting_Sound;
    [SerializeField, Tooltip("鬼影音效")] public AudioClip ghost_Sound;
    [SerializeField, Tooltip("鬼魂逃跑")] public AudioClip ghost_Escape;
    [SerializeField, Tooltip("門縫鬼影")] public AudioClip ghostIn_The_Door;
    #endregion  

    [SerializeField, Header("房間")] AudioSource[] roomSound;
    #region 房間  
    [SerializeField, Tooltip("開關燈")] AudioClip light_Switch_Sound;
    [SerializeField, Tooltip("抽屜")] AudioClip drawer_Opening_Sound;
    [SerializeField, Tooltip("床")] AudioClip getting_Out_Of_Bed;
    [SerializeField, Tooltip("衣櫃")] AudioClip The_Sound_Of_Opening_Wardrobes_And_Doors;
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

    public void OpenTheDrawerSFX()
    {
        doorSound.PlayOneShot(drawer_Opening_Sound);
    }
    public void PlayerWalkSFX()
    {
        PlayerSound.PlayOneShot(walking);
    }
    public void PlayerDoorOpenSFX()
    {
        doorSound.PlayOneShot(door_Opening);
    }
    public void PlayerLotusPaperSFX()
    {
        PlayerSound.PlayOneShot(gold_Paper[Random.Range(0,2)]);
    }
    public void PlayerLightSwitchSFX()
    {
        roomSound[0].PlayOneShot(light_Switch_Sound);
    }
    public void PlayerFlashlighSFX()
    {
        PlayerSound.PlayOneShot(light_Switch_Sound);
    }
    public void PlayerGrandmaRushSFX()
    {
        grandmaSound.PlayOneShot(grandma_Starts_Walking);
    }
    public void PlayerGameEventSFX()
    {
        PlayerSound.PlayOneShot(ui_Context);
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
