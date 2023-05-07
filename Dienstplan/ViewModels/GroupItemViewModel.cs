using CommunityToolkit.Mvvm.ComponentModel;

namespace Dienstplan;

internal class GroupItemViewModel : ObservableObject
{
    private readonly Group group;
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
    public bool IsOut
    {
        get => group.IsOut;
        set
        {
            group.IsOut = value;
            OnPropertyChanged();
        }
    }
    public GroupItemViewModel(Group group)
    {
        this.group = group;
    }
}
