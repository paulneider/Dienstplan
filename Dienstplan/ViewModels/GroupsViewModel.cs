using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;

namespace Dienstplan;

internal class GroupsViewModel : VMBase
{
    public event EventHandler<IList<Group>> SaveAndClose;
    public Visibility Visibility
    {
        get => GetValue(Visibility.Collapsed);
        set => SetValue(value);
    }
    public Visibility EditGridVisibility
    {
        get => GetValue(Visibility.Collapsed);
        set => SetValue(value);
    }
    public bool GridIsEnabled
    {
        get => GetValue(true);
        set => SetValue(value);
    }
    public ObservableCollection<Group> Groups
    {
        get => GetValue<ObservableCollection<Group>>();
        set => SetValue(value);
    }
    public Group SelectedItem
    {
        get => GetValue<Group>();
        set => SetValue(value);
    }
    public string NewName
    {
        get => GetValue<string>();
        set => SetValue(value);
    }
    public GroupType NewType
    {
        get => GetValue<GroupType>();
        set => SetValue(value);
    }
    public ICommand AddGroupCommand => new Command(AddGroup);
    public ICommand DeleteGroupCommand => new Command(DeleteGroup);
    public ICommand UpdateGroupCommand => new Command(UpdateGroup);
    public ICommand SaveCommand => new Command(Save);
    public ICommand OkayCommand => new Command(Okay);
    public ICommand CancleCommand => new Command(Cancle);

    private bool isAdd = true;
    private readonly List<Group> newGroups = new List<Group>();

    private void AddGroup(object param)
    {
        NewName = "";
        NewType = GroupType.Small;

        EditGridVisibility = Visibility.Visible;
        GridIsEnabled = false;
        isAdd = true;
    }
    private void DeleteGroup(object param)
    {
        if (SelectedItem is null)
            return;

        SelectedItem.IsOut = true;
        Groups.Remove(SelectedItem);
    }
    private void UpdateGroup(object param)
    {
        if (SelectedItem is null)
            return;

        NewName = SelectedItem.Name;
        NewType = SelectedItem.Type;
        
        EditGridVisibility = Visibility.Visible;
        GridIsEnabled = false;
        isAdd = false;
    }
    private void Save(object param)
    {
        SaveAndClose?.Invoke(this, newGroups);
        newGroups.Clear();
    }
    private void Okay(object param)
    {
        if (string.IsNullOrWhiteSpace(NewName))
            return;

        if (isAdd)
        {
            Group newGroup = new Group();
            newGroup.Name = NewName;
            newGroup.Type = NewType;
            Groups.Add(newGroup);
            newGroups.Add(newGroup);
        }
        else 
        {
            SelectedItem.Name = NewName;
            SelectedItem.Type = NewType;

            Groups = new ObservableCollection<Group>(Groups);
        }

        GridIsEnabled = true;
        EditGridVisibility = Visibility.Collapsed;
    }
    private void Cancle(object param)
    {
        GridIsEnabled = true;
        EditGridVisibility = Visibility.Collapsed;
    }
}
