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

    public EmployeesViewModel EmployeesViewModel { get; init; } = new EmployeesViewModel();
    public GroupsViewModel GroupsViewModel { get; init; } = new GroupsViewModel();

    public MainViewModel()
    {
        GroupsViewModel.SaveAndClose += GroupsViewModel_SaveAndClose;
        EmployeesViewModel.SaveAndClose += EmployeesViewModel_SaveAndClose;

        context.Database.EnsureCreated();
        context.Groups.Load();
        context.Employees.Load();
    }

    private void EmployeesViewModel_SaveAndClose(object? sender, IList<Employee> newItems)
    {
        StackPanelVisibility = Visibility.Visible;
        EmployeesViewModel.Visibility = Visibility.Collapsed;
        context.Employees.AddRange(newItems);
        context.SaveChanges();
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
        EmployeesViewModel.Employees = new ObservableCollection<Employee>(context.Employees.Where(x => !x.IsOut));
        EmployeesViewModel.Groups = new ObservableCollection<Group>(context.Groups.Where(x => !x.IsOut));

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
