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
        "�ڪ������A�b�ګܤp���ɭԴN�L�@�F",
        "���ɭԪ��ڡA�٤����D�ڬݨ쪺�O�ƻ�",
    };

    public readonly static string[] UITitle = new string[]
    {
        "",
        "�������Ӥ�",
        "�齬��M�ί�",
        "����",
        "��î",
    };

    public readonly static string[] UIIntroduce = new string[]
    {
        "",

        "�����ͤ�ɪ����������窺�Ӥ��C",

        "�P�Ƚ��᷽���[�@�����ĩҭ������y�l�ͦӨӡA\r\n" +
        "���ͿˤH�����۽���A�b����跥�֥@�ɪ����~���A\r\n" +
        "�ਫ�o��[�wí�P�����C\r\n" +
        "�b�@���H�P�Ƚ��ᵹ���ͪ����H�A\r\n" +
        "��������~�١A��ƨ�޿˶ťD�A�����W���C",

        "�]�����C�ʯf�A���׫���Ĥ��V�A\r\n" +
        "�C�ʯf�z�o�ɭP���`�C",

        "�ˤH�𵴡A���������A�Y�H�@�ǥե��s�E�s�A�a����ͧɤ��P��A�٬��u�Q�E���v�A�U�١u��î�v�C\r\n" +
        "�Q�E���ت��A�b�B�����u�Ӯg�A�H�K�����ܦ����͡C\r\n" +
        "����E���~�A�U���|����@�v�A�ͦb�U���󥪴v�A�b�k��k�v�F\r\n" +
        "�S�k���󥪴v�A�k����k�v�A�E���P���v������ܥX�l����C\r\n" +
        "���̥H�����N���ե��A���b�j�����~�A���H�c���C"
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