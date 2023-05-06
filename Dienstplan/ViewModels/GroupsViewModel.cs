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

    [ObservableProperty]
    private Visibility editGridVisibility = Visibility.Collapsed;
    [ObservableProperty]
    private bool gridIsEnabled = true;
    [ObservableProperty]
    private ObservableCollection<Group> groups;
    [ObservableProperty]
    private Group selectedItem;
    [ObservableProperty]
    private string newName;
    [ObservableProperty]
    private GroupType newType;
   
    private bool isAdd = true;
    private readonly List<Group> newGroups = new List<Group>();

    [RelayCommand]
    private void AddGroup(object param)
    {
        NewName = "";
        NewType = GroupType.Small;

        EditGridVisibility = Visibility.Visible;
        GridIsEnabled = false;
        isAdd = true;
    }
    [RelayCommand]
    private void DeleteGroup(object param)
    {
        if (selectedItem is null)
            return;

        selectedItem.IsOut = true;
        groups.Remove(selectedItem);
    }
    [RelayCommand]
    private void UpdateGroup(object param)
    {
        if (selectedItem is null)
            return;

        newName = selectedItem.Name;
        newType = selectedItem.Type;
        
        editGridVisibility = Visibility.Visible;
        gridIsEnabled = false;
        isAdd = false;
    }
    [RelayCommand]
    private void Save(object param)
    {
        SaveGroups?.Invoke(this, newGroups);
        newGroups.Clear();
    }
    [RelayCommand]
    private void Okay(object param)
    {
        if (string.IsNullOrWhiteSpace(newName))
            return;

        if (isAdd)
        {
            Group newGroup = new Group();
            newGroup.Name = newName;
            newGroup.Type = newType;
            groups.Add(newGroup);
            newGroups.Add(newGroup);
        }
        else 
        {
            selectedItem.Name = newName;
            selectedItem.Type = newType;

            groups = new ObservableCollection<Group>(groups);
        }

        gridIsEnabled = true;
        editGridVisibility = Visibility.Collapsed;
    }
    [RelayCommand]
    private void Cancle(object param)
    {
        gridIsEnabled = true;
        editGridVisibility = Visibility.Collapsed;
    }
}
