using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class LotusGameManager : MonoBehaviour
{
    [SerializeField][Header("蓮花紙當前階段")] bool[] bLotusState;
    [SerializeField][Header("蓮花紙 - 物件")] GameObject[] LotusPaperObj;
    [SerializeField][Header("蓮花紙 - 動畫師")] Animator[] LotusPaperAni;
    [SerializeField][Header("蓮花紙 - 動畫片段")] AnimationClip[] LotusPaperAniClip;
    [SerializeField][Header("提示按鈕 - 物件")] GameObject HintObj;
    [SerializeField][Header("上下左右 - 圖片")] Sprite[] HintSprite;

    Image HintImg;
    RectTransform HintRectTf;
    int iAllLotusCount;
    AnimatorStateInfo LotusState;

    readonly string[] strLotusAniTriggerName = new string[30]
{
        "State_1",
        "State_2",
        "State_3",
        "State_4",
        "State_5",
        "State_6",
        "State_7-1",
        "State_7-2",
        "State_7-3",
        "State_7-4",
        "State_7-5",
        "State_7-6",
        "State_7-7",
        "State_7-8",
        "State_7-9",
        "State_7-10",
        "State_7-11",
        "State_7-12",
        "State_7-13",
        "State_7-14",
        "State_7-15",
        "State_7-16",
        "State_7-17",
        "State_7-18",
        "State_7-19",
        "State_7-20",
        "State_7-21",
        "State_7-22",
        "State_7-23",
        "State_7-24",
};

    void Awake()
    {
        HintRectTf = HintObj.GetComponent<RectTransform>();
        HintImg = HintObj.GetComponent<Image>();
    }

    void Start()
    {
        iAllLotusCount = 30;

        for (int index = 0; index < iAllLotusCount; index++)
            bLotusState[index] = false;

        bLotusState[0] = true;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.W))
            PlayLotusAni(KeyCode.W);

        if (Input.GetKeyDown(KeyCode.A))
            PlayLotusAni(KeyCode.A);

        if (Input.GetKeyDown(KeyCode.S))
            PlayLotusAni(KeyCode.S);

        if (Input.GetKeyDown(KeyCode.D))
            PlayLotusAni(KeyCode.D);
    }

    void PlayLotusAni(KeyCode r_key)
    {
        switch (r_key)
        {
            case KeyCode.W:
                if (bLotusState[0])
                {
                    StartCoroutine(ProcessAnimator(HintSprite[2], LotusPaperAni[0], LotusPaperAniClip[0], strLotusAniTriggerName[0], 0));
                    AUDManager.instance.PlayerLotusPaperSFX();
                }
                else if (bLotusState[3])
                {
                    StartCoroutine(ProcessAnimator(HintSprite[2], LotusPaperAni[3], LotusPaperAniClip[3], strLotusAniTriggerName[3], 3)); AUDManager.instance.PlayerLotusPaperSFX();
                }
                else if (bLotusState[9])
                {
                    StartCoroutine(ProcessAnimator(HintSprite[3], LotusPaperAni[6], LotusPaperAniClip[9], strLotusAniTriggerName[9], 9)); AUDManager.instance.PlayerLotusPaperSFX();
                }
                else if (bLotusState[13])
                {
                    StartCoroutine(ProcessAnimator(HintSprite[3], LotusPaperAni[6], LotusPaperAniClip[13], strLotusAniTriggerName[13], 13)); AUDManager.instance.PlayerLotusPaperSFX();
                }
                else if (bLotusState[17])
                {
                    StartCoroutine(ProcessAnimator(HintSprite[3], LotusPaperAni[6], LotusPaperAniClip[17], strLotusAniTriggerName[17], 17)); AUDManager.instance.PlayerLotusPaperSFX();
                }
                else if (bLotusState[21])
                {
                    StartCoroutine(ProcessAnimator(HintSprite[3], LotusPaperAni[6], LotusPaperAniClip[21], strLotusAniTriggerName[21], 21)); AUDManager.instance.PlayerLotusPaperSFX();
                }
                else if (bLotusState[25])
                {
                    StartCoroutine(ProcessAnimator(HintSprite[0], LotusPaperAni[6], LotusPaperAniClip[25], strLotusAniTriggerName[25], 25)); AUDManager.instance.PlayerLotusPaperSFX();
                }
                else if (bLotusState[26])
                {
                    StartCoroutine(ProcessAnimator(HintSprite[3], LotusPaperAni[6], LotusPaperAniClip[26], strLotusAniTriggerName[26], 26)); AUDManager.instance.PlayerLotusPaperSFX();
                }
                break;
            case KeyCode.A:
                if (bLotusState[2])
                {
                    StartCoroutine(ProcessAnimator(HintSprite[0], LotusPaperAni[2], LotusPaperAniClip[2], strLotusAniTriggerName[2], 2)); AUDManager.instance.PlayerLotusPaperSFX();
                }
                else if (bLotusState[8])
                {
                    StartCoroutine(ProcessAnimator(HintSprite[0], LotusPaperAni[6], LotusPaperAniClip[8], strLotusAniTriggerName[8], 8)); AUDManager.instance.PlayerLotusPaperSFX();
                }
                else if (bLotusState[12])
                {
                    StartCoroutine(ProcessAnimator(HintSprite[0], LotusPaperAni[6], LotusPaperAniClip[12], strLotusAniTriggerName[12], 12)); AUDManager.instance.PlayerLotusPaperSFX();
                }
                else if (bLotusState[16])
                {
                    StartCoroutine(ProcessAnimator(HintSprite[0], LotusPaperAni[6], LotusPaperAniClip[16], strLotusAniTriggerName[16], 16)); AUDManager.instance.PlayerLotusPaperSFX();
                }
                else if (bLotusState[20])
                {
                    StartCoroutine(ProcessAnimator(HintSprite[0], LotusPaperAni[6], LotusPaperAniClip[20], strLotusAniTriggerName[20], 20)); AUDManager.instance.PlayerLotusPaperSFX();
                }
                else if (bLotusState[24])
                {
                    StartCoroutine(ProcessAnimator(HintSprite[0], LotusPaperAni[6], LotusPaperAniClip[24], strLotusAniTriggerName[24], 24)); AUDManager.instance.PlayerLotusPaperSFX();
                }
                else if (bLotusState[29])
                    StartCoroutine(ProcessAnimator(HintSprite[1], LotusPaperAni[6], LotusPaperAniClip[29], strLotusAniTriggerName[29], 29)); AUDManager.instance.PlayerLotusPaperSFX();
                break;
            case KeyCode.S:
                if (bLotusState[1])
                {
                    StartCoroutine(ProcessAnimator(HintSprite[1], LotusPaperAni[1], LotusPaperAniClip[1], strLotusAniTriggerName[1], 1)); AUDManager.instance.PlayerLotusPaperSFX();
                }
                else if (bLotusState[4])
                {
                    StartCoroutine(ProcessAnimator(HintSprite[3], LotusPaperAni[4], LotusPaperAniClip[4], strLotusAniTriggerName[4], 4)); AUDManager.instance.PlayerLotusPaperSFX();
                }
                else if (bLotusState[7])
                {
                    StartCoroutine(ProcessAnimator(HintSprite[1], LotusPaperAni[6], LotusPaperAniClip[7], strLotusAniTriggerName[7], 7)); AUDManager.instance.PlayerLotusPaperSFX();
                }
                else if (bLotusState[11])
                {
                    StartCoroutine(ProcessAnimator(HintSprite[1], LotusPaperAni[6], LotusPaperAniClip[11], strLotusAniTriggerName[11], 11)); AUDManager.instance.PlayerLotusPaperSFX();
                }
                else if (bLotusState[15])
                {
                    StartCoroutine(ProcessAnimator(HintSprite[1], LotusPaperAni[6], LotusPaperAniClip[15], strLotusAniTriggerName[15], 15)); AUDManager.instance.PlayerLotusPaperSFX();
                }
                else if (bLotusState[19])
                {
                    StartCoroutine(ProcessAnimator(HintSprite[1], LotusPaperAni[6], LotusPaperAniClip[19], strLotusAniTriggerName[19], 19)); AUDManager.instance.PlayerLotusPaperSFX();
                }
                else if (bLotusState[23])
                {
                    StartCoroutine(ProcessAnimator(HintSprite[1], LotusPaperAni[6], LotusPaperAniClip[23], strLotusAniTriggerName[23], 23)); AUDManager.instance.PlayerLotusPaperSFX();
                }
                else if (bLotusState[28])
                    StartCoroutine(ProcessAnimator(HintSprite[1], LotusPaperAni[6], LotusPaperAniClip[28], strLotusAniTriggerName[28], 28)); AUDManager.instance.PlayerLotusPaperSFX();
                break;
            case KeyCode.D:
                if (bLotusState[5])
                {
                    StartCoroutine(ProcessAnimator(HintSprite[3], LotusPaperAni[5], LotusPaperAniClip[5], strLotusAniTriggerName[5], 5)); AUDManager.instance.PlayerLotusPaperSFX();
                }
                else if (bLotusState[6])
                {
                    StartCoroutine(ProcessAnimator(HintSprite[2], LotusPaperAni[6], LotusPaperAniClip[6], strLotusAniTriggerName[6], 6)); AUDManager.instance.PlayerLotusPaperSFX();
                }
                else if (bLotusState[10])
                {
                    StartCoroutine(ProcessAnimator(HintSprite[2], LotusPaperAni[6], LotusPaperAniClip[10], strLotusAniTriggerName[10], 10)); AUDManager.instance.PlayerLotusPaperSFX();
                }
                else if (bLotusState[14])
                {
                    StartCoroutine(ProcessAnimator(HintSprite[2], LotusPaperAni[6], LotusPaperAniClip[14], strLotusAniTriggerName[14], 14)); AUDManager.instance.PlayerLotusPaperSFX();
                }
                else if (bLotusState[18])
                {
                    StartCoroutine(ProcessAnimator(HintSprite[2], LotusPaperAni[6], LotusPaperAniClip[18], strLotusAniTriggerName[18], 18)); AUDManager.instance.PlayerLotusPaperSFX();
                }
                else if (bLotusState[22])
                {
                    StartCoroutine(ProcessAnimator(HintSprite[2], LotusPaperAni[6], LotusPaperAniClip[22], strLotusAniTriggerName[22], 22)); AUDManager.instance.PlayerLotusPaperSFX();
                }
                else if (bLotusState[27])
                    StartCoroutine(ProcessAnimator(HintSprite[2], LotusPaperAni[6], LotusPaperAniClip[27], strLotusAniTriggerName[27], 27)); AUDManager.instance.PlayerLotusPaperSFX();
                break;
            default:
                break;
        }
    }

    IEnumerator ProcessAnimator(Sprite sprite, Animator ani, AnimationClip clip, string strTriggerName, int iStateIndex)
    {
        HintObj.SetActive(false);
        HintImg.sprite = sprite;

        ani.SetTrigger(strTriggerName);
        AUDManager.instance.PlayerLotusPaperSFX();


        yield return new WaitForSeconds(clip.length + 0.2f);

        CheckAniState(ani, strTriggerName, iStateIndex);
    }

    void CheckAniState(Animator ani, string strTriggerName, int iStateIndex)
    {
        LotusState = ani.GetCurrentAnimatorStateInfo(0);

        if (bLotusState[29])
        {
            GameManager gm = GameObject.Find("GameManager").GetComponent<GameManager>();
            gm.SendMessage("ExitLotusGame");
            GlobalDeclare.bLotusGameComplete = true;
            return;
        }

        if (LotusState.IsName(strTriggerName) && LotusState.normalizedTime >= 1.0f)
        {
            bLotusState[iStateIndex] = false;
            bLotusState[iStateIndex + 1] = true;
        }

        for (int index = 1; index < 7; index++)
        {
            if (bLotusState[index])
            {
                LotusPaperObj[index - 1].transform.position = new Vector3(0, 0, 1);
                LotusPaperObj[index].transform.position = Vector3.zero;
            }
        }

        HintRectTf.anchoredPosition = UIHintPosition(iStateIndex + 1);
        HintObj.SetActive(true);
    }

    Vector2 UIHintPosition(int iStateIndex)
    {
        return iStateIndex switch
        {
            0 => new Vector2(0, 400),      // W
            1 => new Vector2(0, -400),     // S
            2 => new Vector2(-500, 0),     // A
            3 => new Vector2(0, 400),      // W
            4 => new Vector2(0, -400),     // S
            5 => new Vector2(500, 0),      // D
            6 => new Vector2(500, 0),      // D
            7 => new Vector2(0, -400),     // S
            8 => new Vector2(-500, 0),     // A
            9 => new Vector2(0, 400),      // W
            10 => new Vector2(300, -300),  // D
            11 => new Vector2(-300, -300), // S
            12 => new Vector2(-300, 300),  // A
            13 => new Vector2(300, 300),   // W
            14 => new Vector2(500, 0),     // D
            15 => new Vector2(0, -400),    // S
            16 => new Vector2(-500, 0),    // A
            17 => new Vector2(0, 400),     // W
            18 => new Vector2(300, -300),  // D
            19 => new Vector2(-300, -300), // S
            20 => new Vector2(-300, 300),  // A
            21 => new Vector2(300, 300),   // W
            22 => new Vector2(500, 0),     // D
            23 => new Vector2(0, -400),    // S
            24 => new Vector2(-500, 0),    // A
            25 => new Vector2(0, 400),     // W
            26 => new Vector2(300, 300),   // W
            27 => new Vector2(300, -300),  // D
            28 => new Vector2(-300, -300), // S
            29 => new Vector2(-300, 300),  // A
            _ => new Vector2(0, 0),
        };
    }
}