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
    public ProgressProcessing progressProcessing;

    float targetIntensity = 1f; // 目標強度值
    float currentIntensity = 0.3f; // 當前強度值
    public float changeSpeed = 1f; // 強度改變速度
    bool isMoveingObject = false;    // 是否正在移動物件
    public Vector3 originalPosition;    // 原始位置
    public Quaternion originalRotation; // 原始旋轉
    [SerializeField] private AUDManager audManager;
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

    [Header("玩家")] public PlayerController playerCtrlr;
    [SerializeField] [Header("Flowchart")] GameObject[] flowchartObjects;
    [SerializeField] [Header("設定頁面")] public GameObject settingObjects;
    [SerializeField] [Header("破碎相框co")] public Collider photoCollider;
    [SerializeField] [Header("S2_阿嬤相框Ro")] public GameObject S2_Photo_Frame_Obj;

    [SerializeField] [Header("Video 撥放器")] VideoPlayer videoPlayer;
    [SerializeField] [Header("Video - 阿嬤看螢幕")] VideoClip GrandmaVP;

    [SerializeField] [Header("QRCode UI")] GameObject QRCodeUI;
    [SerializeField] [Header("準心 UI")] GameObject CrosshairUI;

    int m_iGrandmaRushCount;
    Scene currentScene;

    ItemController TempItem;

    #region Canvas Zone
    [SerializeField] GameObject goCanvas;
    [SerializeField] UnityEngine.UI.Image imgUIBackGround;
    [SerializeField] Text txtTitle;

    [SerializeField] UnityEngine.UI.Image imgInstructions;
    [SerializeField] Text txtInstructions;
    [SerializeField] Text txtIntroduce;

    [SerializeField] UnityEngine.UI.Button ExitBtn;
    [SerializeField] Text txtEnterGameHint;
    [SerializeField] UnityEngine.UI.Button EnterGameBtn;
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

    public static bool m_bPhotoFrameLightOn = false;
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
    [SerializeField] [Header("S1_打翻前的腳尾飯")] GameObject S1_Rice_Funeral_Obj;
    [SerializeField] [Header("S1_完好的相框")] GameObject S1_Photo_Frame_Obj;
    [SerializeField] [Header("S1_破碎的相框")] GameObject S1_Photo_Frame_Has_Broken_Obj;
    [SerializeField] [Header("S1_奶奶房間抽屜")] GameObject S1_Desk_Drawer_Obj;
    [SerializeField] [Header("S1_還沒摺的蓮花紙")] GameObject S1_Lotus_Paper_Obj;
    [SerializeField] [Header("S1_蓮花紙旁的蠟燭")] GameObject S1_Lotus_Candle_Obj;

    [Header("場景二物件")]
    [SerializeField] [Header("S2_鬼阿嬤")] GameObject S2_Grandma_Ghost_Obj;
    [SerializeField] [Header("S2_廚房物件_狀態一")] GameObject S2_Furniture_State_1_Obj;
    [SerializeField] [Header("S2_廚房物件_狀態二")] GameObject S2_Furniture_State_2_Obj;
    [SerializeField] [Header("S2_躺在床上的奶奶屍體")] GameObject S2_Grandma_Deadbody_On_Table_Obj;
    [SerializeField] [Header("S2_廁所鬼頭")] GameObject S2_Toilet_Door_GhostHead_Obj;
    [SerializeField] [Header("S2_阿嬤相框")] GameObject S2_Photo_Frame_Obj_floor;
    [SerializeField] [Header("S2_阿嬤哭聲撥放器")] GameObject S2_Grandma_Cry_Audio_Obj;
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

        imgUIBackGround = goCanvas.transform.GetChild(0).GetComponent<UnityEngine.UI.Image>();  // 背景
        txtTitle = goCanvas.transform.GetChild(2).GetComponent<Text>(); // 標題

        imgInstructions = goCanvas.transform.GetChild(3).GetComponent<UnityEngine.UI.Image>();  // 說明圖示
        txtInstructions = goCanvas.transform.GetChild(3).GetComponentInChildren<Text>();    // 說明文字
        txtIntroduce = goCanvas.transform.GetChild(4).GetComponentInChildren<Text>();   // 介紹文字

        ExitBtn = goCanvas.transform.GetChild(5).GetComponent<UnityEngine.UI.Button>(); // 返回按鈕

        txtEnterGameHint = goCanvas.transform.GetChild(6).GetComponent<Text>(); // 進入遊戲提示
        EnterGameBtn = goCanvas.transform.GetChild(7).GetComponent<UnityEngine.UI.Button>();    // 進入遊戲按鈕

        TempItem = null;    // 暫存物件
        currentScene = SceneManager.GetActiveScene();   // 當前場景
    }

    void Start()
    {
        GameEvent(GameEventID.Close_UI);

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

        if (isUIOpen && Input.GetKeyDown(KeyCode.R))    // 關閉 UI 時按 R 鍵
            ButtonFunction(ButtonEventID.Enter_Game);   // 進入蓮花遊戲

        if (bIsGameEnd && Input.GetKeyDown(KeyCode.F9))
            ShowQRCode();

        //if (m_bReturnToBegin && Input.GetKeyDown(KeyCode.F10))
        //    BackToBaseGame();

        //if (Input.GetKeyDown(KeyCode.F11))
        //    GameEvent(GameEventID.S1_Rice_Funeral_Spilled);
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

                // UI 返回後執行 Fungus 對話
                if (m_bShowDialog)
                    ProcessDialog(GlobalDeclare.GetDialogObjName());

                if (m_bSetPlayerViewLimit)
                    SetPlayerViewLimit(true, GlobalDeclare.PlayerCameraLimit.GetPlayerCameraLimit());

                if (bS2_TriggerLastAnimateAfterPhotoFrame)
                    LastAnimateAfterPhotoFrame();

                GameStateCheck();
                break;
            case GameEventID.S1_Photo_Frame:
                Debug.Log("場景1 ==> 正常相框 (S1_Photo_Frame)");
                S1_PhotoFrameEvent();
                break;
            case GameEventID.S1_Photo_Frame_Has_Broken:
                Debug.Log("場景1 ==> 破碎相框 (S1_Photo_Frame_Has_Broken)");
                S1_PhotoFrameHasBroken();
                break;
            case GameEventID.S1_Grandma_Door_Open:
                Debug.Log("場景1 ==> 打開奶奶房間門 (S1_Grandma_Door_Open)");
                S1_GrandmaDoorOpen();
                break;
            case GameEventID.S1_Lotus_Paper:
                Debug.Log("場景1 ==> 點擊桌上的蓮花紙 (S1_Lotus_Paper)");
                S1_LotusPaper();
                break;
            case GameEventID.S1_Grandma_Dead_Body:
                Debug.Log("場景1 ==> 跟孝濂內的奶奶互動 (S1_Grandma_Dead_Body)");
                S1_GrandmaDeadBody();
                break;
            case GameEventID.S1_White_Tent:
                Debug.Log("場景1 ==> 觸發孝濂 (S1_White_Tent)");
                S1_WhiteTent();
                break;
            case GameEventID.S1_Photo_Frame_Light_On:
                Debug.Log("場景1 ==> 亮相框提示燈 (舊 Func 暫無使用) (S1_Photo_Frame_Light_On)");
                S1_PhotoFrameLightOn();
                break;
            case GameEventID.S1_Grandma_Rush:
                Debug.Log("場景1 ==> 奶奶衝刺 (舊 Func 暫無使用) (S1_Grandma_Rush)");
                S1_GrandmaRush();
                break;
            case GameEventID.S1_Light_Switch:
                Debug.Log("場景1 ==> 觸發奶奶房間燈開關 (S1_Light_Switch)");
                S1_LightSwitch();
                break;
            case GameEventID.S1_Flashlight:
                Debug.Log("場景1 ==> 觸發奶奶房間手電筒 (S1_Flashlight)");
                S1_Flashlight();
                break;
            case GameEventID.S1_Desk_Drawer:
                Debug.Log("場景1 ==> 奶奶房間抽屜 (S1_Desk_Drawer)");
                S1_DeskDrawer();
                break;
            case GameEventID.S1_GrandmaRoomKey:
                Debug.Log("場景1 ==> 奶奶房間抽屜鑰匙 (S1_GrandmaRoomKey)");
                S1_GrandmaRoomKey();
                break;
            case GameEventID.S1_Grandma_Room_Door_Lock:
                Debug.Log("場景1 ==> 奶奶房間門鎖住 (S1_Grandma_Room_Door_Lock)");
                S1_GrandmaRoomDoorLock();
                break;
            case GameEventID.S1_Rice_Funeral_Spilled:
                Debug.Log("場景1 ==> 腳尾飯打翻 (S1_Rice_Funeral_Spilled)");
                S1_RiceFuneralSpilled();
                break;
            case GameEventID.S1_Rice_Funeral:
                Debug.Log("場景1 ==> 桌上的腳尾飯 (S1_Rice_Funeral)");
                S1_RiceFuneral();
                break;
            case GameEventID.S1_Toilet_Door_Lock:
                Debug.Log("場景1 ==> 廁所門鎖住了 (S1_Toilet_Door_Lock)");
                S1_ToiletDoorLock();
                break;
            case GameEventID.S1_Toilet_Door_Open:
                Debug.Log("場景1 ==> 打開廁所門 (S1_Toilet_Door_Open)");
                S1_ToiletDoorOpen();
                break;
            case GameEventID.S1_Toilet_Ghost_Hide:
                Debug.Log("場景1 ==> 廁所鬼頭縮回門後 (S1_Toilet_Ghost_Hide)");
                S1_ToiletGhostHide();
                break;
            case GameEventID.S1_Toilet_Ghost_Hand_Push:
                Debug.Log("場景1 ==> 廁所鬼手推玩家 (S1_Toilet_Ghost_Hand_Push)");
                S1_ToiletGhostHandPush();
                break;
            case GameEventID.S2_Light_Switch:
                Debug.Log("場景2 ==> 房間燈開關 (S2_Light_Switch)");
                bS2_TriggerLightSwitch = true;
                audManager.Play(1, "light_Switch_Sound", false);
                flowchartObjects[13].gameObject.SetActive(true);
                ShowHint(HintItemID.S2_FlashLight);
                break;
            case GameEventID.S2_Room_Door_Lock:
                Debug.Log("場景2 ==> 房間門鎖住了 (S2_Room_Door_Lock)");
                audManager.Play(1, "the_door_is_locked_and_cannot_be_opened_with_sound_effects", false);
                bS2_TriggerGrandmaDoorLock = true;
                flowchartObjects[12].gameObject.SetActive(true);

                ShowHint(HintItemID.S2_FlashLight);
                break;
            case GameEventID.S2_FlashLight:
                Debug.Log("場景2 ==> 手電筒 (S2_FlashLight)");
                flowchartObjects[14].gameObject.SetActive(true);
                audManager.Play(1, "light_Switch_Sound", false);
                GameObject S2_FlashLightObj = GameObject.Find("S2_FlashLight");
                Destroy(S2_FlashLightObj);
                ShowHint(HintItemID.S2_Side_Table);
                break;
            case GameEventID.S2_Side_Table:
                Debug.Log("場景2 ==> 開門旁邊的小桌抽屜 (S2_Side_Table)");
                ProcessAnimator("S2_Side_Table", "S2_Side_Table_Open_01");
                GameObject RoomKeyObj = GameObject.Find("S2_Grandma_Room_Key");
                RoomKeyObj.GetComponent<Animation>().Play();
                audManager.Play(1, "drawer_Opening_Sound", false);
                Invoke(nameof(IvkShowS2DoorKey), 1.25f);
                break;
            case GameEventID.S2_Room_Key:
                Debug.Log("場景2 ==> 門旁邊小桌抽屜內的鑰匙 (S2_Room_Key)");
                audManager.Play(1, "tet_Sound_Of_Get_The_Key", false);
                BoxCollider S2_Door_Knock_Trigger = GameObject.Find("S2_Door_Knock_Trigger").GetComponent<BoxCollider>();
                S2_Door_Knock_Trigger.enabled = true;
                GameObject GrandMaRoomKeyObj = GameObject.Find("S2_Grandma_Room_Key");
                Destroy(GrandMaRoomKeyObj);
                break;
            case GameEventID.S2_Door_Knock_Stop:
                Debug.Log("S2_Door_Knock_Stop");
                audManager.Play(1, "emergency_Knock_On_The_Door", false);
                ShowHint(HintItemID.S2_Grandma_Room_Door_Open);
                break;
            case GameEventID.S2_Grandma_Door_Open:
                Debug.Log("S2_Grandma_Door_Open");
                ProcessAnimator("S2_Grandma_Room_Door", "S2_Grandma_Room_Door_Open");
                audManager.Play(1, "the_sound_of_the_eyes_opening_the_door", false);
                break;
            case GameEventID.S2_Grandma_Door_Close: //用力關門
                Debug.Log("S2_Grandma_Door_Close");
                audManager.Play(1, "door_Close", false);
                ProcessAnimator("S2_Grandma_Room_Door", "S2_Grandma_Room_Door_Close");
                audManager.Play(1, "Girl_laughing", false);
                break;
            case GameEventID.S2_Ghost_Pass_Door:
                Debug.Log("S2_Ghost_Pass_Door");
                S2_Grandma_Ghost_Obj.GetComponent<Animator>().SetTrigger("S2_Grandma_Pass_Door");
                audManager.Play(1, "grandma_StrangeVoice", false);
                S2_Grandma_Cry_Audio_Obj.SetActive(true);
                Invoke(nameof(IvkS2_Grandma_Pass_Door), 1.5f);
                break;
            case GameEventID.S2_Toilet_Door://阿嬤關門
                Debug.Log("S2_Toilet_Door");
                audManager.Play(1, "Crying_in_the_bathroom", false);
                ProcessPlayerAnimator("Player_S2_Shocked_By_Toilet_Ghost");
                S2_Furniture_State_1_Obj.SetActive(false);
                S2_Furniture_State_2_Obj.SetActive(true);
                S2_Photo_Frame_Obj_floor.SetActive(true);
                ShowHint(HintItemID.S2_Rice_Funeral);
                Invoke(nameof(IvkS2_Shocked_By_Toilet), 9.5f);
                break;
            case GameEventID.S2_Rice_Funeral:
                Debug.Log("S2_Rice_Funeral");
                audManager.Play(1, "get_Item_Sound", false);
                flowchartObjects[15].gameObject.SetActive(true);
                BoxCollider S2_Rice_Funeral_Collider = GameObject.Find("S2_Rice_Funeral").GetComponent<BoxCollider>();
                S2_Rice_Funeral_Collider.enabled = false;
                ShowHint(HintItemID.S2_Photo_Frame);
                break;
            case GameEventID.S2_Photo_Frame:
                Debug.Log("S2_Photo_Frame");
                audManager.Play(1, "At_the_end_it_is_found_that_Acuan_has_mostly_disappeared_and_Acuan_has_climbed_up", false);
                ProcessRoMoving(4);
                UIState(UIItemID.S2_Photo_Frame, true);
                ShowObj(ObjItemID.S2_Photo_Frame_Floor);

                // 開啟相框光源
                S2_Grandma_Deadbody_On_Table_Obj.SetActive(false);
                bS2_TriggerLastAnimateAfterPhotoFrame = true;
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
                TempItem = GameObject.Find("Lotus_Paper").GetComponent<ItemController>();
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

        isMoveingObject = true;  // 正在移動物件
        saveRotaObj = iIndex;   // 儲存物件  
        originalPosition = RO_OBJ[saveRotaObj].transform.position;  // 儲存物件位置
        originalRotation = RO_OBJ[saveRotaObj].transform.rotation;  // 儲存物件旋轉
        Ro_Cololider = RO_OBJ[saveRotaObj].GetComponent<Collider>();    // 儲存物件碰撞器
        romanager = RO_OBJ[saveRotaObj].GetComponent<RotateObjDetect>().enabled = true; // 啟用旋轉物件碰撞器
    }

    // 顯示進入旋轉遊戲按鈕
    public void ShowObj(ObjItemID O_ItemID)  // 顯示物件 UI  (旋轉物件)  (物件ID)
    {
        switch (O_ItemID)
        {
            case ObjItemID.S1_Rice_Funeral:
                RO_OBJ[saveRotaObj].transform.DOMove(
                    new Vector3(itemObjTransform.transform.position.x,
                    itemObjTransform.transform.position.y,
                    itemObjTransform.transform.position.z), 2);
                break;
            case ObjItemID.S1_Lotus_Paper:
                RO_OBJ[saveRotaObj].transform.DOMove(
                    new Vector3(itemObjTransform.transform.position.x,
                    itemObjTransform.transform.position.y + .07f,
                    itemObjTransform.transform.position.z), 2);
                break;
            case ObjItemID.S1_Photo_Frame:
                RO_OBJ[saveRotaObj].transform.DOMove(
                    new Vector3(itemObjTransform.transform.position.x,
                    itemObjTransform.transform.position.y,
                    itemObjTransform.transform.position.z), 2);
                break;
            case ObjItemID.S2_Photo_Frame:
                RO_OBJ[saveRotaObj].transform.DOMove(
                    new Vector3(itemObjTransform.transform.position.x,
                    itemObjTransform.transform.position.y,
                    itemObjTransform.transform.position.z), 2);
                break;
            case ObjItemID.S2_Photo_Frame_Floor:
                RO_OBJ[saveRotaObj].transform.DOMove(
                    new Vector3(itemObjTransform.transform.position.x,
                    itemObjTransform.transform.position.y,
                    itemObjTransform.transform.position.z), 2);
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
        imgUIBackGround.color = r_bEnable ? new Color(0, 0, 0, .02f) : new Color(0, 0, 0, 0);
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

    // 執行 Fungus 對話
    public void ProcessDialog(string sDialogObjName)
    {
        if (sDialogObjName.Contains("Empty"))
            return;

        GameObject dialog = GameObject.Find(sDialogObjName);
        dialog.gameObject.SetActive(true);
        m_bShowDialog = false;
        GlobalDeclare.SetDialogObjName("Empty");
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
                    GameEvent(GameEventID.Close_UI);
                    playerCtrlr.m_bCanControl = false;
                    playerCtrlr.tfPlayerCamera.gameObject.SetActive(false);
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

    public void ExitLotusGame() // 離開蓮花遊戲
    {
        m_bPlayLotusEnable = false;
        playerCtrlr.m_bCanControl = true;
        playerCtrlr.tfPlayerCamera.gameObject.SetActive(true);
        SceneManager.UnloadSceneAsync(3);

        UnityEngine.Object Lotus_state_Final = Resources.Load<GameObject>("Prefabs/Lotus_state_Final");
        GameObject LotusObj = Instantiate(Lotus_state_Final) as GameObject;
        LotusObj.transform.position = new Vector3(-5.2f, 0.6f, -2.4f);

        GameObject LotusDestory = GameObject.Find("Lotus_Paper");
        LotusDestory.transform.position = new Vector3(-5f, -2f, -2f);

        m_bPhotoFrameLightOn = true;

        flowchartObjects[5].gameObject.SetActive(true);

        TempItem = GameObject.Find("Toilet_Door_Ghost").GetComponent<ItemController>();
        TempItem.bAlwaysActive = false;
        TempItem.eventID = GameEventID.S1_Toilet_Door_Open;
    }

    void KeyboardCheck()    // 鍵盤檢查
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            // 關閉 UI 畫面
            if (m_bInUIView)
            {
                GameEvent(GameEventID.Close_UI);

                if (isMoveingObject)
                {
                    //關閉旋轉
                    romanager = false;

                    if (!romanager)
                    {
                        print(RO_OBJ[saveRotaObj].transform.GetChild(0).name);

                        //恢復物件位置
                        RO_OBJ[saveRotaObj].transform.DOMove(originalPosition, 2);

                        //恢復物件角度
                        RO_OBJ[saveRotaObj].transform.DORotate(originalRotation.eulerAngles, 2);
                        isMoveingObject = false;
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