using UnityEngine;

[RequireComponent(typeof(Outline))]
public class ItemController : MonoBehaviour
{
    [Header("¹CÀ¸¨Æ¥ó")] public GameEventID eventID;

    GameManager gameManager;

    public bool b_isActive;
    [HideInInspector] public bool b_isOutline;
    [HideInInspector] public Outline outline;

    void Awake()
    {
        outline = GetComponent<Outline>();
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    void Start()
    {
        outline.OutlineMode = Outline.Mode.OutlineVisible;
        outline.OutlineWidth = 0;
    }

    public void LightOn(bool _open)
    {
        outline.OutlineWidth = _open ? 10 : 0;
    }

    public void SendGameEvent()
    {
        gameManager.GameEvent(eventID);
    }
}
