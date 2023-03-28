using UnityEngine;

public class GlobalDeclare
{
    public enum ItemMessage
    {
        test_box,
    }

    public readonly static string[] StoryMessage = new string[]
    {
        "我的阿嬤，在我很小的時候就過世了",
        "那時候的我，還不知道我看到的是甚麼",
    };
}

public enum StoryID
{
    // S1 : Scene_1
    S1_0_0,
    S1_0_1,
}

public enum GameEventID
{
    // S1 : Scene_1
    S1_Move_To_Indoor,
    S1_Move_To_OutDoor,
    S1_RollingDoor_Up,
    S1_RollingDoor_Jump,
}
