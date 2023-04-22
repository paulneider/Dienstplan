using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using Microsoft.EntityFrameworkCore;

namespace Dienstplan;
internal class MainViewModel : VMBase
{
    private readonly ModelContext context = new ModelContext();

    private Visibility stackPanelVisibility = Visibility.Visible;
    public Visibility StackPanelVisibility
    {
        get { return stackPanelVisibility; }
        set 
        {
            stackPanelVisibility = value;
            OnPropertChanged(nameof(StackPanelVisibility));
        }
    }

    public ICommand EditEmployeesCommand => new Command(EditEmployees);
    public ICommand EditGroupsCommand => new Command(EditGroups);

    public EmployeesViewModel EmployeesViewModel { get; init; }
    public GroupsViewModel GroupsViewModel { get; init; }


    public MainViewModel()
    {
        EmployeesViewModel = new EmployeesViewModel();
        GroupsViewModel = new GroupsViewModel();
        GroupsViewModel.SaveAndClose += GroupsViewModel_SaveAndClose;

        context.Database.EnsureCreated();
        context.Groups.Load();
    }

    private void GroupsViewModel_SaveAndClose(object? sender, IList<Group> newItems)
    {
        StackPanelVisibility = Visibility.Visible;
        GroupsViewModel.Visibility = Visibility.Collapsed;
        context.Groups.AddRange(newItems);
        context.SaveChanges();
    }

    private void EditEmployees(object param)
    {
        StackPanelVisibility = Visibility.Collapsed;
        EmployeesViewModel.Visibility = Visibility.Visible;
    }
    private void EditGroups(object param)
    {
        GroupsViewModel.Groups = new ObservableCollection<Group>(context.Groups.Where(x => !x.IsOut));
        StackPanelVisibility = Visibility.Collapsed;
        GroupsViewModel.Visibility = Visibility.Visible;
    }
}
