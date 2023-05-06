using CommunityToolkit.Mvvm.ComponentModel;

namespace Dienstplan;

internal class GroupItemViewModel : ObservableObject
{
    private readonly Group group;
    public GroupItemViewModel(Group group)
    {
        this.group = group;
    }

    public string Name
    {
        get => group.Name;
        set
        {
            group.Name = value;
            OnPropertyChanged();
        }
    }
    public GroupType Type
    {
        get => group.Type;
        set
        {
            group.Type = value;
            OnPropertyChanged();
        }
    }
}
