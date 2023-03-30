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
    [SerializeField] [Header("UI 圖片庫")] Sprite[] UISprite;

    Transform tf_player;

    public static bool m_bInUIView = false;

    void Awake()
    {
        if (playerCtrlr == null)
            playerCtrlr = GameObject.Find("Player").GetComponent<PlayerController>();

        tf_player = playerCtrlr.transform;

        if (goCanvas == null)
            goCanvas = GameObject.Find("UI Canvas");

        imgUIBackGround = goCanvas.transform.GetChild(0).GetComponent<Image>();
        imgUIDisplay = goCanvas.transform.GetChild(1).GetComponent<Image>();
        txtTitle = goCanvas.transform.GetChild(2).GetComponent<Text>();
        txtIntroduce = goCanvas.transform.GetChild(3).GetComponent<Text>();
    }

    void Start()
    {
        goCanvas.SetActive(false);
    }

    public void GameEvent(GameEventID _eventID)
    {
        switch (_eventID)
        {
            case GameEventID.Close_UI:
                UIState(UIItemID.Empty, false);
                break;
            case GameEventID.S1_Move_To_Indoor:
                tf_player.position = tfIndoorPos.position;
                break;
            case GameEventID.S1_Move_To_OutDoor:
                tf_player.position = tfOutdoorPos.position;
                break;
            case GameEventID.S1_RollingDoor_Up:
                tfRollingDoor.position += new Vector3(0, 4, 0);
                break;
            case GameEventID.S1_Photo_Frame:
                UIState(UIItemID.S1_Photo_Frame, true);
                break;
        }
    }

    public void UIState(UIItemID r_ItemID, bool r_bEnable)
    {
        m_bInUIView = r_bEnable;
        playerCtrlr.m_bCanControl = !r_bEnable;
        playerCtrlr.SetCursor();

        goCanvas.SetActive(r_bEnable);
        imgUIBackGround.color = r_bEnable ? new Color(0, 0, 0, 0.9f) : new Color(0, 0, 0, 0);
        imgUIDisplay.color = r_bEnable ? new Color(1, 1, 1, 1) : new Color(1, 1, 1, 0);

        int iItemID = (int)r_ItemID;

        imgUIDisplay.sprite = UISprite[iItemID];
        txtTitle.text = GlobalDeclare.UITitle[iItemID];
        txtIntroduce.text = GlobalDeclare.UIIntroduce[iItemID];
    }
}
