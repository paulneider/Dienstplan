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

    public EditGroupViewModel EditGroupViewModel { get; init; } = new EditGroupViewModel();

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

    private bool isAdd = true;
    private readonly List<Group> newGroups = new List<Group>();
    public ICommand AddGroupCommand => new RelayCommand(AddGroup);
    public ICommand DeleteGroupCommand => new RelayCommand(DeleteGroup);
    public ICommand UpdateGroupCommand => new RelayCommand(UpdateGroup);
    public ICommand SaveCommand => new RelayCommand(Save);

    private void AddGroup()
    {
        EditGroupViewModel.NewName = "";
        EditGroupViewModel.NewType = GroupType.Small;

        EditGroupViewModel.Visibility = Visibility.Visible;
        EditGroupViewModel.isAdd = true;
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

        EditGroupViewModel.NewName = SelectedItem.Name;
        EditGroupViewModel.NewType = SelectedItem.Type;
        
        EditGroupViewModel.Visibility = Visibility.Visible;
        EditGroupViewModel.isAdd = false;
    }
    private void Save()
    {
        SaveGroups?.Invoke(this, newGroups);
        newGroups.Clear();
    }
}
