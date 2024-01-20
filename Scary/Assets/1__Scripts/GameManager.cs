using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;
using UnityEngine.Rendering.HighDefinition;
using Fungus;
using ProgressP;
using DG.Tweening;

public partial class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public string CurrentDialogue;
    [Space]
    [SerializeField] Volume CameraVolume;
    [Header("Volume參數設定")]
    [SerializeField]
    float targetIntensity = 1f,
                     currentIntensity = 0.3f, changeSpeed = 1f;
    [SerializeField] GameObject[] taskListUi;
    [Space]
    [Header("物件旋轉參數設定")]
    bool isMoveingObject = false;    // 是否正在移動物件
    public Vector3 originalPosition;    // 原始位置
    public Quaternion originalRotation; // 原始旋轉

    [SerializeField] AUDManager audManager;
    // 音效管理器
    [Header("遊戲結束畫面UI")] public GameObject FinalUI;

    [SerializeField][Header("欲製物 - Schedule")] Text prefabs_Schedule;

    [Header("物件移動速度")] public float objSpeed;
    [Header("旋轉物件collider")] public Collider Ro_Cololider;
    [Header("旋轉物件功能")] public bool romanager;

    [Header("全域變數")] public Volume postProcessVolume;

    [Header("物件位置")] public GameObject itemObjTransform;

    [Header("生成後物件")] public GameObject[] RO_OBJ;
    [Header("儲存生成物件")] public int saveRotaObj;
    [Header("攝影棚畫面UI")] public GameObject StudioUI;
    [Header("旋轉物件使用燈關")] public Light Ro_Light;

    [Header("玩家")] public PlayerController playerCtrlr;
    [SerializeField][Header("對話程序")] DialogueManager[] DialogueObjects;
    [SerializeField][Header("設定頁面")] public GameObject settingObjects;
    [SerializeField][Header("S2_阿嬤相框Ro")] public GameObject S2_Photo_Frame_Obj;

    [SerializeField][Header("Video 撥放器")] VideoPlayer videoPlayer;
    [SerializeField][Header("Video - 阿嬤看螢幕")] VideoClip GrandmaVP;

    [SerializeField][Header("QRCode UI")] GameObject QRCodeUI;
    [SerializeField][Header("準心 UI")] GameObject CrosshairUI;

    int m_iGrandmaRushCount;
    Scene currentScene;

    ItemController TempItem;

    #region Canvas Zone
    [SerializeField] GameObject goCanvas;
    [SerializeField] Image imgUIBackGround;
    [SerializeField] Text txtTitle;

    [SerializeField] Image imgInstructions;
    [SerializeField] Text txtInstructions;
    [SerializeField] Text txtIntroduce;

    [SerializeField] Button ExitBtn;
    [SerializeField] Text txtEnterGameHint;
    [SerializeField] Button EnterGameBtn;

    #endregion

    #region Light Zone
    public GameObject goPhotoFrameLight;
    #endregion

    #region Static Boolean Zone
    public static bool m_bInUIView = false;
    public static bool m_bIsEnterGameView = false;
    public static bool m_bShowPlayerAnimate = false;
    public static bool m_bShowItemAnimate = false;
    public static bool m_bShowDialog = false;
    public static bool m_bSetPlayerViewLimit = false;
    public static bool m_bGrandmaRush = false;
    public static bool m_bReturnToBegin = false;
    public static bool m_bPlayLotusEnable = false;
    public static bool m_bToiletGhostHasShow = false;
    public static bool m_bWaitToiletGhostHandPush = false;
    #endregion

    #region Game Point
    bool bS1_TriggerFlashlight = false;
    bool bS1_TriggerGrandmaDoorLock = false;
    bool bS1_IsS1LightSwtichOK = false;

    bool bS2_TriggerLightSwitch = false;
    bool bS2_TriggerGrandmaDoorLock = false;
    bool bS2_TriggerLastAnimateAfterPhotoFrame = false;
    #endregion

    #region - All Scene Items -
    [Header("場景一物件")]
    [SerializeField][Header("S1_打翻前的腳尾飯")] GameObject S1_Rice_Funeral_Obj;
    [SerializeField][Header("S1_完好的相框")] GameObject S1_Photo_Frame_Obj;
    [SerializeField][Header("S1_破碎的相框")] GameObject S1_Photo_Frame_Has_Broken_Obj;
    [SerializeField][Header("S1_奶奶房間抽屜")] GameObject S1_Desk_Drawer_Obj;
    [SerializeField][Header("S1_還沒摺的蓮花紙")] GameObject S1_Lotus_Paper_Obj;
    [SerializeField][Header("S1_蓮花紙旁的蠟燭")] GameObject S1_Lotus_Candle_Obj;
    [SerializeField][Header("S1_摺好的紙蓮花")] GameObject S1_Finished_Lotus_Paper_Obj;
    [SerializeField][Header("S1_放紙蓮花的盤子")] GameObject S1_Lotus_Paper_Plate_Obj;

    [Header("場景二物件")]
    [SerializeField][Header("S2_鬼阿嬤")] GameObject S2_Grandma_Ghost_Obj;
    [SerializeField][Header("S2_廚房物件_狀態一")] GameObject S2_Furniture_State_1_Obj;
    [SerializeField][Header("S2_廚房物件_狀態二")] GameObject S2_Furniture_State_2_Obj;
    [SerializeField][Header("S2_躺在床上的奶奶屍體")] GameObject S2_Grandma_Deadbody_On_Table_Obj;
    [SerializeField][Header("S2_廁所鬼頭")] GameObject S2_Toilet_Door_GhostHead_Obj;
    [SerializeField][Header("S2_阿嬤相框")] GameObject S2_Photo_Frame_Obj_floor;
    [SerializeField][Header("S2_阿嬤哭聲撥放器")] GameObject S2_Grandma_Cry_Audio_Obj;
    [SerializeField][Header("S2_走廊門框")] GameObject S2_Corridor_Door_Frame_Obj;
    [SerializeField][Header("S2_取代走廊門框的牆壁")] GameObject S2_Wall_Replace_Door_Frame_Obj;
    #endregion

    #region - Empty Field => For Memory -
    BoxCollider TempBoxCollider;
    GameObject TempGameObject;
    #endregion

    bool bIsGameIntroducing = true;
    bool isPaused = false;
    bool isMouseEnabled = false;
    bool isUIOpen = false;
    bool bIsGameEnd = false;

    void Awake()
    {
        if (playerCtrlr == null)
            playerCtrlr = GameObject.Find("Player").GetComponent<PlayerController>();

        audManager = playerCtrlr.GetComponentInChildren<AUDManager>();

        if (goCanvas == null)
            goCanvas = GameObject.Find("UI Canvas");

        imgUIBackGround = goCanvas.transform.GetChild(0).GetComponent<Image>();  // 背景
        txtTitle = goCanvas.transform.GetChild(2).GetComponent<Text>(); // 標題

        imgInstructions = goCanvas.transform.GetChild(3).GetComponent<Image>();  // 說明圖示
        txtInstructions = goCanvas.transform.GetChild(3).GetComponentInChildren<Text>();    // 說明文字
        txtIntroduce = goCanvas.transform.GetChild(4).GetComponentInChildren<Text>();   // 介紹文字

        ExitBtn = goCanvas.transform.GetChild(5).GetComponent<Button>(); // 返回按鈕

        txtEnterGameHint = goCanvas.transform.GetChild(6).GetComponent<Text>(); // 進入遊戲提示
        EnterGameBtn = goCanvas.transform.GetChild(7).GetComponent<Button>();    // 進入遊戲按鈕

        TempItem = null;    // 暫存物件
        currentScene = SceneManager.GetActiveScene();   // 當前場景
        Ro_Light.enabled = false;   // 旋轉物件使用燈關
        StudioUI.SetActive(false);  // 攝影棚畫面UI
    }

    void Start()
    {
        GameEvent(GameEventID.Close_UI);
        DialogueObjects[(byte)Lv1_Dialogue.Begin].CallAction();
        ExitBtn.onClick.AddListener(() => ButtonFunction(ButtonEventID.UI_Back));   // 返回
        EnterGameBtn.onClick.AddListener(() => ButtonFunction(ButtonEventID.Enter_Game));   // 進入蓮花遊戲

        ShowHint(HintItemID.S1_Light_Switch);
        ShowHint(HintItemID.S1_Grandma_Room_Door_Lock);
        ShowHint(HintItemID.S1_Toilet_Door);

        // 尚未完成前情提要的串接，因此先在 Start 的地方跑動畫
        playerCtrlr.gameObject.GetComponent<Animation>().PlayQueued("Player_Wake_Up");

        SetCrosshairEnable(true);
    }

    void Update()
    {
        KeyboardCheck();

        if (isPaused && isMouseEnabled) // 暫停時啟用滑鼠
            MouseCheck();

        if (isUIOpen && Input.GetKeyDown(KeyCode.R))    // 按 R 鍵
            ButtonFunction(ButtonEventID.Enter_Game);   // 進入蓮花遊戲

        if (bIsGameEnd && Input.GetKeyDown(KeyCode.F9))
            ShowQRCode();
    }

    public void SetGameSetting()
    {
        SetCrosshairEnable(GlobalDeclare.bCrossHairEnable);
    }

    public void GameEvent(GameEventID r_eventID)
    {
        switch (r_eventID)
        {
            case GameEventID.Close_UI:
                UIState(UIItemID.Empty, false);
                ShowEnterGame(false);
                audManager.Play(1, "ui_Context", false);

                // UI 返回後執行玩家動畫
                if (m_bShowPlayerAnimate)
                    ProcessPlayerAnimator(GlobalDeclare.GetPlayerAnimateType().ToString());

                // UI 返回後執行 Item 動畫
                if (m_bShowItemAnimate)
                    ProcessAnimator(GlobalDeclare.GetItemAniObject(), GlobalDeclare.GetItemAniName());

                // 鎖定玩家視角旋轉
                if (m_bSetPlayerViewLimit)
                    SetPlayerViewLimit(true, GlobalDeclare.PlayerCameraLimit.GetPlayerCameraLimit());

                // 動畫
                if (bS2_TriggerLastAnimateAfterPhotoFrame)
                    LastAnimateAfterPhotoFrame();

                GameStateCheck();
                break;
            case GameEventID.S1_Light_Switch:
                S1_LightSwitch();
                break;
            case GameEventID.S1_Grandma_Room_Door_Lock:
                S1_GrandmaRoomDoorLock();
                break;
            case GameEventID.S1_Flashlight:
                S1_Flashlight();
                break;
            case GameEventID.S1_Desk_Drawer:
                S1_DeskDrawer();
                break;
            case GameEventID.S1_GrandmaRoomKey:
                S1_GrandmaRoomKey();
                break;
            case GameEventID.S1_Grandma_Door_Open:
                S1_GrandmaDoorOpen();
                break;
            case GameEventID.S1_Rice_Funeral:
                S1_RiceFuneral();
                break;
            case GameEventID.S1_Grandma_Pass_Door_After_RiceFurnel:
                S1_GrandmaPassDoorAfterRiceFurnel();
                break;
            case GameEventID.S1_White_Tent:
                S1_WhiteTent();
                break;
            case GameEventID.S1_Grandma_Dead_Body:
                S1_GrandmaDeadBody();
                break;
            case GameEventID.S1_Rice_Funeral_Spilled:
                S1_RiceFuneralSpilled();
                break;
            case GameEventID.S1_Photo_Frame:
                S1_PhotoFrameEvent();
                break;
            case GameEventID.S1_Photo_Frame_Has_Broken:
                S1_PhotoFrameHasBroken();
                break;
            case GameEventID.S1_Lotus_Paper:
                S1_LotusPaper();
                break;
            case GameEventID.S1_Finished_Lotus_Paper:
                S1_FinishedLotusPaper();
                break;
            case GameEventID.S1_Lotus_Paper_Plate:
                S1_LotusPaperPlate();
                break;
            case GameEventID.S1_Photo_Frame_Light_On:
                S1_PhotoFrameLightOn();
                break;
            case GameEventID.S1_Grandma_Rush:
                S1_GrandmaRush();
                break;
            case GameEventID.S1_Toilet_Door_Lock:
                S1_ToiletDoorLock();
                break;
            case GameEventID.S1_Toilet_Door_Open:
                S1_ToiletDoorOpen();
                break;
            case GameEventID.S1_Toilet_Ghost_Hide:
                S1_ToiletGhostHide();
                break;
            case GameEventID.S1_Toilet_Ghost_Hand_Push:
                S1_ToiletGhostHandPush();
                break;
            case GameEventID.S2_Light_Switch:
                S2_LightSwitch();
                break;
            case GameEventID.S2_Room_Door_Lock:
                S2_RoomDoorLock();
                break;
            case GameEventID.S2_FlashLight:
                S2_FlashLight();
                break;
            case GameEventID.S2_Side_Table:
                S2_SideTable();
                break;
            case GameEventID.S2_Room_Key:
                S2_RoomKey();
                break;
            case GameEventID.S2_Door_Knock_Stop:
                S2_DoorKnockStop();
                break;
            case GameEventID.S2_Grandma_Door_Open:
                S2_GrandmaDoorOpen();
                break;
            case GameEventID.S2_Grandma_Door_Close:
                S2_GrandmaDoorClose();
                break;
            case GameEventID.S2_Ghost_Pass_Door:
                S2_GhostPassDoor();
                break;
            case GameEventID.S2_Toilet_Door:
                S2_ToiletDoor();
                break;
            case GameEventID.S2_Rice_Funeral:
                S2_Rice_Funeral();
                break;
            case GameEventID.S2_Photo_Frame:
                S2_Photo_Frame();
                break;
        }
    }

    // 顯示眼睛 Hint 圖示
    public void ShowHint(HintItemID _ItemID)
    {
        switch (_ItemID)
        {
            case HintItemID.S1_Light_Switch:
                TempItem = GameObject.Find("Light_Switch").GetComponent<ItemController>();
                break;
            case HintItemID.S1_Grandma_Room_Door:
                TempItem = GameObject.Find("Grandma_Room_Door").GetComponent<ItemController>();
                TempItem.gameObject.layer = LayerMask.NameToLayer("InteractiveItem");
                TempItem.eventID = GameEventID.S1_Grandma_Door_Open;
                break;
            case HintItemID.S1_Flashlight:
                if (!bS1_TriggerGrandmaDoorLock || !bS1_IsS1LightSwtichOK)
                    return;
                TempItem = GameObject.Find("Flashlight").GetComponent<ItemController>();
                break;
            case HintItemID.S1_Desk_Drawer:
                if (!bS1_TriggerGrandmaDoorLock || !bS1_TriggerFlashlight)
                    return;
                TempItem = S1_Desk_Drawer_Obj.GetComponent<ItemController>();
                break;
            case HintItemID.S1_Grandma_Room_Key:
                TempItem = GameObject.Find("Grandma_Room_Key").GetComponent<ItemController>();
                break;
            case HintItemID.S1_Filial_Piety_Curtain:
                TempItem = GameObject.Find("Filial_Piety_Curtain").GetComponent<ItemController>();
                break;
            case HintItemID.S1_Lie_Grandma_Body:
                TempItem = GameObject.Find("Lie_Grandma_Body").GetComponent<ItemController>();
                break;
            case HintItemID.S1_Rice_Funeral:
                TempItem = GameObject.Find("Rice_Funeral").GetComponent<ItemController>();
                break;
            case HintItemID.S1_Lotus_Paper:
                TempItem = S1_Lotus_Paper_Obj.GetComponent<ItemController>();
                break;
            case HintItemID.S1_Finished_Lotus_Paper:
                TempItem = S1_Finished_Lotus_Paper_Obj.GetComponent<ItemController>();
                break;
            case HintItemID.S1_Lotus_Paper_Plate:
                TempItem = S1_Lotus_Paper_Plate_Obj.GetComponent<ItemController>();
                break;
            case HintItemID.S1_Grandma_Room_Door_Lock:
                TempItem = GameObject.Find("Grandma_Room_Door").GetComponent<ItemController>();
                break;
            case HintItemID.S1_Rice_Funeral_Spilled:
                TempItem = GameObject.Find("Rice_Funeral_Spilled").GetComponent<ItemController>();
                break;
            case HintItemID.S1_Photo_Frame_Has_Broken:
                TempItem = S1_Photo_Frame_Has_Broken_Obj.GetComponent<ItemController>();
                break;
            case HintItemID.S1_Photo_Frame:
                TempItem = S1_Photo_Frame_Obj.GetComponent<ItemController>();
                break;
            case HintItemID.S1_Toilet_Door:
                TempItem = GameObject.Find("Toilet_Door_Ghost").GetComponent<ItemController>();
                break;
            case HintItemID.S1_Toilet_GhostHand_Trigger:
                TempItem = GameObject.Find("Ghost_Hand_Trigger").GetComponent<ItemController>();
                break;
            case HintItemID.S2_Light_Switch:
                TempItem = GameObject.Find("S2_Light_Switch").GetComponent<ItemController>();
                break;
            case HintItemID.S2_Room_Door:
                TempItem = GameObject.Find("S2_Grandma_Room_Door").GetComponent<ItemController>();
                break;
            case HintItemID.S2_FlashLight:
                if (!bS2_TriggerLightSwitch || !bS2_TriggerGrandmaDoorLock)
                    return;

                TempItem = GameObject.Find("S2_FlashLight").GetComponent<ItemController>();
                break;
            case HintItemID.S2_Side_Table:
                TempItem = GameObject.Find("S2_Side_Table").GetComponent<ItemController>();
                break;
            case HintItemID.S2_Room_Key:
                TempItem = GameObject.Find("S2_Grandma_Room_Key").GetComponent<ItemController>();
                break;
            case HintItemID.S2_Grandma_Room_Door_Open:
                TempItem = GameObject.Find("S2_Grandma_Room_Door").GetComponent<ItemController>();
                TempItem.gameObject.layer = LayerMask.NameToLayer("InteractiveItem");
                TempItem.eventID = GameEventID.S2_Grandma_Door_Open;
                break;
            case HintItemID.S2_Rice_Funeral:
                TempItem = GameObject.Find("S2_Rice_Funeral").GetComponent<ItemController>();
                break;
            case HintItemID.S2_Photo_Frame:
                TempItem = S2_Photo_Frame_Obj.GetComponent<ItemController>();
                break;
            case HintItemID.S2_Toilet_Door:
                TempItem = GameObject.Find("S2_Toilet_Door_GhostHead").GetComponent<ItemController>();
                break;
        }

        TempItem.bActive = true;
        TempItem.SetHintable(true);
    }

    // 旋轉物件 (物件ID)     
    void ProcessRoMoving(int iIndex)
    {
        if (RO_OBJ[saveRotaObj] == null)
            return;
        Ro_Light.enabled = true;
        CameraVolume.enabled = true;
        isMoveingObject = true;  // 正在移動物件
        saveRotaObj = iIndex;   // 儲存物件  
        originalPosition = RO_OBJ[saveRotaObj].transform.position;  // 儲存物件位置
        originalRotation = RO_OBJ[saveRotaObj].transform.rotation;  // 儲存物件旋轉
        // Ro_Cololider = RO_OBJ[saveRotaObj].GetComponent<Collider>();    // 儲存物件碰撞器
        romanager = RO_OBJ[saveRotaObj].GetComponent<RotateObjDetect>().enabled = true; // 啟用旋轉物件碰撞器
    }

    // 顯示進入旋轉遊戲按鈕
    public void ShowObj(ObjItemID O_ItemID)  // 顯示物件 UI  (旋轉物件)  (物件ID)
    {
        StudioUI.SetActive(true);
        switch (O_ItemID)
        {
            case ObjItemID.S1_Rice_Funeral:
                RO_OBJ[saveRotaObj].transform.DOMove(
                    new Vector3(-27.815f, 1.809f, 8.32354f), 2);
                break;
            case ObjItemID.S1_Lotus_Paper:
                RO_OBJ[saveRotaObj].transform.DOMove(
                    new Vector3(-27.719f, 1.777f, 8.745541f), 2);
                break;
            case ObjItemID.S1_Photo_Frame:
                RO_OBJ[saveRotaObj].transform.DOMove(
                    new Vector3(-27.75f, 1.803f, 8.55254f), 2);
                break;
            case ObjItemID.S2_Photo_Frame:
                RO_OBJ[saveRotaObj].transform.DOMove(
                    new Vector3(-27.75f, 1.803f, 8.55254f), 2);
                break;
            case ObjItemID.S2_Photo_Frame_Floor:
                RO_OBJ[saveRotaObj].transform.DOMove(
                    new Vector3(-27.762f, 1.801f, 8.55254f), 2);
                break;
        }
    }

    // 旋轉物件UI畫面
    public void UIState(UIItemID r_ItemID, bool r_bEnable)
    {
        m_bInUIView = r_bEnable;
        playerCtrlr.m_bCanControl = !r_bEnable;
        playerCtrlr.SetCursor();

        goCanvas.SetActive(r_bEnable);
        ExitBtn.gameObject.SetActive(r_bEnable);
        imgUIBackGround.color = r_bEnable ? new Color(0, 0, 0, 0.60f) : new Color(0, 0, 0, 0.60f);
        imgInstructions.color = r_bEnable ? new Color(0, 0, 0, 1) : new Color(0, 0, 0, 0);
        int iItemID = (int)r_ItemID;

        txtTitle.text = GlobalDeclare.UITitle[iItemID];

        txtIntroduce.text = GlobalDeclare.UIIntroduce[iItemID];
        txtInstructions.text = GlobalDeclare.TxtInstructionsmage[iItemID];

        GetM_bInUIView();
    }

    // 執行物件動畫
    public void ProcessAnimator(string r_sObject, string r_sTriggerName)
    {
        if (r_sObject.Contains("null") || r_sTriggerName.Contains("null"))
            return;

        GameObject obj = GameObject.Find(r_sObject);
        Animator ani = obj.transform.GetComponent<Animator>();
        ani.SetTrigger(r_sTriggerName);

        if (obj.transform.GetComponent<ItemController>() != null)
        {
            obj.transform.GetComponent<ItemController>().SetHintable(false);
            obj.transform.GetComponent<ItemController>().bActive = false;
        }

        GlobalDeclare.SetItemAniObject("Empty");
        GlobalDeclare.SetItemAniName("Empty");
        m_bShowItemAnimate = false;
    }

    public void ProcessItemAnimator(string r_strObject, string r_strTriggerName)
    {
        if (r_strObject.Contains("null") || r_strTriggerName.Contains("null"))
            return;

        GameObject obj = GameObject.Find(r_strObject);
        Animator ani = obj.transform.GetComponent<Animator>();
        ani.SetTrigger(r_strTriggerName);
        m_bShowItemAnimate = false;
    }

    // 執行玩家動畫
    public void ProcessPlayerAnimator(string r_sAnimationName)
    {
        Animation am = playerCtrlr.GetComponent<Animation>();
        am.Play(r_sAnimationName);
        m_bShowPlayerAnimate = false;
        GlobalDeclare.SetPlayerAnimateType(PlayerAnimateType.Empty);
    }

    // 限制角色視角 (暫無使用)
    public void SetPlayerViewLimit(bool bLimitRotation, float[] fViewLimit)
    {
        m_bSetPlayerViewLimit = false;
        playerCtrlr.m_bLimitRotation = bLimitRotation;
        playerCtrlr.m_fHorizantalRotationRange.x = fViewLimit[0];
        playerCtrlr.m_fHorizantalRotationRange.y = fViewLimit[1];

        if (bLimitRotation)
        {
            playerCtrlr.tfTransform.localEulerAngles = Vector3.up * fViewLimit[2];
            Debug.Log("Value : " + fViewLimit[2]);
        }
    }

    // 顯示進入蓮花遊戲按鈕
    public void ShowEnterGame(bool r_bEnable)
    {
        isUIOpen = r_bEnable;
        EnterGameBtn.gameObject.SetActive(r_bEnable);
        txtEnterGameHint.gameObject.SetActive(r_bEnable);
        txtEnterGameHint.text = r_bEnable ? "按 *R* 開始摺紙 \r\n(Press *R* Origami Lotus Paper)" : "";
    }

    public void ButtonFunction(ButtonEventID _eventID)
    {
        switch (_eventID)
        {
            case ButtonEventID.UI_Back:
                GameEvent(GameEventID.Close_UI);
                break;
            case ButtonEventID.Enter_Game:
                if (isUIOpen)
                {
                    // 執行相應的程式碼
                    GlobalDeclare.bLotusGameComplete = true;
                    RestoreItemLocation();
                    GameEvent(GameEventID.Close_UI);
                    playerCtrlr.m_bCanControl = false;
                    playerCtrlr.tfPlayerCamera.GetComponent<AudioListener>().enabled = false;
                    SceneManager.LoadScene(3, LoadSceneMode.Additive);
                }
                break;
        }
    }

    public void GrandMaRush()   // 奶奶衝撞
    {
        //tfGrandmaGhost.Translate(0f, 0f, 0.3f);
        m_iGrandmaRushCount++;

        if (m_iGrandmaRushCount >= 10)
        {
            CancelInvoke(nameof(GrandMaRush));
            playerCtrlr.m_bCanControl = false;
            goCanvas.SetActive(true);
            imgUIBackGround.color = new Color(0, 0, 0, 0.95f);
            Invoke(nameof(IvkShowGrandmaFaceUI), 0.5f);
        }
    }

    // 離開蓮花遊戲
    public void ExitLotusGame()
    {
        m_bPlayLotusEnable = false;
        playerCtrlr.m_bCanControl = true;
        playerCtrlr.tfPlayerCamera.GetComponent<AudioListener>().enabled = true;
        playerCtrlr.tfPlayerCamera.gameObject.SetActive(true);
        SceneManager.UnloadSceneAsync(3);

        S1_Lotus_Paper_Obj.transform.localPosition = new Vector3(-5.1f, -2f, -2f);
        S1_Finished_Lotus_Paper_Obj.transform.localPosition = new Vector3(-5.1f, 0.6f, -2f);

        ShowHint(HintItemID.S1_Finished_Lotus_Paper);

        DialogueObjects[ (byte)Lv1_Dialogue.AfterPlayLotus_Lv1 ].CallAction();
    }

    // 鍵盤檢查
    void KeyboardCheck()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            // 關閉 UI 畫面
            if (m_bInUIView)
            {
                GameEvent(GameEventID.Close_UI);

                if (isMoveingObject)
                {
                    
                    romanager = false;

                    if (!romanager)
                    {
                        RestoreItemLocation();
                        Ro_Light.enabled = false;
                    }
                }
            }
            else
            {
                // 顯示遊戲狀態
                SetGameState();
            }
        }
    }

    void RestoreItemLocation()
    {
        CameraVolume.enabled = false;
        romanager = RO_OBJ[saveRotaObj].GetComponent<RotateObjDetect>().enabled = false;
        print(RO_OBJ[saveRotaObj].transform.name);

        //恢復物件位置
        RO_OBJ[saveRotaObj].transform.DOMove(originalPosition, 2);

        //恢復物件角度
        RO_OBJ[saveRotaObj].transform.DORotate(originalRotation.eulerAngles, 2);
        isMoveingObject = false;
        StudioUI.SetActive(false);
    }

    void MouseCheck()   // 滑鼠檢查MouseButtonDown(0)
    {
        if (Input.GetMouseButtonDown(0))
        {
            // 在此處理滑鼠點擊事件
            // 可以使用 EventSystem 或 Raycasting 等方法進行 UI 按鈕的選擇處理
        }
    }

    public void SetGameState()  // 設定遊戲狀態
    {
        playerCtrlr.SetCursor();
        isPaused = !isPaused;
        Time.timeScale = isPaused ? 0f : 1f;
        settingObjects.SetActive(isPaused);
        isMouseEnabled = isPaused;
    }

    public void GameStateCheck()    // 檢查遊戲狀態
    {
        if (!GlobalDeclare.bLotusGameComplete &&
             m_bPlayLotusEnable &&
             currentScene.name == "2 Grandma House")
        {
            ShowHint(HintItemID.S1_Lotus_Paper);
        }
    }

    public bool GetM_bInUIView()    // 取得是否在 UI 畫面中
    {
        return m_bInUIView;
    }

    public void StopReadding()  // 停止閱讀查看物件
    {
        playerCtrlr.m_bCanControl = false;
        playerCtrlr.m_bLimitRotation = true;
        StartCoroutine(ChangeVignetteIntensity());
    }

    private IEnumerator ChangeVignetteIntensity()  // 改變電影模式Vignette強度
    {
        VolumeProfile profile = postProcessVolume.sharedProfile;

        if (profile.TryGet(out Vignette vignette) &&
            profile.TryGet(out CloudLayer cloudLayer))
        {
            float currentIntensity = 0.64f;
            float elapsedTime = 0f;

            while (elapsedTime < 1f)
            {
                vignette.intensity.value = Mathf.Lerp(.7f,
                                                    targetIntensity, elapsedTime);
                vignette.smoothness.value = Mathf.Lerp(.16f,
                                                    targetIntensity, elapsedTime);
                vignette.roundness.value = Mathf.Lerp(.18f,
                                                    targetIntensity, elapsedTime);
                cloudLayer.opacity.value = Mathf.Lerp(currentIntensity,
                                                    targetIntensity, elapsedTime);

                elapsedTime += Time.deltaTime * changeSpeed;
                yield return null;
            }
            vignette.intensity.value = 0.7f;
            vignette.smoothness.value = 0.16f;
            vignette.roundness.value = 0.18f;
            cloudLayer.opacity.value = 0.0f;
            playerCtrlr.m_bCanControl = true;
            playerCtrlr.m_bLimitRotation = false;
        }
    }

    void LastAnimateAfterPhotoFrame()   // 照片框動畫後的最後動畫
    {
        // 暫時這樣做
        playerCtrlr.m_bCanControl = false;

        audManager.Play(1, "Crying_in_the_bathroom", false);
        Invoke(nameof(IvkS2_SlientAfterPhotoFrame), 2f);
    }

    void ShowQRCode()
    {
        bIsGameEnd = false;
        m_bReturnToBegin = true;
        FinalUI.SetActive(false);
        QRCodeUI.SetActive(true);
    }

    void BackToBaseGame()
    {
        m_bReturnToBegin = false;
        QRCodeUI.SetActive(false);
        playerCtrlr.SetCursor();
        SceneManager.LoadScene(0);
    }

    void SetCrosshairEnable(bool bEnable)
    {
        CrosshairUI.SetActive(bEnable);
    }

    // 延遲動作
    IEnumerator DelayedAction()
    {
        yield return new WaitForSeconds(2.5f);
    }

    // 延遲載入大廳場景
    IEnumerator DelayLodelobby()
    {
        audManager.Play(1, "Opening_Scene", false);
        FinalUI.SetActive(true);
        SceneManager.LoadScene(0);
        yield return null;
    }
}