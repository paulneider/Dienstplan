using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using Microsoft.EntityFrameworkCore;

namespace Dienstplan;

internal class MainViewModel : VMBase
{
    private readonly ApplicationDbContext context = new ApplicationDbContext();

    public EmployeesViewModel EmployeesViewModel { get; init; } = new EmployeesViewModel();
    public GroupsViewModel GroupsViewModel { get; init; } = new GroupsViewModel();
    public RosterViewModel RosterViewModel { get; set; } = new RosterViewModel();

    public MainViewModel()
    {
        context.Database.EnsureCreated();
        context.Groups.Load();
        context.Employees.Load();

        GroupsViewModel.SaveGroups += SaveGroups;
        EmployeesViewModel.SaveEmployees += SaveEmployees;
        RosterViewModel.SaveRoster += SaveRoster;

        EmployeesViewModel.Employees = new ObservableCollection<Employee>(context.Employees.Where(x => !x.IsOut));
        EmployeesViewModel.Groups = new ObservableCollection<Group>(context.Groups.Where(x => !x.IsOut));
        GroupsViewModel.Groups = new ObservableCollection<Group>(context.Groups.Where(x => !x.IsOut));
        RosterViewModel.Employees = new ObservableCollection<Employee>(context.Employees.Where(x => !x.IsOut));
        RosterViewModel.InitCreate(new DateOnly(2023, 4, 24), new DateOnly(2023, 4, 28));
    }

    private void SaveRoster(object? sender, Roster e)
    {
        //Save
    }

    private void SaveEmployees(object? sender, IList<Employee> newItems)
    {
        context.Employees.AddRange(newItems);
        context.SaveChanges();

        RosterViewModel.Employees = new ObservableCollection<Employee>(context.Employees.Where(x => !x.IsOut));
        EmployeesViewModel.Employees = new ObservableCollection<Employee>(context.Employees.Where(x => !x.IsOut));
    }

    private void SaveGroups(object? sender, IList<Group> newItems)
    {
        context.Groups.AddRange(newItems);
        context.SaveChanges();

        EmployeesViewModel.Groups = new ObservableCollection<Group>(context.Groups.Where(x => !x.IsOut));
        GroupsViewModel.Groups = new ObservableCollection<Group>(context.Groups.Where(x => !x.IsOut));
    }
}
