using UnityEngine;

[RequireComponent(typeof(Outline))]
public class ItemController : MonoBehaviour
{
    [Header("¹CÀ¸¨Æ¥ó")] public GameEventID eventID;

    public bool b_isActive;

    Outline outline;
    GameManager gameManager;
    GameObject InteractiveItem;
    Transform tfInteractiveItem;

    void Awake()
    {
        outline = GetComponent<Outline>();
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        InteractiveItem = gameObject.transform.GetChild(0).gameObject;
        tfInteractiveItem = InteractiveItem.transform;
    }

    void Start()
    {
        outline.OutlineMode = Outline.Mode.OutlineVisible;
        outline.OutlineWidth = 0;
    }

    public void LightOn(bool _open)
    {
        outline.OutlineWidth = _open ? 4 : 0;
    }

    public void SendGameEvent()
    {
        gameManager.GameEvent(eventID);
    }

    public void ShowInteractionPrompt(bool show)
    {
        InteractiveItem.SetActive(show);

        if (show)
            tfInteractiveItem.LookAt(Camera.main.transform);
    }
}
