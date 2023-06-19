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
    S1_Flashlight,
    S1_DrawerWithKey,
    S1_GrandmaRoomKey,
}

public enum ActivateItemID
{
    Light_Switch,
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
