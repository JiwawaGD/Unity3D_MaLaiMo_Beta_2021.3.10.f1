using Fungus;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public partial class GameManager : MonoBehaviour
{
    [SerializeField][Header("玩家")] PlayerController playerCtrlr;
    [SerializeField][Header("UI 圖片庫")] Sprite[] UISprite;
    [SerializeField][Header("Flowchart")] GameObject[] flowchartObjects;
    [SerializeField][Header("音效撥放清單")] AudioClip[] audioClip;
    [SerializeField][Header("音效撥放器")]AudioSource[] audioSources;

    public int m_iGrandmaRushCount;

    Transform tfGrandmaGhost;
    Scene currentScene;

    #region Canvas Zone
    [SerializeField] GameObject goCanvas;
    [SerializeField] Image imgUIBackGround;
    [SerializeField] Image imgUIDisplay;
    //[SerializeField] Image titleImg;
    [SerializeField] Text txtTitle;

    [SerializeField] Image imgInstructions;
    [SerializeField] Text txtInstructions;
    //[SerializeField] Image imgScendInstructions;

    //[SerializeField] Image imgIntroduceBackground;
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

    public static bool m_bPhotoFrameLightOn = false;
    public static bool m_bGrandmaRush = false;
    public static bool m_bReturnToBegin = false;
    #endregion

    void Awake()
    {
        if (playerCtrlr == null)
            playerCtrlr = GameObject.Find("Player").GetComponent<PlayerController>();

        if (goCanvas == null)
            goCanvas = GameObject.Find("UI Canvas");

        imgUIBackGround = goCanvas.transform.GetChild(0).GetComponent<Image>();
        imgUIDisplay = goCanvas.transform.GetChild(1).GetComponent<Image>();
        //titleImg = goCanvas.transform.GetChild(2).GetComponent<Image>();
        txtTitle = goCanvas.transform.GetChild(2).GetComponent<Text>();

        imgInstructions = goCanvas.transform.GetChild(3).GetComponent<Image>();
        txtInstructions = goCanvas.transform.GetChild(3).GetComponentInChildren<Text>();
        //imgScendInstructions = goCanvas.transform.GetChild(2).GetComponentInChildren<Image>();

        //imgIntroduceBackground = goCanvas.transform.GetChild(4).GetComponent<Image>();
        txtIntroduce = goCanvas.transform.GetChild(4).GetComponentInChildren<Text>();

        ExitBtn = goCanvas.transform.GetChild(5).GetComponent<Button>();

        txtEnterGameHint = goCanvas.transform.GetChild(6).GetComponent<Text>();
        EnterGameBtn = goCanvas.transform.GetChild(7).GetComponent<Button>();

        currentScene = SceneManager.GetActiveScene();
    }

    void Start()
    {
        GameEvent(GameEventID.Close_UI);

        ExitBtn.onClick.AddListener(() => ButtonFunction(ButtonEventID.UI_Back));
        EnterGameBtn.onClick.AddListener(() => ButtonFunction(ButtonEventID.Enter_Game));
    }

    void Update()
    {
        KeyboardCheck();
        //if (m_bInUIView && Input.GetKeyDown(KeyCode.N))
        //{
        //    imgIntroduceBackground.gameObject.SetActive(true);
        //}
    }

    public void GameEvent(GameEventID _eventID)
    {
        switch (_eventID)
        {
            case GameEventID.Close_UI:
                UIState(UIItemID.Empty, false);
                ShowEnterGame(false);

                // UI 返回後執行玩家動畫
                if (m_bShowPlayerAnimate)
                    ProcessPlayerAnimator(GlobalDeclare.GetPlayerAnimateType().ToString());

                // UI 返回後執行 Item 動畫
                if (m_bShowItemAnimate)
                    ProcessAnimator(GlobalDeclare.GetItemAniObject(), GlobalDeclare.GetItemAniName());

                // UI 返回後執行 Fungus 對話
                if (m_bShowDialog)
                    ProcessDialog(GlobalDeclare.GetDialogObjName());

                GameStateCheck();
                break;
            case GameEventID.S1_Photo_Frame:
                UIState(UIItemID.S1_Photo_Frame, true);

                // Set player transform
                Transform tfPlayer = playerCtrlr.transform;
                tfPlayer.position = new Vector3(-4.5f, 0.8f, 1f);
                tfPlayer.rotation = Quaternion.Euler(0, 180, 0);
                tfPlayer.GetChild(0).rotation = Quaternion.Euler(0, 180, 0);

                // Show grandma
                tfGrandmaGhost = GameObject.Find("Grandma_Ghost").transform;
                ParticleSystem psMist_Partical = GameObject.Find("Mist_Partical").GetComponent<ParticleSystem>();
                tfGrandmaGhost.Translate(0f, 100f, 0f);
                psMist_Partical.Play();

                m_bGrandmaRush = true;
                m_bShowPlayerAnimate = true;
                GlobalDeclare.SetPlayerAnimateType(PlayerAnimateType.Player_Turn_After_Photo_Frame);
                break;
            case GameEventID.S1_Grandma_Door_Open:
                ProcessAnimator("Grandma Door", "DoorOpen");
                audioSources[0].PlayOneShot(audioClip[0]);
                break;
            case GameEventID.S1_Lotus_Paper:
                UIState(UIItemID.S1_Lotus_Paper, true);
                ShowEnterGame(true);
                break;
            case GameEventID.S1_Grandma_Dead_Body:
                UIState(UIItemID.S1_Grandma_Dead_Body, true);
                ItemController PhotoFrame = GameObject.Find("Photo Frame").GetComponent<ItemController>();
                PhotoFrame.bActive = true;
                m_bPhotoFrameLightOn = true;
                flowchartObjects[6].gameObject.SetActive(true);
                break;
            case GameEventID.S1_White_Tent:
                //UIState(UIItemID.S1_White_Tent, true);
                m_bShowItemAnimate = true;
                GlobalDeclare.SetItemAniObject("Filial_piety_curtain");
                GlobalDeclare.SetItemAniName("Filial_piety_curtain Open");
                BoxCollider curtain = GameObject.Find("Filial_piety_curtain").GetComponent<BoxCollider>();
                ProcessAnimator("Filial_piety_curtain", "Filial_piety_curtain Open");
                curtain.enabled = false;
                break;
            case GameEventID.S1_Photo_Frame_Light_On:
                goPhotoFrameLight.SetActive(true);
                m_bPhotoFrameLightOn = false;
                flowchartObjects[9].gameObject.SetActive(true);
                break;
            case GameEventID.S1_Grandma_Rush:
                InvokeRepeating(nameof(GrandMaRush), 0f, 0.025f);
                playerCtrlr.m_bCanControl = false;
                Animator AniGrandma = tfGrandmaGhost.GetComponent<Animator>();
                AniGrandma.SetBool("Grandma_Attack", true);
                m_bGrandmaRush = false;
                break;
            case GameEventID.S1_Light_Switch:
                flowchartObjects[2].gameObject.SetActive(true);
                break;
            case GameEventID.S1_Flashlight:
                Light playerFlashlight = playerCtrlr.tfPlayerCamera.GetComponent<Light>();
                playerFlashlight.enabled = true;
                GameObject FlashLight = GameObject.Find("flashlight");
                Destroy(FlashLight);
                break;
            case GameEventID.S1_DrawerWithKey:
                BoxCollider DrawerCollider = GameObject.Find("grandpa_desk/DrawerWithKey").GetComponent<BoxCollider>();
                DrawerCollider.enabled = false;
                ProcessAnimator("grandpa_desk/DrawerWithKey", "DrawerWithKey_Open");
                Invoke(nameof(IvkShowDoorKey), 0.5f);
                break;
            case GameEventID.S1_GrandmaRoomKey:
                ItemController GrandmaDoor = GameObject.Find("Door/Grandma Door").GetComponent<ItemController>();
                GrandmaDoor.bActive = true;
                flowchartObjects[3].gameObject.SetActive(true);
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
        imgUIDisplay.color = r_bEnable ? new Color(1, 1, 1, 1) : new Color(1, 1, 1, 0);
        imgInstructions.color = r_bEnable ? new Color(0, 0, 0, 1) : new Color(0, 0, 0, 0);
        int iItemID = (int)r_ItemID;

        imgUIDisplay.sprite = UISprite[iItemID];
        txtTitle.text = GlobalDeclare.UITitle[iItemID];
 
        txtIntroduce.text = GlobalDeclare.UIIntroduce[iItemID];
        txtInstructions.text = GlobalDeclare.TxtInstructionsmage[iItemID];

        //titleImg.color = r_bEnable ? new Color(63, 0, 0, .18f) : new Color(63, 0, 0, 0);
        //imgScendInstructions.color = r_bEnable ? new Color(255, 255, 255, 1) : new Color(255, 255, 255, 0);
        //imgIntroduceBackground.color = r_bEnable ? new Color(108, 106, 106, 1) : new Color(108, 106, 106, 0);
        //imgIntroduceBackground.gameObject.SetActive(false);
    }

    public void ProcessAnimator(string r_sObject, string r_sTriggerName)
    {
        if (r_sObject.Contains("null") || r_sTriggerName.Contains("null"))
            return;

        GameObject obj = GameObject.Find(r_sObject);
        Animator ani = obj.transform.GetComponent<Animator>();
        ani.SetTrigger(r_sTriggerName);
        obj.transform.GetComponent<ItemController>().HintState(false);
        obj.transform.GetComponent<ItemController>().bActive = false;

        GlobalDeclare.SetItemAniObject("Empty");
        GlobalDeclare.SetItemAniName("Empty");
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
                GlobalDeclare.bLotusGameComplete = true;
                GameEvent(GameEventID.Close_UI);
                playerCtrlr.m_bCanControl = false;
                playerCtrlr.tfPlayerCamera.gameObject.SetActive(false);
                SceneManager.LoadScene(2, LoadSceneMode.Additive);
                break;
        }
    }

    public void GrandMaRush()
    {
        tfGrandmaGhost.Translate(0f, 0f, 0.3f);
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
        playerCtrlr.m_bCanControl = true;
        playerCtrlr.tfPlayerCamera.gameObject.SetActive(true);
        SceneManager.UnloadSceneAsync(2);

        Object LotusObj = Resources.Load<GameObject>("Prefabs/Lotus_state_Final");
        GameObject LotusGO = Instantiate(LotusObj) as GameObject;
        LotusGO.transform.position = new Vector3(-5.2f, 0.6f, -2.4f);

        GameObject LotusDestory = GameObject.Find("Lotus Paper");
        LotusDestory.transform.position = new Vector3(-5f, -2f, -2f);

        //ProcessDialog("Flowchart (3)");
        flowchartObjects[5].gameObject.SetActive(true);
    }

    public void KeyboardCheck()
    {
        if (m_bReturnToBegin)
        {
            if (Input.GetKeyDown(KeyCode.F10))
            {
                playerCtrlr.SetCursor();
                SceneManager.LoadScene(0);
                m_bReturnToBegin = false;
            }
        }
    }

    public void GameStateCheck()
    {
        if (!GlobalDeclare.bLotusGameComplete && currentScene.name == "Grandma House")
        {
            ItemController LotusItem = GameObject.Find("Lotus Paper").GetComponent<ItemController>();
            LotusItem.bActive = true;
        }
    }
}