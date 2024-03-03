using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;
using UnityEngine.Rendering.HighDefinition;
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

    [SerializeField] [Header("欲製物 - Schedule")] Text prefabs_Schedule;

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
    [SerializeField] [Header("對話程序")] DialogueManager[] DialogueObjects;
    [SerializeField] [Header("設定頁面")] public GameObject settingObjects;
    [SerializeField] [Header("S2_阿嬤相框Ro")] public GameObject S2_Photo_Frame_Obj;

    [SerializeField] [Header("Video 撥放器")] VideoPlayer videoPlayer;

    [SerializeField] [Header("QRCode UI")] GameObject QRCodeUI;
    [SerializeField] [Header("準心 UI")] GameObject CrosshairUI;
    [SerializeField] [Header("阿嬤收尾嚇人影片 UI")] RawImage RawImgGrandmaUI;

    [SerializeField] [Header("洗手台的水")] GameObject WaterSurfaceObj;
    [SerializeField] [Header("追蹤物件位置")] Transform[] Targers;
    [SerializeField] [Header("鋼琴提示介面")] GameObject PianoUI;
    int m_iGrandmaRushCount;
    Scene currentScene;

    ItemController TempItem;

    [SerializeField] [Header("電視 White noise 材質球")] Material Lv1_matTVWhiteNoise;

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
    #endregion

    #region Game Point
    bool bLv1_HasGrandmaRoomKey = false;
    bool bLv1_HasFlashlight = false;
    bool bLv1_TriggerRiceFuneral = false;

    bool bLv2_HasGrandmaRoomKey = false;
    bool bLv2_HasFlashlight = false;
    bool bS2_TriggerLastAnimateAfterPhotoFrame = false;
    #endregion

    #region - All Scene Items -
    [Header("場景一物件")]
    [SerializeField] [Header("Lv1_阿嬤的房間門")] ItemController Lv1_Grandma_ROOM_Door_Item;
    [SerializeField] [Header("Lv1_房間燈開關")] ItemController Lv1_Light_Switch_Item;
    [SerializeField] [Header("Lv1_手電筒")] ItemController Lv1_FlashLight_Item;
    [SerializeField] [Header("Lv1_阿嬤房間內的抽屜")] ItemController Lv1_Desk_Drawer_Item;
    [SerializeField] [Header("Lv1_孝濂")] ItemController Lv1_Filial_Piety_Curtain_Item;
    [SerializeField] [Header("Lv1_水龍頭")] ItemController Lv1_Faucet_Item;
    [SerializeField] [Header("Lv1_水龍頭水粒子")] GameObject Lv1_Faucet_Flush_Obj;
    [SerializeField] [Header("Lv1_廁所門")] ItemController Lv1_Toilet_Door_Item;
    [SerializeField] [Header("Lv1_完整的相框")] ItemController Lv1_Photo_Frame_Item;
    [SerializeField] [Header("Lv1_破碎的相框")] ItemController Lv1_Photo_Frame_Broken_Item;
    [SerializeField] [Header("Lv1_鋼琴")] ItemController Lv1_Piano_Item;
    [SerializeField] [Header("Lv1_娃娃 Ani")] Animator Lv1_Doll_Ani;

    [SerializeField] [Header("S1_打翻前的腳尾飯")] GameObject S1_Rice_Funeral_Obj;
    [SerializeField] [Header("S1_完好的相框")] GameObject S1_Photo_Frame_Obj;
    [SerializeField] [Header("S1_破碎的相框")] GameObject S1_Photo_Frame_Has_Broken_Obj;
    [SerializeField] [Header("S1_阿嬤房間抽屜")] GameObject S1_Desk_Drawer_Obj;
    [SerializeField] [Header("S1_還沒摺的蓮花紙")] GameObject S1_Lotus_Paper_Obj;
    [SerializeField] [Header("S1_蓮花紙旁的蠟燭")] GameObject S1_Lotus_Candle_Obj;
    [SerializeField] [Header("S1_摺好的紙蓮花")] GameObject S1_Finished_Lotus_Paper_Obj;
    [SerializeField] [Header("S1_放紙蓮花的盤子")] GameObject S1_Lotus_Paper_Plate_Obj;

    [Header("場景二物件")]
    [SerializeField] [Header("Lv2_手電筒")] ItemController Lv2_FlashLight_Item;
    [SerializeField] [Header("Lv2_小邊桌")] ItemController Lv2_SideTable_Item;
    [SerializeField] [Header("Lv2_阿嬤房間門")] ItemController Lv2_Grandma_Room_Door_Item;

    [SerializeField] [Header("Lv2_腳尾飯_Item")] ItemController Lv2_Rice_Funeral_Item;
    [SerializeField] [Header("Lv2_腳尾飯_Obj")] GameObject Lv2_Rice_Funeral_Obj;
    [SerializeField] [Header("Lv2_腳尾凳子_Item")] ItemController Lv2_Piano_Stool_Item;

    [SerializeField] [Header("Lv2_哥哥的鞋子_Obj")] GameObject Lv2_BrotherShoe_Obj;
    [SerializeField] [Header("Lv2_哥哥的鞋子_Item")] ItemController Lv2_BrotherShoe_Item;

    [SerializeField] [Header("S2_鬼阿嬤")] GameObject S2_Grandma_Ghost_Obj;
    [SerializeField] [Header("S2_廚房物件_狀態一")] GameObject S2_Furniture_State_1_Obj;
    [SerializeField] [Header("S2_廚房物件_狀態二")] GameObject S2_Furniture_State_2_Obj;
    [SerializeField] [Header("S2_躺在床上的阿嬤屍體")] GameObject S2_Grandma_Deadbody_On_Table_Obj;
    [SerializeField] [Header("S2_廁所鬼頭")] GameObject S2_Toilet_Door_GhostHead_Obj;
    [SerializeField] [Header("S2_阿嬤相框")] GameObject S2_Photo_Frame_Obj_floor;
    [SerializeField] [Header("S2_阿嬤哭聲撥放器")] GameObject S2_Grandma_Cry_Audio_Obj;
    [SerializeField] [Header("S2_走廊門框")] GameObject S2_Corridor_Door_Frame_Obj;
    [SerializeField] [Header("S2_取代走廊門框的牆壁")] GameObject S2_Wall_Replace_Door_Frame_Obj;
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
    bool bNeedShowDialog = false;
    bool bIsPlayingLotus = false;
    bool bIsPlayingPiano = false;
    bool bHasTriggerLotus = false;

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

        SetCrosshairEnable(true);

        // Lv1 預設開啟的 Hint
        ShowHint(HintItemID.S1_Desk_Drawer);
        ShowHint(HintItemID.S1_Flashlight);
        ShowHint(HintItemID.S1_Light_Switch);
        ShowHint(HintItemID.S1_Grandma_Room_Door_Lock);
        ShowHint(HintItemID.S1_Toilet_Door);
        ShowHint(HintItemID.S1_Filial_Piety_Curtain);
        ShowHint(HintItemID.Lv1_Piano);

        // 尚未完成前情提要的串接，因此先在 Start 的地方跑動畫
        playerCtrlr.gameObject.GetComponent<Animation>().PlayQueued("Player_Wake_Up");

        // For Test
        //Light playerFlashlight = playerCtrlr.tfPlayerCamera.GetComponent<Light>();
        //playerFlashlight.enabled = true;
        // S2_ToiletDoor();
        //S1_RiceFuneralSpilled();
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

                if (bNeedShowDialog)
                {
                    DialogueObjects[GlobalDeclare.byCurrentDialogIndex].CallAction();
                    bNeedShowDialog = false;
                }

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
            case GameEventID.Lv1_CheckFilialPietyCurtain:
                Lv1_CheckFilialPietyCurtain();
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
            case GameEventID.Lv1_Faucet:
                Lv1_Faucet();
                break;
            case GameEventID.Lv1_Piano:
                Lv1_CheckPiano();
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
            case GameEventID.Lv2_Ruce_Funeral_Plate:
                Lv2_RuceFuneralPlate();
                break;
            case GameEventID.Lv2_Boy_Sneaker:
                Lv2_BoySneaker();
                break;
        }
    }

    // 顯示眼睛 Hint 圖示
    public void ShowHint(HintItemID _ItemID)
    {
        switch (_ItemID)
        {
            case HintItemID.S1_Grandma_Room_Door_Lock:
                TempItem = Lv1_Grandma_ROOM_Door_Item;
                break;
            case HintItemID.S1_Light_Switch:
                TempItem = Lv1_Light_Switch_Item;
                break;
            case HintItemID.S1_Grandma_Room_Door:
                TempItem = Lv1_Grandma_ROOM_Door_Item;
                TempItem.gameObject.layer = LayerMask.NameToLayer("InteractiveItem");
                TempItem.bAlwaysActive = false;
                TempItem.eventID = GameEventID.S1_Grandma_Door_Open;
                break;
            case HintItemID.S1_Flashlight:
                TempItem = Lv1_FlashLight_Item;
                break;
            case HintItemID.S1_Desk_Drawer:
                TempItem = S1_Desk_Drawer_Obj.GetComponent<ItemController>();
                break;
            case HintItemID.S1_Grandma_Room_Key:
                TempItem = GameObject.Find("Grandma_Room_Key").GetComponent<ItemController>();
                break;
            case HintItemID.S1_Filial_Piety_Curtain:
                TempItem = Lv1_Filial_Piety_Curtain_Item;

                if (bLv1_TriggerRiceFuneral)
                {
                    TempItem.bAlwaysActive = false;
                    TempItem.eventID = GameEventID.S1_White_Tent;
                }
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
            case HintItemID.S1_Rice_Funeral_Spilled:
                TempItem = GameObject.Find("Rice_Funeral_Spilled").GetComponent<ItemController>();
                break;
            case HintItemID.S1_Photo_Frame_Has_Broken:
                TempItem = Lv1_Photo_Frame_Broken_Item;
                break;
            case HintItemID.S1_Photo_Frame:
                TempItem = Lv1_Photo_Frame_Item;
                break;
            case HintItemID.S1_Toilet_Door:
                TempItem = Lv1_Toilet_Door_Item;
                break;
            case HintItemID.S1_Toilet_GhostHand_Trigger:
                TempItem = GameObject.Find("Ghost_Hand_Trigger").GetComponent<ItemController>();
                break;
            case HintItemID.Lv1_Faucet:
                TempItem = Lv1_Faucet_Item;
                break;
            case HintItemID.S2_Light_Switch:
                TempItem = GameObject.Find("S2_Light_Switch").GetComponent<ItemController>();
                break;
            case HintItemID.S2_Room_Door:
                TempItem = Lv2_Grandma_Room_Door_Item;
                break;
            case HintItemID.S2_FlashLight:
                TempItem = Lv2_FlashLight_Item;
                break;
            case HintItemID.S2_Side_Table:
                TempItem = Lv2_SideTable_Item;
                break;
            case HintItemID.S2_Room_Key:
                TempItem = GameObject.Find("S2_Grandma_Room_Key").GetComponent<ItemController>();
                break;
            case HintItemID.S2_Grandma_Room_Door_Open:
                TempItem = Lv2_Grandma_Room_Door_Item;
                TempItem.gameObject.layer = LayerMask.NameToLayer("InteractiveItem");
                TempItem.eventID = GameEventID.S2_Grandma_Door_Open;
                TempItem.bAlwaysActive = false;
                break;
            case HintItemID.S2_Photo_Frame:
                TempItem = S2_Photo_Frame_Obj.GetComponent<ItemController>();
                break;
            case HintItemID.S2_Rice_Funeral:
                TempItem = Lv2_Rice_Funeral_Item;
                break;
            case HintItemID.S2_Toilet_Door:
                TempItem = GameObject.Find("S2_Toilet_Door_GhostHead").GetComponent<ItemController>();
                break;
            case HintItemID.Lv2_Ruce_Funeral_Plate:
                TempItem = Lv2_Piano_Stool_Item;
                break;
            case HintItemID.Lv2_Boy_Sneaker:
                TempItem = Lv2_BrotherShoe_Item;
                break;
            case HintItemID.Lv1_Piano:
                TempItem = Lv1_Piano_Item;
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
    public void ShowObj(ObjItemID O_ItemID)
    {
        StudioUI.SetActive(true);

        switch (O_ItemID)
        {
            case ObjItemID.S1_Rice_Funeral:
                RO_OBJ[saveRotaObj].transform.DOMove(
                    new Vector3(-28f, 1.85f, 8.32354f), 0.5f);
                break;
            case ObjItemID.S1_Lotus_Paper:
                RO_OBJ[saveRotaObj].transform.DOMove(
                    new Vector3(-27.8f, 1.8f, 8.745541f), 0.5f);
                break;
            case ObjItemID.S1_Photo_Frame:
                RO_OBJ[saveRotaObj].transform.DOMove(
                    new Vector3(-28f, 1.85f, 8.55254f), 0.5f);
                break;
            case ObjItemID.S2_Photo_Frame:
                RO_OBJ[saveRotaObj].transform.DOMove(
                    new Vector3(-28f, 1.85f, 8.55254f), 0.5f);
                break;
            case ObjItemID.S2_Photo_Frame_Floor:
                RO_OBJ[saveRotaObj].transform.DOMove(
                    new Vector3(-27.762f, 1.801f, 8.55254f), 0.5f);
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

    // 執行玩家移動到指定區域
    public IEnumerator ProcessPlayerTraceTarget(int index)
    {
        bIsPlayingPiano = true;
        Transform tfPianoPos = GameObject.Find("PianoTarget").GetComponent<Transform>();
        Transform tfCameraPos = tfPianoPos.GetChild(0);

        yield return StartCoroutine(PlayerToAniPos(Targers[index].position, tfPianoPos.rotation, tfCameraPos.rotation));

        if (bIsPlayingPiano == true)
            PianoUI.SetActive(true);
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
                    GameEvent(GameEventID.Close_UI);
                    RestoreItemLocation();
                    bIsPlayingLotus = true;

                    Transform tfPlayingLotusPos = GameObject.Find("Lv1_Playing_Lotus_Pos").GetComponent<Transform>();
                    Transform tfCameraPos = tfPlayingLotusPos.GetChild(0);
                    StartCoroutine(PlayerToAniPos(tfPlayingLotusPos.position, tfPlayingLotusPos.rotation, tfCameraPos.rotation));

                    Invoke(nameof(DelayEnterLotusGame), 2.2f);
                }
                break;
        }
    }

    public void GrandMaRush()   // 阿嬤衝撞
    {
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

    void QuitLotusGame()
    {
        bIsPlayingLotus = false;
        playerCtrlr.tfPlayerCamera.gameObject.SetActive(true);
        S1_Lotus_Paper_Obj.transform.localPosition = new Vector3(-3.9f, 0.6f, -2.4f);

        playerCtrlr.transform.localPosition = new Vector3(-3, 0.8f, -2.5f);
        playerCtrlr.m_bCanControl = true;
        playerCtrlr.gameObject.GetComponent<CapsuleCollider>().enabled = true;
        playerCtrlr.gameObject.GetComponent<Rigidbody>().useGravity = true;

        LotusGameManager LotusCtrlr = GameObject.Find("LotusGameController").GetComponent<LotusGameManager>();
        LotusCtrlr.SendMessage("SetLotusCanvasEnable", false);

        LotusGameManager.bIsGamePause = true;

        ShowHint(HintItemID.S1_Lotus_Paper);
    }

    void QuitPiano()
    {
        bIsPlayingPiano = false;
        PianoUI.SetActive(false);
        playerCtrlr.m_bCanControl = true;
        playerCtrlr.gameObject.GetComponent<CapsuleCollider>().enabled = true;
        playerCtrlr.gameObject.GetComponent<Rigidbody>().useGravity = true;
        ShowHint(HintItemID.Lv1_Piano);
    }

    // 離開蓮花遊戲
    public void ExitLotusGame()
    {
        m_bPlayLotusEnable = false;
        bIsPlayingLotus = false;
        playerCtrlr.m_bCanControl = true;
        playerCtrlr.tfPlayerCamera.gameObject.SetActive(true);
        playerCtrlr.transform.localPosition = new Vector3(-3, 0.8f, -2.5f);

        playerCtrlr.gameObject.GetComponent<CapsuleCollider>().enabled = true;
        playerCtrlr.gameObject.GetComponent<Rigidbody>().useGravity = true;

        SceneManager.UnloadSceneAsync(3);

        S1_Lotus_Paper_Obj.transform.localPosition = new Vector3(-3.9f, -2f, -2.4f);
        S1_Finished_Lotus_Paper_Obj.transform.localPosition = new Vector3(-3.9f, 0.6f, -2.4f);

        ShowHint(HintItemID.S1_Finished_Lotus_Paper);
        DialogueObjects[(byte)Lv1_Dialogue.AfterPlayLotus_Lv1].CallAction();
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
            else if (bIsPlayingLotus)
            {
                QuitLotusGame();
            }
            else if (bIsPlayingPiano)
            {
                QuitPiano();
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
        RO_OBJ[saveRotaObj].transform.DOMove(originalPosition, 0.1f);

        //恢復物件角度
        RO_OBJ[saveRotaObj].transform.DORotate(originalRotation.eulerAngles, 0.1f);
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
        yield return new WaitForSeconds(11f);
        playerCtrlr.m_bCanControl = true;
        playerCtrlr.m_bLimitRotation = false;
        //VolumeProfile profile = postProcessVolume.sharedProfile;

        //if (profile.TryGet(out Vignette vignette) &&
        //    profile.TryGet(out CloudLayer cloudLayer))
        //{
        //    float currentIntensity = 0.64f;
        //    float elapsedTime = 0f;

        //    while (elapsedTime < 1f)
        //    {
        //        vignette.intensity.value = Mathf.Lerp(0.7669371f,
        //                                            targetIntensity, elapsedTime);
        //        vignette.smoothness.value = Mathf.Lerp(0.3474241f,
        //                                            targetIntensity, elapsedTime);
        //        vignette.roundness.value = Mathf.Lerp(0.3629616f,
        //                                            targetIntensity, elapsedTime);
        //        cloudLayer.opacity.value = Mathf.Lerp(currentIntensity,
        //                                            targetIntensity, elapsedTime);

        //        elapsedTime += Time.deltaTime * changeSpeed;
        //        yield return null;
        //    }
        //    yield return new WaitForSeconds(8f);
        //    vignette.intensity.value = 0.7f;
        //    vignette.smoothness.value = 0.16f;
        //    vignette.roundness.value = 0.18f;
        //    cloudLayer.opacity.value = 0.0f;
        //    playerCtrlr.m_bCanControl = true;
        //    playerCtrlr.m_bLimitRotation = false;
        //}
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

    IEnumerator PlayerToAniPos(Vector3 v3PlayerTargetPos, Quaternion PlayerRotation, Quaternion CameraRotation)
    {
        playerCtrlr.m_bCanControl = false;
        playerCtrlr.gameObject.GetComponent<CapsuleCollider>().enabled = false;
        playerCtrlr.gameObject.GetComponent<Rigidbody>().useGravity = false;

        // 移動玩家
        float fTotalMoveTime = 1.0f;
        float fCurrentMoveTime = 0.0f;

        while (fCurrentMoveTime < fTotalMoveTime)
        {
            playerCtrlr.transform.localPosition = Vector3.Lerp(playerCtrlr.transform.localPosition, v3PlayerTargetPos, fCurrentMoveTime / (fTotalMoveTime * 5f));
            playerCtrlr.transform.localRotation = Quaternion.Slerp(playerCtrlr.transform.localRotation, PlayerRotation, fCurrentMoveTime / (fTotalMoveTime * 5f));

            fCurrentMoveTime += Time.deltaTime;

            yield return null;
        }

        playerCtrlr.transform.localPosition = v3PlayerTargetPos;
        playerCtrlr.transform.localRotation = PlayerRotation;

        // 移動玩家 Camera
        float fTotalViewTime = 1.0f;
        float fCurrentViewTime = 0.0f;

        while (fCurrentViewTime < fTotalViewTime)
        {
            playerCtrlr.tfPlayerCamera.localRotation = Quaternion.Slerp(playerCtrlr.tfPlayerCamera.localRotation, CameraRotation, fCurrentViewTime / (fTotalViewTime * 5f));

            fCurrentViewTime += Time.deltaTime;

            yield return null;
        }

        playerCtrlr.tfPlayerCamera.localRotation = CameraRotation;
    }

    public void GameQuit()
    {
        Debug.Log("GameQuit");
        Application.Quit();
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