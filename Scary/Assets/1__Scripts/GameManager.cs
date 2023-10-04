using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
using static UnityEditor.Rendering.CameraUI;
using UnityEngine.Rendering;
using System.Linq;
using System;
using DG.Tweening;
using UnityEngine.Rendering.HighDefinition;
using ProgressP;

public partial class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public ProgressProcessing progressProcessing;

    float targetIntensity = 1f; // 目標強度值
    float currentIntensity = 0.3f; // 當前強度值
    public float changeSpeed = 1f; // 強度改變速度
    bool isMovingObject = false;
    public Vector3 originalPosition;
    public Quaternion originalRotation;
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
    [SerializeField] [Header("S2_阿嬤相框Ro")] public GameObject S2_Photo_Frame_Obj_RO;


    int m_iGrandmaRushCount;
    Scene currentScene;

    ItemController TempItem;

    #region Canvas Zone
    [SerializeField] GameObject goCanvas;
    [SerializeField] UnityEngine.UI.Image imgUIBackGround;
    //[SerializeField] Image imgUIDisplay;
    //[SerializeField] Image titleImg;
    [SerializeField] Text txtTitle;

    [SerializeField] UnityEngine.UI.Image imgInstructions;
    [SerializeField] Text txtInstructions;
    //[SerializeField] Image imgScendInstructions;
    //[SerializeField] Image imgIntroduceBackground;
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

    [Header("場景二物件")]
    [SerializeField] [Header("S2_鬼阿嬤")] GameObject S2_Grandma_Ghost_Obj;
    [SerializeField] [Header("S2_廚房物件_狀態一")] GameObject S2_Furniture_State_1_Obj;
    [SerializeField] [Header("S2_廚房物件_狀態二")] GameObject S2_Furniture_State_2_Obj;
    [SerializeField] [Header("S2_躺在床上的奶奶屍體")] GameObject S2_Grandma_Deadbody_On_Table_Obj;
    [SerializeField] [Header("S2_廁所鬼頭")] GameObject S2_Toilet_Door_GhostHead_Obj;
    [SerializeField] [Header("S2_阿嬤相框")] GameObject S2_Photo_Frame_Obj;
    #endregion

    bool isPaused = false;
    bool isMouseEnabled = false;
    public bool isUIOpen = false;

    void Awake()
    {
        RO_OBJ = GameObject.FindGameObjectsWithTag("ItemObj");
        SortRO_OBJByName();

        if (playerCtrlr == null)
            playerCtrlr = GameObject.Find("Player").GetComponent<PlayerController>();

        if (goCanvas == null)
            goCanvas = GameObject.Find("UI Canvas");

        imgUIBackGround = goCanvas.transform.GetChild(0).GetComponent<UnityEngine.UI.Image>();
        //imgUIDisplay = goCanvas.transform.GetChild(1).GetComponent<Image>();
        //titleImg = goCanvas.transform.GetChild(2).GetComponent<Image>();
        txtTitle = goCanvas.transform.GetChild(2).GetComponent<Text>();

        imgInstructions = goCanvas.transform.GetChild(3).GetComponent<UnityEngine.UI.Image>();
        txtInstructions = goCanvas.transform.GetChild(3).GetComponentInChildren<Text>();
        //imgScendInstructions = goCanvas.transform.GetChild(2).GetComponentInChildren<Image>();

        //imgIntroduceBackground = goCanvas.transform.GetChild(4).GetComponent<Image>();
        txtIntroduce = goCanvas.transform.GetChild(4).GetComponentInChildren<Text>();

        ExitBtn = goCanvas.transform.GetChild(5).GetComponent<UnityEngine.UI.Button>();

        txtEnterGameHint = goCanvas.transform.GetChild(6).GetComponent<Text>();
        EnterGameBtn = goCanvas.transform.GetChild(7).GetComponent<UnityEngine.UI.Button>();

        TempItem = null;
        currentScene = SceneManager.GetActiveScene();
    }

    void Start()
    {
        GameEvent(GameEventID.Close_UI);

        ExitBtn.onClick.AddListener(() => ButtonFunction(ButtonEventID.UI_Back));
        EnterGameBtn.onClick.AddListener(() => ButtonFunction(ButtonEventID.Enter_Game));

        ShowHint(HintItemID.S1_Light_Switch);
        ShowHint(HintItemID.S1_Grandma_Room_Door_Lock);
        ShowHint(HintItemID.S1_Toilet_Door);
    }

    void Update()
    {
        KeyboardCheck();

        if (isPaused && isMouseEnabled)
            MouseCheck();

        if (isUIOpen && Input.GetKeyDown(KeyCode.R))
            ButtonFunction(ButtonEventID.Enter_Game);
    }

    public void GameEvent(GameEventID r_eventID)
    {
        switch (r_eventID)
        {
            case GameEventID.Close_UI:
                UIState(UIItemID.Empty, false);
                ShowEnterGame(false);
                AUDManager.instance.PlayerGameEventSFX();

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
                S1_Photo_Frame_Obj.SetActive(false);
                S1_Photo_Frame_Has_Broken_Obj.SetActive(true);
                ShowHint(HintItemID.S1_Photo_Frame_Has_Broken);
                break;
            case GameEventID.S1_Photo_Frame_Has_Broken:
                UIState(UIItemID.S1_Photo_Frame, true);
                photoCollider.enabled = true;
                saveRotaObj = 2;
                isMovingObject = true;
                originalPosition = RO_OBJ[saveRotaObj].transform.position;
                originalRotation = RO_OBJ[saveRotaObj].transform.rotation;
                Ro_Cololider = RO_OBJ[2].GetComponent<Collider>();
                romanager = RO_OBJ[2].GetComponent<ROmanager>().enabled = true;
                ShowObj(ObjItemID.S1_Photo_Frame);

                // 人形黑影
                ProcessAnimator("Toilet_Door_Ghost", "Toilet_Door_Ghost_In");
                m_bToiletGhostHasShow = true;

                // 限制角色視角
                m_bSetPlayerViewLimit = true;
                GlobalDeclare.PlayerCameraLimit.SetPlayerCameraLimit(150f, 250f, 160f);

                // 鬼手出現
                GameObject GhostHandObj = GameObject.Find("Ghost_Hand");
                GhostHandObj.transform.position = new Vector3(-8.5f, 0, 6);
                break;
            case GameEventID.S1_Grandma_Door_Open:
                ProcessAnimator("Grandma_Room_Door", "DoorOpen");
                AUDManager.instance.PlayerDoorOpenSFX();
                ShowHint(HintItemID.S1_Rice_Funeral);
                flowchartObjects[4].gameObject.SetActive(true);
                break;
            case GameEventID.S1_Lotus_Paper:
                saveRotaObj = 1;
                isMovingObject = true;
                Ro_Cololider = RO_OBJ[1].GetComponent<Collider>();
                romanager = RO_OBJ[1].GetComponent<ROmanager>().enabled = true;
                originalPosition = RO_OBJ[saveRotaObj].transform.position;
                originalRotation = RO_OBJ[saveRotaObj].transform.rotation;
                UIState(UIItemID.S1_Lotus_Paper, true);
                ShowEnterGame(true);
                ShowObj(ObjItemID.S1_Lotus_Paper);
                AUDManager.instance.PlayerLotusPaperSFX();

                break;
            case GameEventID.S1_Grandma_Dead_Body:
                StopReadding();
                //UIState(UIItemID.S1_Grandma_Dead_Body, true);
                flowchartObjects[6].gameObject.SetActive(true);
                Destroy(S1_Rice_Funeral_Obj);
                UnityEngine.Object RiceFuneralSpilled = Resources.Load<GameObject>("Prefabs/Rice_Funeral_Spilled");
                GameObject RiceFuneralSpilledObj = Instantiate(RiceFuneralSpilled) as GameObject;
                RiceFuneralSpilledObj.transform.parent = GameObject.Find("===== ITEMS =====").transform;
                RiceFuneralSpilledObj.transform.position = new Vector3(-4.4f, 0.006f, 11.8f);
                RiceFuneralSpilledObj.name = "Rice_Funeral_Spilled";
                ShowHint(HintItemID.S1_Rice_Funeral_Spilled);
                //progressProcessing.UpdateProgress(2);
                //KeepstoryReadding();
                break;
            case GameEventID.S1_White_Tent:
                ProcessAnimator("Filial_Piety_Curtain", "Filial_piety_curtain Open");
                BoxCollider curtain = GameObject.Find("Filial_Piety_Curtain").GetComponent<BoxCollider>();
                curtain.enabled = false;
                ShowHint(HintItemID.S1_Lie_Grandma_Body);
                AUDManager.instance.PlayerWhiteTentSFX();
                break;
            case GameEventID.S1_Photo_Frame_Light_On:
                goPhotoFrameLight.SetActive(true);
                m_bPhotoFrameLightOn = false;
                break;
            case GameEventID.S1_Grandma_Rush:
                //InvokeRepeating(nameof(GrandMaRush), 0f, 0.025f);
                AUDManager.instance.PlayerGrandmaRushSFX();
                playerCtrlr.m_bCanControl = false;
                //Animator AniGrandma = tfGrandmaGhost.GetComponent<Animator>();
                //AniGrandma.SetBool("Grandma_Attack", true);
                m_bGrandmaRush = false;
                break;
            case GameEventID.S1_Light_Switch:
                bS1_IsS1LightSwtichOK = true;
                flowchartObjects[2].gameObject.SetActive(true);
                AUDManager.instance.PlayerLightSwitchSFX();
                ShowHint(HintItemID.S1_Flashlight);
                break;
            case GameEventID.S1_Flashlight:
                bS1_TriggerFlashlight = true;
                ShowHint(HintItemID.S1_Desk_Drawer);
                Light playerFlashlight = playerCtrlr.tfPlayerCamera.GetComponent<Light>();
                playerFlashlight.enabled = true;
                AUDManager.instance.PlayerLightSwitchSFX();
                GameObject FlashLight = GameObject.Find("Flashlight");
                Destroy(FlashLight);
                break;
            case GameEventID.S1_Desk_Drawer:
                BoxCollider DrawerCollider = GameObject.Find("grandpa_desk/Desk_Drawer").GetComponent<BoxCollider>();
                DrawerCollider.enabled = false;
                ProcessAnimator("grandpa_desk/Desk_Drawer", "DrawerWithKey_Open");
                Invoke(nameof(IvkShowDoorKey), 1.2f);
                AUDManager.instance.OpenTheDrawerSFX();
                break;
            case GameEventID.S1_GrandmaRoomKey:
                ShowHint(HintItemID.S1_Grandma_Room_Door);
                AUDManager.instance.GetTheKeySFX();
                //progressProcessing.UpdateProgress(1);
                flowchartObjects[3].gameObject.SetActive(true);
                GameObject GrandmaRoomKeyObj = GameObject.Find("Grandma_Room_Key");
                Destroy(GrandmaRoomKeyObj);
                break;
            case GameEventID.S1_Grandma_Room_Door_Lock:
                bS1_TriggerGrandmaDoorLock = true;
                ShowHint(HintItemID.S1_Desk_Drawer);
                ShowHint(HintItemID.S1_Flashlight);
                flowchartObjects[1].gameObject.SetActive(true);
                AUDManager.instance.PlayerDoorLockSFX();
                //progressProcessing.UpdateProgress(0);
                break;
            case GameEventID.S1_Rice_Funeral_Spilled:
                RO_OBJ = GameObject.FindGameObjectsWithTag("ItemObj");
                SortRO_OBJByName();
                ShowHint(HintItemID.S1_Lotus_Paper);
                m_bPlayLotusEnable = true;
                flowchartObjects[8].gameObject.SetActive(true);
                //progressProcessing.UpdateProgress(3);
                break;
            case GameEventID.S1_Rice_Funeral:
                ShowHint(HintItemID.S1_Filial_Piety_Curtain);
                flowchartObjects[11].gameObject.SetActive(true);
                UIState(UIItemID.S1_Rice_Funeral, true);
                ShowObj(ObjItemID.S1_Rice_Funeral);
                Ro_Cololider = RO_OBJ[0].GetComponent<Collider>();
                print(RO_OBJ[0].name);
                romanager = RO_OBJ[0].GetComponent<ROmanager>().enabled = true;
                originalPosition = RO_OBJ[saveRotaObj].transform.position;
                originalRotation = RO_OBJ[saveRotaObj].transform.rotation;
                saveRotaObj = 0;
                isMovingObject = true;

                break;
            case GameEventID.S1_Toilet_Door_Lock:
                flowchartObjects[12].gameObject.SetActive(true);
                break;
            case GameEventID.S1_Toilet_Door_Open:
                ProcessAnimator("Toilet_Door_Ghost", "Toilet_Door_Open");
                BoxCollider ToiletDoorCollider = GameObject.Find("Toilet_Door_Ghost").GetComponent<BoxCollider>();
                ToiletDoorCollider.enabled = false;
                ShowHint(HintItemID.S1_Photo_Frame);
                break;
            case GameEventID.S1_Toilet_Ghost_Hide:
                ProcessAnimator("Toilet_Door_Ghost", "Toilet_Door_Ghost_Out");
                m_bToiletGhostHasShow = false;
                playerCtrlr.m_bLimitRotation = false;
                m_bWaitToiletGhostHandPush = true;
                GlobalDeclare.PlayerCameraLimit.ClearValue();
                ShowHint(HintItemID.S1_Toilet_GhostHand_Trigger);
                GameObject GhostHandTriggerObj = GameObject.Find("Ghost_Hand_Trigger");
                GhostHandTriggerObj.transform.position = new Vector3(-8.5f, 0.1f, 6.1f);
                break;
            case GameEventID.S1_Toilet_Ghost_Hand_Push:
                m_bWaitToiletGhostHandPush = false;
                ProcessItemAnimator("Ghost_Hand", "Ghost_Hand_Push");
                Invoke(nameof(IvkProcessPlayerFallingAnimator), 0.2f);
                break;
            case GameEventID.S2_Light_Switch:
                bS2_TriggerLightSwitch = true;
                flowchartObjects[13].gameObject.SetActive(true);
                ShowHint(HintItemID.S2_FlashLight);
                break;
            case GameEventID.S2_Room_Door_Lock:
                bS2_TriggerGrandmaDoorLock = true;
                flowchartObjects[12].gameObject.SetActive(true);
                ShowHint(HintItemID.S2_FlashLight);
                break;
            case GameEventID.S2_FlashLight:
                flowchartObjects[14].gameObject.SetActive(true);
                GameObject S2_FlashLightObj = GameObject.Find("S2_FlashLight");
                Destroy(S2_FlashLightObj);
                ShowHint(HintItemID.S2_Side_Table);
                break;
            case GameEventID.S2_Side_Table:
                ProcessAnimator("S2_Side_Table", "S2_Side_Table_Open_01");
                Invoke(nameof(IvkShowS2DoorKey), 1.25f);
                break;
            case GameEventID.S2_Room_Key:
                AUDManager.instance.EmergencyKnockOnTheDoor();
                BoxCollider S2_Door_Knock_Trigger = GameObject.Find("S2_Door_Knock_Trigger").GetComponent<BoxCollider>();
                S2_Door_Knock_Trigger.enabled = true;
                break;
            case GameEventID.S2_Door_Knock_Stop:
                AUDManager.instance.EmergencyKnockOnTheDoor();
                ShowHint(HintItemID.S2_Grandma_Room_Door_Open);
                break;
            case GameEventID.S2_Grandma_Door_Open:
                ProcessAnimator("S2_Grandma_Room_Door", "S2_Grandma_Room_Door_Open");
                AUDManager.instance.PlayerDoorOpenSFX();
                break;
            case GameEventID.S2_Grandma_Door_Close:
                AUDManager.instance.PlayerDoorLockSFX();
                ProcessAnimator("S2_Grandma_Room_Door", "S2_Grandma_Room_Door_Close");
                break;
            case GameEventID.S2_Ghost_Pass_Door:
                AUDManager.instance.ThereIsAStrangeContinuousSoundInTheToilet();
                S2_Grandma_Ghost_Obj.GetComponent<Animator>().SetTrigger("S2_Grandma_Pass_Door");
                Invoke(nameof(IvkS2_Grandma_Pass_Door), 1.5f);
                break;
            case GameEventID.S2_Toilet_Door:
                AUDManager.instance.StrangeNoisesInTheToilet();
                ProcessPlayerAnimator("Player_S2_Shocked_By_Toilet_Ghost");

                S2_Furniture_State_1_Obj.SetActive(false);
                S2_Furniture_State_2_Obj.SetActive(true);
                S2_Photo_Frame_Obj_RO.SetActive(true);
                RO_OBJ = GameObject.FindGameObjectsWithTag("ItemObj");
                ShowHint(HintItemID.S2_Rice_Funeral);

                Invoke(nameof(IvkS2_Shocked_By_Toilet), 4f);
                break;
            case GameEventID.S2_Rice_Funeral:
                // 紹威 (字幕 腳尾飯
                flowchartObjects[15].gameObject.SetActive(true);
                BoxCollider S2_Rice_Funeral_Collider = GameObject.Find("S2_Rice_Funeral").GetComponent<BoxCollider>();
                S2_Rice_Funeral_Collider.enabled = false;
                ShowHint(HintItemID.S2_Photo_Frame);
                break;
            case GameEventID.S2_Photo_Frame:
                // 紹威 (確認表現跟畫面 (會有 Error
                //saveRotaObj = 3;
                //isMovingObject = true;
                //originalPosition = RO_OBJ[saveRotaObj].transform.position;
                //originalRotation = RO_OBJ[saveRotaObj].transform.rotation;
                //Ro_Cololider = RO_OBJ[3].GetComponent<Collider>();
                //romanager = RO_OBJ[3].GetComponent<ROmanager>().enabled = true;
                ////UIState(UIItemID.S2_Photo_Frame, true);
                //ShowEnterGame(true);
                //ShowObj(ObjItemID.S2_Photo_Frame);


                // Correct
                S2_Grandma_Deadbody_On_Table_Obj.SetActive(false);
                bS2_TriggerLastAnimateAfterPhotoFrame = true;

                //AUDManager.instance.BodyTwistingSound();
                //StartCoroutine(DelayedAction());
                //AUDManager.instance.GhostEscape();
                //StartCoroutine(DelayLodelobby());

                // For Record
                LastAnimateAfterPhotoFrameForRecord();
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
                TempItem = GameObject.Find("Desk_Drawer").GetComponent<ItemController>();
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
                TempItem = GameObject.Find("S1_Photo_Frame_Has_Broken").GetComponent<ItemController>();
                break;
            case HintItemID.S1_Photo_Frame:
                TempItem = GameObject.Find("S1_Photo_Frame").GetComponent<ItemController>();
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

    /// <summary>
    /// 旋轉物件
    /// </summary>
    /// <param name="O_ItemID"></param>
    public void ShowObj(ObjItemID O_ItemID)
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
                    itemObjTransform.transform.position.y,
                    itemObjTransform.transform.position.z), 2);
                //RO_OBJ[1] = Instantiate(itemObj[1], newPosition, newRoation);
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
        }
    }

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

    public void ProcessPlayerAnimator(string r_sAnimationName)
    {
        Animation am = playerCtrlr.GetComponent<Animation>();
        am.Play(r_sAnimationName);
        m_bShowPlayerAnimate = false;
        GlobalDeclare.SetPlayerAnimateType(PlayerAnimateType.Empty);
    }

    public void ProcessDialog(string sDialogObjName)
    {
        if (sDialogObjName.Contains("Empty"))
            return;

        GameObject dialog = GameObject.Find(sDialogObjName);
        dialog.gameObject.SetActive(true);
        m_bShowDialog = false;
        GlobalDeclare.SetDialogObjName("Empty");
    }

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

    public void ShowEnterGame(bool r_bEnable)
    {
        isUIOpen = r_bEnable;
        EnterGameBtn.gameObject.SetActive(r_bEnable);
        txtEnterGameHint.gameObject.SetActive(r_bEnable);
        txtEnterGameHint.text = r_bEnable ? "---- R 進入遊戲 ----" : "";
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

    public void GrandMaRush()
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

    public void ExitLotusGame()
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

        //ProcessDialog("Flowchart (3)");
        flowchartObjects[5].gameObject.SetActive(true);

        TempItem = GameObject.Find("Toilet_Door_Ghost").GetComponent<ItemController>();
        TempItem.bAlwaysActive = false;
        TempItem.eventID = GameEventID.S1_Toilet_Door_Open;
    }

    void KeyboardCheck()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (m_bInUIView)
            {
                if (!isMovingObject)
                {
                    GameEvent(GameEventID.Close_UI);
                }
                else
                {
                    GameEvent(GameEventID.Close_UI);

                    //關閉旋轉
                    romanager = false;

                    if (!romanager)
                    {
                        print(RO_OBJ[saveRotaObj].transform.GetChild(0).name);
                        //恢復物件位置
                        RO_OBJ[saveRotaObj].transform.DOMove(originalPosition, 2);
                        //恢復物件角度
                        RO_OBJ[saveRotaObj].transform.DORotate(originalRotation.eulerAngles, 2);
                        isMovingObject = false;
                    }
                }
            }
            else
            {
                SetGameState();
            }
        }

        if (m_bReturnToBegin)
        {
            if (Input.GetKeyDown(KeyCode.L))
            {
                playerCtrlr.SetCursor();
                SceneManager.LoadScene(0);
                m_bReturnToBegin = false;
            }
        }
    }

    void MouseCheck()
    {
        if (Input.GetMouseButtonDown(0))
        {
            // 在此處理滑鼠點擊事件
            // 可以使用 EventSystem 或 Raycasting 等方法進行 UI 按鈕的選擇處理
        }
    }

    public void SetGameState()
    {
        playerCtrlr.SetCursor();
        isPaused = !isPaused;
        Time.timeScale = isPaused ? 0f : 1f;
        settingObjects.SetActive(isPaused);
        isMouseEnabled = isPaused;
    }

    public void GameStateCheck()
    {
        if (!GlobalDeclare.bLotusGameComplete &&
             m_bPlayLotusEnable &&
             currentScene.name == "2 Grandma House")
        {
            ShowHint(HintItemID.S1_Lotus_Paper);
        }
    }

    public void ShowItemObject(GameObject objToShow)
    {
        // 將指定的物件顯示在 itemObjTransform 的位置
        // 假設你想將該物件作為子物件添加到 itemObjTransform 下
        // 你可以使用 Instantiate 或 SetParent 方法
        //Instantiate(objToShow, itemObjTransform.position, itemObjTransform.rotation, itemObjTransform);
        // 如果需要調整物件的位置、縮放等屬性，可以在這裡進行設置
        //}
    }

    public void DestroyImmediateLo()
    {
        RO_OBJ[1].SetActive(false);
    }

    private void SortRO_OBJByName()
    {
        Array.Sort(RO_OBJ, CompareGameObjectNames);
    }

    private int CompareGameObjectNames(GameObject x, GameObject y)
    {
        string[] names = { "Rice_Funeral", "Lotus_Paper", "Photo_Frame" };

        int xIndex = Array.IndexOf(names, x.name);
        int yIndex = Array.IndexOf(names, y.name);

        return xIndex.CompareTo(yIndex);
    }

    public bool GetM_bInUIView()
    {
        return m_bInUIView;
    }

    public void StopReadding()
    {
        playerCtrlr.m_bCanControl = false;
        playerCtrlr.m_bLimitRotation = true;
        StartCoroutine(ChangeVignetteIntensity());
    }

    private IEnumerator ChangeVignetteIntensity()
    {
        VolumeProfile profile = postProcessVolume.sharedProfile;

        if (profile.TryGet(out Vignette vignette) &&
            profile.TryGet(out CloudLayer cloudLayer))
        {
            float currentIntensity = 0.738f;
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
            vignette.intensity.value = 0.64f;
            vignette.smoothness.value = 0.168f;
            vignette.roundness.value = 0.184f;
            cloudLayer.opacity.value = 0.0f;
            playerCtrlr.m_bCanControl = true;
            playerCtrlr.m_bLimitRotation = false;
        }
    }

    void LastAnimateAfterPhotoFrame()
    {
        // 有東西竄動的聲音
        //AUDManager.instance.TheSoundOfSomethingMoving();
        AUDManager.instance.BodyTwistingSound();

        // 聲音大約出現 2-3 秒後安靜下來
        Invoke(nameof(IvkS2_SlientAfterPhotoFrame), 2f);

        // 奶奶突然出現 >> 黑畫面 >> 嬤來魔的標題
        FinalUI.SetActive(true);
    }

    void LastAnimateAfterPhotoFrameForRecord()
    {
        ProcessPlayerAnimator("Player_S2_Shocked_After_PhotoFrame");

        AUDManager.instance.BodyTwistingSound();

        Invoke(nameof(IvkS2_SlientAfterPhotoFrameForRecord), 20f);
    }

    IEnumerator DelayedAction()
    {
        yield return new WaitForSeconds(2.5f);
    }

    IEnumerator DelayLodelobby()
    {
        AUDManager.instance.HorrorStart();
        FinalUI.SetActive(true);
        SceneManager.LoadScene(0);
        yield return null;
    }
}