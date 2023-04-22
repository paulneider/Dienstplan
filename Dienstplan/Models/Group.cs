namespace Dienstplan;

internal enum GroupType
{
    Small,
    Big
}
internal class Group
{
    public long Id { get; set; } = -1;
    public string Name { get; set; }
    public GroupType Type { get; set; }
    public bool IsOut { get; set; }
    public bool IsEdit { get; set; }
    //Name Versions
}
