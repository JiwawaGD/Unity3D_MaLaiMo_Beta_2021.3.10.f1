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
        "�齬��M�ί�"
    };

    public readonly static string[] UIIntroduce = new string[]
    {
        "",
        "���]�������W��",
        "�P�Ƚ��᷽���[�@�����ĩҭ������y�l�ͦӨӡA\r\n" +
        "���ͿˤH�����۽���A�b����跥�֥@�ɪ����~���A\r\n" +
        "�ਫ�o��[�wí�P�����C\r\n" +
        "�b�@���H�P�Ƚ��ᵹ���ͪ����H�A\r\n" +
        "��������~�١A��ƨ�޿˶ťD�A�����W���C",
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