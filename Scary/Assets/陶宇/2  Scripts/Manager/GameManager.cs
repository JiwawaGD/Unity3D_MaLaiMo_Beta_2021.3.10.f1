using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField] [Header("玩家")] PlayerController playerCtrlr;

    [SerializeField] [Header("戶內傳送點")] Transform tfIndoorPos;
    [SerializeField] [Header("戶外傳送點")] Transform tfOutdoorPos;
    [SerializeField] [Header("鐵捲門物件")] Transform tfRollingDoor;

    GameObject goCanvas;
    Image imgUIBackGround;
    Image imgUIDisplay;
    Text txtTitle;
    Text txtIntroduce;
    Button ExitBtn;
    Text txtEnterGameHint;
    Button EnterGameBtn;
    [SerializeField] [Header("UI 圖片庫")] Sprite[] UISprite;

    public static bool m_bInUIView = false;
    public static bool m_bIsEnterGameView = false;
    public static bool m_bInCGAnimate = false;

    void Awake()
    {
        if (playerCtrlr == null)
            playerCtrlr = GameObject.Find("Player").GetComponent<PlayerController>();

        if (goCanvas == null)
            goCanvas = GameObject.Find("UI Canvas");

        imgUIBackGround = goCanvas.transform.GetChild(0).GetComponent<Image>();
        imgUIDisplay = goCanvas.transform.GetChild(1).GetComponent<Image>();
        txtTitle = goCanvas.transform.GetChild(2).GetComponent<Text>();
        txtIntroduce = goCanvas.transform.GetChild(3).GetComponent<Text>();
        ExitBtn = goCanvas.transform.GetChild(4).GetComponent<Button>();
        txtEnterGameHint = goCanvas.transform.GetChild(5).GetComponent<Text>();
        EnterGameBtn = goCanvas.transform.GetChild(6).GetComponent<Button>();
    }

    void Start()
    {
        GameEvent(GameEventID.Close_UI);

        ExitBtn.onClick.AddListener(() => ButtonFunction(ButtonEventID.UI_Back));
    }

    public void GameEvent(GameEventID _eventID)
    {
        switch (_eventID)
        {
            case GameEventID.Close_UI:
                UIState(UIItemID.Empty, false);
                ShowEnterGame(false);

                if (m_bInCGAnimate)
                    ProcessPlayerAnimator(GlobalDeclare.GetPlayerAnimateType().ToString());

                break;
            case GameEventID.S1_Photo_Frame:
                UIState(UIItemID.S1_Photo_Frame, true);
                m_bInCGAnimate = true;
                GlobalDeclare.SetPlayerAnimateType(PlayerAnimateType.Player_Turn_After_Photo_Frame);
                break;
            case GameEventID.S1_Grandma_Door_Open:
                ProcessAnimator("Grandma Door", "DoorOpen");
                break;
            case GameEventID.S1_Lotus_Paper:
                UIState(UIItemID.S1_Lotus_Paper, true);
                ShowEnterGame(true);
                break;
            case GameEventID.S1_Grandma_Dead_Body:
                UIState(UIItemID.S1_Grandma_Dead_Body, true);
                break;
            case GameEventID.S1_White_Tent:
                UIState(UIItemID.S1_White_Tent, true);
                ProcessAnimator("White Tent Temp", "White Tent Open");
                break;
        }
    }

    public void UIState(UIItemID r_ItemID, bool r_bEnable)
    {
        m_bInUIView = r_bEnable;
        playerCtrlr.m_bCanControl = !r_bEnable;
        playerCtrlr.SetCursor();

        goCanvas.SetActive(r_bEnable);
        ExitBtn.gameObject.SetActive(r_bEnable);
        imgUIBackGround.color = r_bEnable ? new Color(0, 0, 0, 0.95f) : new Color(0, 0, 0, 0);
        imgUIDisplay.color = r_bEnable ? new Color(1, 1, 1, 1) : new Color(1, 1, 1, 0);

        int iItemID = (int)r_ItemID;

        imgUIDisplay.sprite = UISprite[iItemID];
        txtTitle.text = GlobalDeclare.UITitle[iItemID];
        txtIntroduce.text = GlobalDeclare.UIIntroduce[iItemID];
    }

    public void ProcessAnimator(string r_sObject, string r_sTriggerName)
    {
        GameObject obj = GameObject.Find(r_sObject);
        Animator ani = obj.transform.GetComponent<Animator>();
        ani.SetTrigger(r_sTriggerName);
        obj.transform.GetComponent<ItemController>().HintState(false);
        obj.transform.GetComponent<ItemController>().b_isActive = false;
        obj = null;
        ani = null;
    }

    public void ProcessPlayerAnimator(string r_sAnimationName)
    {
        //playerCtrlr.tfPlayerCamera.localEulerAngles = new Vector3(0, 0, 0);

        Animation am = playerCtrlr.GetComponent<Animation>();
        am.Play(r_sAnimationName);
    }

    public void ShowEnterGame(bool r_bEnable)
    {
        EnterGameBtn.gameObject.SetActive(r_bEnable);
        txtEnterGameHint.gameObject.SetActive(r_bEnable);
        txtEnterGameHint.text = r_bEnable ? "---- 是否進入遊戲 ----" : "";
    }

    public void ButtonFunction(ButtonEventID _eventID)
    {
        switch (_eventID)
        {
            case ButtonEventID.UI_Back:
                GameEvent(GameEventID.Close_UI);
                break;
            case ButtonEventID.Enter_Game:
                GameEvent(GameEventID.Close_UI);
                break;
            default:
                break;
        }
    }
}