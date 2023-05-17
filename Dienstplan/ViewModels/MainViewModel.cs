using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;
using CommunityToolkit.Mvvm.Messaging.Messages;
using Microsoft.EntityFrameworkCore;

namespace Dienstplan;

internal class MainViewModel : ObservableRecipient, IRecipient<ValueChangedMessage<DateTime>>
{
    private readonly ApplicationDbContext context;

    public EmployeesViewModel EmployeesViewModel { get; init; } = new EmployeesViewModel();
    public GroupsViewModel GroupsViewModel { get; init; } = new GroupsViewModel();
    public RosterViewModel RosterViewModel { get; set; } = new RosterViewModel();

    public MainViewModel(ApplicationDbContext context, IMessenger messenger)
    {
        messenger.Register(this);
        this.context = context;

        GroupsViewModel.SaveGroups += SaveGroups;
        EmployeesViewModel.SaveEmployees += SaveEmployees;
        RosterViewModel.SaveRoster += SaveRoster;

        foreach (Employee employee in context.Employees.Where(x => !x.IsOut))
            EmployeesViewModel.Employees.Add(new EmployeeItemViewModel(employee));

        EmployeesViewModel.EditEmployeeViewModel.Groups = new ObservableCollection<Group>(context.Groups.Where(x => !x.IsOut));
        EmployeesViewModel.EditEmployeeViewModel.SelectedGroup = EmployeesViewModel.EditEmployeeViewModel.Groups.FirstOrDefault();

        foreach (Group group in context.Groups.Where(x => !x.IsOut))
            GroupsViewModel.Groups.Add(new GroupItemViewModel(group));

        Roster roster = context.Rosters.AsEnumerable().MaxBy(x => x.Start.DayNumber);
        if (roster is null)
        {
            DateOnly start = DateOnly.FromDateTime(DateTime.Today.AddDays(1 - ((int)DateTime.Today.DayOfWeek)));
            DateOnly end = start.AddDays(4);
            RosterViewModel.InitCreate(start, end, context.Employees.Where(x => !x.IsOut).ToList());
        }
        else
        {
            context.Entry(roster).Collection(b => b.Employees).Load();
            RosterViewModel.InitUpdate(roster);
        }
    }

    private void SaveRoster(object? sender, Roster? newRoster)
    {
        if (newRoster is not null)
            context.Rosters.Add(newRoster);
        
        context.SaveChanges();
    }
    private void SaveEmployees(object? sender, IList<Employee> newEmployees)
    {
        context.Employees.AddRange(newEmployees);
        context.SaveChanges();

        EmployeesViewModel.Employees.Clear();
        foreach (Employee employee in context.Employees.Where(x => !x.IsOut))
            EmployeesViewModel.Employees.Add(new EmployeeItemViewModel(employee));
    }
    private void SaveGroups(object? sender, IList<Group> newGroups)
    {
        context.Groups.AddRange(newGroups);
        context.SaveChanges();

        EmployeesViewModel.EditEmployeeViewModel.Groups = new ObservableCollection<Group>(context.Groups.Where(x => !x.IsOut));
        EmployeesViewModel.EditEmployeeViewModel.SelectedGroup = EmployeesViewModel.EditEmployeeViewModel.Groups.FirstOrDefault();

        GroupsViewModel.Groups.Clear();
        foreach (Group group in context.Groups.Where(x => !x.IsOut))
            GroupsViewModel.Groups.Add(new GroupItemViewModel(group));
    }

    public void Receive(ValueChangedMessage<DateTime> message)
    {
        int dayIndex = 1 - ((int)message.Value.DayOfWeek);
        DateOnly start = DateOnly.FromDateTime(message.Value.AddDays(dayIndex == 1 ? -6 : dayIndex));

        Roster roster = context.Rosters.AsEnumerable().FirstOrDefault(x => x.Start == start);
        if (roster is null)
        {
            DateOnly end = start.AddDays(4);
            RosterViewModel.InitCreate(start, end, context.Employees.Where(x => !x.IsOut).ToList());
        }
        else
        {
            context.Entry(roster).Collection(b => b.Employees).Load();
            RosterViewModel.InitUpdate(roster);
        }
    }
}
