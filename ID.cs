using System;

public static class ID
{
    static int _groupRange = 10000;
    public enum ItemGroup
    {
        None = 0,
        Bottle = 1,
        Garnish = 2,
        Glass = 3,
        Tool = 4
    }
    public static ItemGroup GetGroup(int ID)
    {
        ItemGroup group = ItemGroup.None;
        int gid = ID / _groupRange;

        if (0 < gid && gid <= GetItemGroupEnumCount())
            group = (ItemGroup)gid;

        return group;
    }
    static int GetItemGroupEnumCount()
    {
        return Enum.GetNames(typeof(ItemGroup)).Length;
    }
}
