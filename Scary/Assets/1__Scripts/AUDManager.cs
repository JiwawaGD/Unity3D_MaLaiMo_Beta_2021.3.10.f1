using UnityEngine;
using UnityEngine.Audio;

public class AUDManager : MonoBehaviour
{
    [SerializeField] AudioMixer audioMixer;
    public static AUDManager instance;

    //[SerializeField] AudioSource mainAudioSource;

    [SerializeField, Header("���a�n����")] AudioSource PlayerSound;
    [SerializeField, Header("�H��/���~�n����")] AudioSource grandmaSound;
    #region �H��/���~�n    
    [SerializeField, Tooltip("�����}�l�V�e")] public AudioClip grandma_Starts_Walking;
    [SerializeField, Tooltip("�����޲��n")] public AudioClip grandma_StrangeVoice;
    [SerializeField, Tooltip("��ʨ��骺����")] public AudioClip body_Twisting_Sound;
    [SerializeField, Tooltip("�ҽk���M���H�n��")] public AudioClip muffled_Vocals;
    //[SerializeField,Tooltip("�}�B�n")] 
    public AudioClip walking;
    [SerializeField, Tooltip("��i�I�l�n")] public AudioClip strained_Breathing;
    [SerializeField, Tooltip("��q���}���n")] public AudioClip flashlight_Switch_Sound;
    [SerializeField, Tooltip("���v�X�{�n")] public AudioClip ghosting_Sound;
    [SerializeField, Tooltip("���v����")] public AudioClip ghost_Sound;
    [SerializeField, Tooltip("����k�]")] public AudioClip ghost_Escape;
    [SerializeField, Tooltip("���_���v")] public AudioClip ghostIn_The_Door;
    #endregion  

    [SerializeField, Header("�ж�")] AudioSource[] roomSound;
    #region �ж�  
    [SerializeField, Tooltip("�}���O")] AudioClip light_Switch_Sound;
    [SerializeField, Tooltip("��P")] AudioClip drawer_Opening_Sound;
    [SerializeField, Tooltip("��")] AudioClip getting_Out_Of_Bed;
    [SerializeField, Tooltip("���d")] AudioClip The_Sound_Of_Opening_Wardrobes_And_Doors;
    #endregion

    [SerializeField, Header("���U")] AudioSource[] livingRoomSound;
    #region ���U
    [SerializeField, Tooltip("����")] AudioClip[] gold_Paper;
    [SerializeField, Tooltip("����")] AudioClip clock;
    [SerializeField, Tooltip("���^")] AudioClip piano;
    [SerializeField, Tooltip("��î")] AudioClip filial_Piety_Curtain;
    [SerializeField, Tooltip("��q")] AudioClip buddhist_Song;
    [SerializeField, Tooltip("��q���_")] AudioClip buddhist_Song_Stop;
    #endregion

    [SerializeField, Header("���U�[�p��")] AudioSource livingRoomPlusKitchen;
    #region ���U&�p��
    [SerializeField, Tooltip("����U�N")] AudioClip candle_Burning;
    [SerializeField, Tooltip("����jŧ���n")] AudioClip candle_Blowing_Sound;
    #endregion

    [SerializeField, Header("��")] AudioSource doorSound;
    #region ���U+�Z��
    [SerializeField, Tooltip("�������n")] AudioClip door_Unlock_Sound;
    [SerializeField, Tooltip("�����n")] AudioClip door_Slam;
    [SerializeField, Tooltip("�}���n")] AudioClip door_Opening;
    #endregion

    [SerializeField, Header("�p��")] AudioSource footRiceSound;
    #region �p��
    [SerializeField, Tooltip("�}����")] AudioClip sound_Of_Something_Falling;
    #endregion

    [SerializeField, Header("�Z��")] AudioSource bathroomSound;
    #region �Z��
    [SerializeField, Tooltip("���w�n")] AudioClip dripping_Sound;
    [SerializeField, Tooltip("����쪱�a�n")] AudioClip ghost_Hand_Catch_Player_Sound;
    [SerializeField, Tooltip("�Y���n")] AudioClip falling_Sound;
    [SerializeField, Tooltip("�Y����µe��")] AudioClip black_Screen_After_Fall;
    [SerializeField, Tooltip("����s�Y�n")] AudioClip turn_The_Tap;
    #endregion

    [SerializeField, Header("����/��L")] AudioSource environmentOtherSound;
    #region ����&��L
    [SerializeField, Tooltip("�վ���")] AudioClip white_Noise;
    [SerializeField, Tooltip("���I������")] AudioClip menu_Background_Music;
    [SerializeField, Tooltip("���ƥվ���")] AudioClip horror_White_Noise;
    [SerializeField, Tooltip("�C���}�l")] AudioClip games_Start;
    [SerializeField, Tooltip("���ƶ}�l")] AudioClip horror_Start;
    [SerializeField, Tooltip("�����p���^�n")] AudioClip soprano_Violin;
    [SerializeField, Tooltip("�i�J�����n")] AudioClip enter_Scene_Sound;
    [SerializeField, Tooltip("UI����")] AudioClip ui_Context;
    [SerializeField, Tooltip("�Y����µe���n")] AudioClip falling_To_Black_Screen_Sound;

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
    /// �s������
    /// </summary>
    public void LoadVolume()
    {
        float musicVolume = PlayerPrefs.GetFloat(MUSIC_KEY, 1f);
        float sfxVolume = PlayerPrefs.GetFloat(SFX_KEY, 1f);

        audioMixer.SetFloat(AudSetting.MIXER_MUSIC, Mathf.Log10(musicVolume) * 20);
        audioMixer.SetFloat(AudSetting.MIXER_SFX, Mathf.Log10(sfxVolume) * 20);
    }

}
