namespace Dienstplan;

internal class UpdateGroupMessage
{
    public GroupItemViewModel GroupItem { get; set; }
    public UpdateGroupMessage(GroupItemViewModel groupItem)
    {
        GroupItem = groupItem;
    }
}
