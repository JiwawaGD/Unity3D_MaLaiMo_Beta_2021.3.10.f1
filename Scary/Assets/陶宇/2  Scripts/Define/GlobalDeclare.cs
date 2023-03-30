using UnityEngine;

public class GlobalDeclare
{
    public enum ItemMessage
    {
        test_box,
    }

    public readonly static string[] StoryMessage = new string[]
    {
        "�ڪ������A�b�ګܤp���ɭԴN�L�@�F",
        "���ɭԪ��ڡA�٤����D�ڬݨ쪺�O�ƻ�",
    };

    public readonly static string[] UITitle = new string[]
    {
        "",
        "�������ۮ�",
    };

    public readonly static string[] UIIntroduce = new string[]
    {
        "",
        "���]�������W��",
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
}

public enum UIItemID
{
    Empty = 0,

    // S1 : Scene_1
    S1_Photo_Frame,
}