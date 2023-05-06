using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Reflection.Metadata;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Input;

namespace Dienstplan;

internal class EditGroupViewModel : ObservableObject
{

    public List<Group> NewGroups { get; init; } = new List<Group>();

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

    public bool isAdd = true;
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
            //Groups.Add(newGroup);
            NewGroups.Add(newGroup);
        }
        else
        {
            //SelectedItem.Name = NewName;
            //SelectedItem.Type = NewType;

            //Groups = new ObservableCollection<Group>(Groups);
        }

        Visibility = Visibility.Collapsed;
    }
    private void Cancle()
    {
        Visibility = Visibility.Collapsed;
    }
}
