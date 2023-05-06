using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;

namespace Dienstplan;

internal partial class GroupsViewModel : ObservableObject
{
    public event EventHandler<IList<Group>> SaveGroups;

    private Visibility editGridVisibility = Visibility.Collapsed;
    public Visibility EditGridVisibility
    {
        get => editGridVisibility;
        set => SetProperty(ref editGridVisibility, value);
    }
    private bool gridIsEnabled = true;
    public bool GridIsEnabled
    {
        get => gridIsEnabled;
        set => SetProperty(ref gridIsEnabled, value);
    }
    private ObservableCollection<Group> groups;
    public ObservableCollection<Group> Groups
    {
        get => groups;
        set => SetProperty(ref groups, value);
    }
    private Group selectedItem;
    public Group SelectedItem
    {
        get => selectedItem;
        set => SetProperty(ref selectedItem, value);
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

    private bool isAdd = true;
    private readonly List<Group> newGroups = new List<Group>();
    public ICommand AddGroupCommand => new RelayCommand(AddGroup);
    public ICommand DeleteGroupCommand => new RelayCommand(DeleteGroup);
    public ICommand UpdateGroupCommand => new RelayCommand(UpdateGroup);
    public ICommand SaveCommand => new RelayCommand(Save);
    public ICommand OkayCommand => new RelayCommand(Okay);
    public ICommand CancleCommand => new RelayCommand(Cancle);

    private void AddGroup()
    {
        NewName = "";
        NewType = GroupType.Small;

        EditGridVisibility = Visibility.Visible;
        GridIsEnabled = false;
        isAdd = true;
    }
    private void DeleteGroup()
    {
        if (SelectedItem is null)
            return;

        SelectedItem.IsOut = true;
        Groups.Remove(SelectedItem);
    }
    private void UpdateGroup()
    {
        if (SelectedItem is null)
            return;

        NewName = SelectedItem.Name;
        NewType = SelectedItem.Type;
        
        EditGridVisibility = Visibility.Visible;
        GridIsEnabled = false;
        isAdd = false;
    }
    private void Save()
    {
        SaveGroups?.Invoke(this, newGroups);
        newGroups.Clear();
    }
    private void Okay()
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
    private void Cancle()
    {
        GridIsEnabled = true;
        EditGridVisibility = Visibility.Collapsed;
    }
}
