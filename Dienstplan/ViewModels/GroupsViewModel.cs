using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace Dienstplan;

internal partial class GroupsViewModel : ObservableObject
{
    public event EventHandler<IList<Group>> SaveGroups;

    public EditGroupViewModel EditGroupViewModel { get; init; } = new EditGroupViewModel();
    private ObservableCollection<GroupItemViewModel> groups = new ObservableCollection<GroupItemViewModel>();
    public ObservableCollection<GroupItemViewModel> Groups
    {
        get => groups;
        set => SetProperty(ref groups, value);
    }
    private GroupItemViewModel selectedItem;
    public GroupItemViewModel SelectedItem
    {
        get => selectedItem;
        set => SetProperty(ref selectedItem, value);
    }
    private readonly List<Group> newGroups = new List<Group>();
    public ICommand AddGroupCommand => new RelayCommand(AddGroup);
    public ICommand DeleteGroupCommand => new RelayCommand(DeleteGroup);
    public ICommand UpdateGroupCommand => new RelayCommand(UpdateGroup);
    public ICommand SaveCommand => new RelayCommand(Save);

    public GroupsViewModel()
    {
        EditGroupViewModel.GroupAdded += GroupAdded;
    }

    private void GroupAdded(object sender, Group newGroup)
    {
        newGroups.Add(newGroup);
        Groups.Add(new GroupItemViewModel(newGroup));
    }
    private void AddGroup()
    {
        EditGroupViewModel.Add();
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

        EditGroupViewModel.Update(SelectedItem);
    }
    private void Save()
    {
        SaveGroups?.Invoke(this, newGroups);
        newGroups.Clear();
    }
}
