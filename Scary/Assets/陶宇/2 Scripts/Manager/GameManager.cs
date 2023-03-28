using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] [Header("玩家")] PlayerController player;

    [SerializeField] [Header("戶內傳送點")] Transform t_indoorPos;
    [SerializeField] [Header("戶外傳送點")] Transform t_outdoorPos;
    [SerializeField] [Header("鐵捲門物件")] Transform t_RollingDoor;

    [SerializeField] [Header("所有可互動物件")] ItemController[] items;

    Transform t_player;

    void Start()
    {
        Init();
    }

    void Init()
    {
        t_player = player.transform;
    }

    public void StoryEvent()
    {

    }

    public void GameEvent(GameEventID _eventID)
    {
        switch (_eventID)
        {
            case GameEventID.S1_Move_To_Indoor:
                t_player.position = t_indoorPos.position;
                break;
            case GameEventID.S1_Move_To_OutDoor:
                t_player.position = t_outdoorPos.position;
                break;
            case GameEventID.S1_RollingDoor_Up:
                t_RollingDoor.position += new Vector3(0, 4, 0);
                break;
        }
    }
}
