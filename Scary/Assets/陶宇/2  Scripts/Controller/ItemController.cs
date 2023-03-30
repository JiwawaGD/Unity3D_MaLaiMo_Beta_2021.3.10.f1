using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Outline),typeof(BoxCollider))]
public class ItemController : MonoBehaviour
{
    [Header("¹CÀ¸¨Æ¥ó")] public GameEventID eventID;

    public bool b_isActive;

    Outline outline;
    GameManager gameManager;
    GameObject InteractiveItem;
    Transform tfInteractiveItem;
    Transform tfPlayerCamera;

    void Awake()
    {
        outline = GetComponent<Outline>();
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        InteractiveItem = gameObject.transform.GetChild(0).GetChild(0).gameObject;
        tfInteractiveItem = InteractiveItem.transform;
    }

    void Start()
    {
        gameObject.layer = LayerMask.NameToLayer("InteractiveItem");
        InteractiveItem.GetComponent<Image>().enabled = false;
        outline.OutlineMode = Outline.Mode.OutlineVisible;
        outline.OutlineWidth = 0;
    }

    public void SendGameEvent()
    {
        gameManager.GameEvent(eventID);
    }

    public void HintState(bool r_bShow)
    {
        outline.OutlineWidth = r_bShow ? 4 : 0;

        InteractiveItem.GetComponent<Image>().enabled = r_bShow;

        if (r_bShow)
        {
            if (tfPlayerCamera == null)
                tfPlayerCamera = GameObject.Find("Player Camera").transform;

            tfInteractiveItem.LookAt(tfPlayerCamera);
        }
    }
}
