public partial class GlobalDeclare
{
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
}