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

    public readonly static string[] UITitle = new string[]
    {
        "",
        "阿嬤的相框",
        "折蓮花專用紙"
    };

    public readonly static string[] UIIntroduce = new string[]
    {
        "",
        "阿夜阿嬤飛上天",
        "摺紙蓮花源自觀世音菩薩所乘之蓮座衍生而來，\r\n" +
        "往生親人乘坐著蓮花，在往西方極樂世界的路途中，\r\n" +
        "能走得更加安穩與平順。\r\n" +
        "在世的人摺紙蓮花給往生的先人，\r\n" +
        "能夠消除業障，渡化其冤親債主，脫離苦難。",
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
    Close_UI = 0,

    // S1 : Scene_1
    S1_Photo_Frame,
    S1_Grandma_Door_Open,
    S1_Lotus_Paper,
}

public enum UIItemID
{
    Empty = 0,

    // S1 : Scene_1
    S1_Photo_Frame,
    S1_Lotus_Paper,
}

public enum ButtonEventID
{
    UI_Back,
    Enter_Game,
}