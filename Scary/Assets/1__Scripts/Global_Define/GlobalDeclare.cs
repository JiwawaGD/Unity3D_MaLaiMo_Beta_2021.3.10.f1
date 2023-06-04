public partial class GlobalDeclare
{
    public static bool bLotusGameComplete = false;
    public static bool bCanControl = true;

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

    public class ItemAnimate
    {
        public static string aniObject;
        public static string aniName;

        public ItemAnimate()
        {
            aniObject = "Empty";
            aniName = "Empty";
        }
    }

    public static void SetItemAniObject(string r_AniObject)
    {
        ItemAnimate.aniObject = r_AniObject;
    }

    public static string GetItemAniObject()
    {
        return ItemAnimate.aniObject;
    }

    public static void SetItemAniName(string r_AniName)
    {
        ItemAnimate.aniName = r_AniName;
    }

    public static string GetItemAniName()
    {
        return ItemAnimate.aniName;
    }

    public class DialogState
    {
        public static string sDialogObjName;

        public DialogState()
        {
            sDialogObjName = "Empty";
        }
    }

    public static void SetDialogObjName(string r_sName)
    {
        DialogState.sDialogObjName = r_sName;
    }

    public static string GetDialogObjName()
    {
        return DialogState.sDialogObjName;
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

        "摺紙蓮花源自觀世音菩薩所乘之蓮座衍生而來。\r\n" +
        "親人能乘坐蓮花到達西方極樂世界，\r\n" +
        "而在世的親人也能以\r\n" +
        "摺蓮花消除業障度化其冤親債主。\r\n" +
        "奶奶在世時人緣很好，\r\n" +
        "希望她能夠一切平安。",

        "奶奶身上有慢性病，\r\n" +
        "前幾年出了一場車禍讓原先身體\r\n" +
        "本來就有慢性病的身體更不好了。\r\n" +
        "車禍後的傷造成又有後遺症，\r\n" +
        "維持了幾年直到前幾天突然就走了。",

        "親人氣絕，孝眷哭畢，\r\n" +
        "即以一匹白布彎九彎，\r\n" +
        "懸掛於屍床之周圍，稱為「吊九條」，\r\n" +
        "俗稱「孝簾」。\r\n" +
        "吊九條目的，在遮日月光線照射，\r\n" +
        "以免死者變成僵屍。\r\n" +
        "除圍九條外，廳門尚須闔一扉，\r\n" +
        "屍在廳左闔左扉，在右闔右扉；\r\n" +
        "又男喪闔左扉，女喪闔右扉，\r\n" +
        "九條與門扉須圍闔至出殯為止。\r\n" +
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
    S1_Photo_Frame_Light_On,
    S1_Grandma_Rush,
    S1_Light_Switch,
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