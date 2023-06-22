using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class ItemController : MonoBehaviour
{
    [Header("遊戲事件")]
    public GameEventID eventID;

    [Header("是否可以無限觸發(裝飾物件)")]
    public bool bAlwaysActive;

    [Header("是否可以觸發(劇情物件)")]
    public bool bActive;

    #region UI
    GameObject HintObj;     // 眼睛 UI
    Transform tfHint;

    GameObject InteractObj; // 提示可互動 UI
    Transform tfInteract;
    #endregion

    GameManager gameManager;
    Transform tfPlayerCamera;

    void Awake()
    {
        GetFields();
    }

    void Start()
    {
        Initialize();
    }

    void GetFields()
    {
        if (HintObj == null)
            HintObj = gameObject.transform.GetChild(0).GetChild(0).gameObject;

        if (tfHint == null)
            tfHint = HintObj.transform;

        if (InteractObj == null)
            InteractObj = gameObject.transform.GetChild(0).GetChild(1).gameObject;

        if (tfInteract == null)
            tfInteract = InteractObj.transform;

        if (gameManager == null)
            gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();

        if (tfPlayerCamera == null)
            tfPlayerCamera = GameObject.Find("Player Camera").transform;
    }

    void Initialize()
    {
        HintObj.SetActive(false);
        InteractObj.SetActive(false);
        gameObject.layer = LayerMask.NameToLayer("InteractiveItem");
    }

    public void SetItemInteractive(bool r_bShow)
    {
        InteractObj.SetActive(r_bShow);

        if (r_bShow)
        {
            tfHint.LookAt(tfPlayerCamera);
            tfInteract.LookAt(tfPlayerCamera);
        }
    }

    public void SetHintable(bool r_bShow)
    {
        HintObj.SetActive(r_bShow);
    }

    public void SendGameEvent()
    {
        gameManager.GameEvent(eventID);
        ItemDisable();
    }

    void ItemDisable()
    {
        SetItemInteractive(false);

        if (bAlwaysActive)
            return;

        SetHintable(false);
        gameObject.layer = LayerMask.NameToLayer("Default");
        bActive = false;
    }
}