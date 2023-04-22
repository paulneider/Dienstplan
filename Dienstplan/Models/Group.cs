namespace Dienstplan;

internal enum GroupType
{
    Small,
    Big
}
internal class Group
{
    public long Id { get; set; }
    public string Name { get; set; }
    public GroupType Type { get; set; }
    public bool IsOut { get; set; }
    //Name History
}
