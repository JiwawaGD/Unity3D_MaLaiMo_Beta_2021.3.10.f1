using UnityEngine;

[RequireComponent(typeof(Outline), typeof(BoxCollider))]
public class ItemController : MonoBehaviour
{
    [Header("遊戲事件")] 
    public GameEventID eventID;

    public bool bAlwaysActive; // 是否可以無限觸發
    public bool bActive;       // 是否可以觸發

    Outline outline;
    GameManager gameManager;
    GameObject InteractiveItem;
    Transform tfInteractiveItem;
    Transform tfPlayerCamera;

    void Awake()
    {
        GetFields();
    }

    void Start()
    {
        InteractiveItem.SetActive(false);
        outline.OutlineMode = Outline.Mode.OutlineAll;
        outline.OutlineWidth = 0;
    }

    public void GetFields()
    {
        if (outline == null)
            outline = GetComponent<Outline>();

        if (gameManager == null)
            gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();

        if (InteractiveItem == null)
            InteractiveItem = gameObject.transform.GetChild(0).gameObject;

        if (tfInteractiveItem == null)
            tfInteractiveItem = InteractiveItem.transform;

        gameObject.layer = LayerMask.NameToLayer("InteractiveItem");
    }

    public void SendGameEvent()
    {
        gameManager.GameEvent(eventID);

        HintState(false);

        ItemDisable();
    }

    public void HintState(bool r_bShow)
    {
        outline.OutlineWidth = r_bShow ? 6 : 0;

        InteractiveItem.SetActive(r_bShow);

        if (r_bShow)
        {
            if (tfPlayerCamera == null)
                tfPlayerCamera = GameObject.Find("Player Camera").transform;

            tfInteractiveItem.LookAt(tfPlayerCamera);
        }
    }

    public void ItemDisable()
    {
        if (bAlwaysActive)
            return;

        gameObject.layer = LayerMask.NameToLayer("Default");
        bActive = false;
    }
}