public class GlobalDeclare
{
    public class PlayerState
    {
        public static PlayerAnimateType aniType;

        public PlayerState()
        {
            aniType = PlayerAnimateType.Empty;
        }
    }

    public static void SetPlayerAnimateType(PlayerAnimateType r_AniType)
    {
        PlayerState.aniType = r_AniType;
    }

    public static PlayerAnimateType GetPlayerAnimateType()
    {
        return PlayerState.aniType;
    }


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
        "阿嬤的照片",
        "折蓮花專用紙",
        "阿嬤",
        "孝簾",
    };

    public readonly static string[] UIIntroduce = new string[]
    {
        "",

        "阿嬤生日時爸爸幫阿嬤拍的照片。",

        "摺紙蓮花源自觀世音菩薩所乘之蓮座衍生而來，\r\n" +
        "往生親人乘坐著蓮花，在往西方極樂世界的路途中，\r\n" +
        "能走得更加安穩與平順。\r\n" +
        "在世的人摺紙蓮花給往生的先人，\r\n" +
        "能夠消除業障，渡化其冤親債主，脫離苦難。",

        "因為有慢性病，車禍後用藥不慎，\r\n" +
        "慢性病爆發導致死亡。",

        "親人氣絕，孝眷哭畢，即以一匹白布彎九彎，懸掛於屍床之周圍，稱為「吊九條」，俗稱「孝簾」。\r\n" +
        "吊九條目的，在遮日月光線照射，以免死者變成僵屍。\r\n" +
        "除圍九條外，廳門尚須闔一扉，屍在廳左闔左扉，在右闔右扉；\r\n" +
        "又男喪闔左扉，女喪闔右扉，九條與門扉須圍闔至出殯為止。\r\n" +
        "今者以黃布代替白布，旨在隔離內外，防人惡之。"
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
    S1_Grandma_Dead_Body,
    S1_White_Tent,
}

public enum UIItemID
{
    Empty = 0,

    // S1 : Scene_1
    S1_Photo_Frame,
    S1_Lotus_Paper,
    S1_Grandma_Dead_Body,
    S1_White_Tent,
}

public enum ButtonEventID
{
    UI_Back,
    Enter_Game,
}

public enum PlayerAnimateType
{
    Empty,
    Player_Wake_Up,
    Player_Turn_After_Photo_Frame,
}