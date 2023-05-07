using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Windows;
using System.Windows.Input;

namespace Dienstplan;

internal class EditGroupViewModel : ObservableObject
{
    public event EventHandler<Group> GroupAdded;

    private GroupItemViewModel updateGroup;
    private bool isAdd = false;

    public string Caption
    {
        get => isAdd ? "Neue Gruppe:" : "Gruppe bearbeiten:";
    }
    private Visibility visibility = Visibility.Collapsed;
    public Visibility Visibility
    {
        get => visibility;
        set => SetProperty(ref visibility, value);
    }
    private string newName;
    public string NewName
    {
        get => newName;
        set => SetProperty(ref newName, value);
    }
    private GroupType newType;
    public GroupType NewType
    {
        get => newType;
        set => SetProperty(ref newType, value);
    }
    public ICommand OkayCommand => new RelayCommand(Okay);
    public ICommand CancleCommand => new RelayCommand(Cancle);
    private void Okay()
    {
        if (string.IsNullOrWhiteSpace(NewName))
            return;

        if (isAdd)
        {
            Group newGroup = new Group();

            newGroup.Name = NewName;
            newGroup.Type = NewType;

            GroupAdded?.Invoke(this, newGroup);
        }
        else
        {
            updateGroup.Name = NewName;
            updateGroup.Type = NewType;
        }

        Visibility = Visibility.Collapsed;
    }
    private void Cancle()
    {
        Visibility = Visibility.Collapsed;
    }
    public void Add()
    {
        NewName = "";
        NewType = GroupType.Small;

        Visibility = Visibility.Visible;
        isAdd = true;
        OnPropertyChanged(nameof(Caption));
    }
    public void Update(GroupItemViewModel group)
    {
        NewName = group.Name;
        NewType = group.Type;

        updateGroup = group;

        Visibility = Visibility.Visible;
        isAdd = false;
        OnPropertyChanged(nameof(Caption));
    }
}
