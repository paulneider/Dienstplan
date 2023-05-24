namespace Dienstplan;

internal class NewGroupMessage
{
    public Group Group { get; set; }
    public NewGroupMessage(Group group)
    {
        Group = group;
    }
}
