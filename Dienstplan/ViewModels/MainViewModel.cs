﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace Dienstplan;

internal class MainViewModel
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
        context.Days.Load();
        context.Rosters.Load();
        context.Shifts.Load();

        GroupsViewModel.SaveGroups += SaveGroups;
        EmployeesViewModel.SaveEmployees += SaveEmployees;
        RosterViewModel.SaveRoster += SaveRoster;
        RosterViewModel.WeekSelectorViewModel.NewWeekSelected += NewWeekSelected;

        EmployeesViewModel.Employees = new ObservableCollection<Employee>(context.Employees.Where(x => !x.IsOut));
        EmployeesViewModel.Groups = new ObservableCollection<Group>(context.Groups.Where(x => !x.IsOut));
        GroupsViewModel.Groups = new ObservableCollection<Group>(context.Groups.Where(x => !x.IsOut));

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

    private void NewWeekSelected(object? sender, DateTime newDate)
    {
        int dayIndex = 1 - ((int)newDate.DayOfWeek);
        DateOnly start = DateOnly.FromDateTime(newDate.AddDays(dayIndex == 1 ? -6 : dayIndex));

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

        EmployeesViewModel.Employees = new ObservableCollection<Employee>(context.Employees.Where(x => !x.IsOut));
    }

    private void SaveGroups(object? sender, IList<Group> newGroups)
    {
        context.Groups.AddRange(newGroups);
        context.SaveChanges();

        EmployeesViewModel.Groups = new ObservableCollection<Group>(context.Groups.Where(x => !x.IsOut));
        GroupsViewModel.Groups = new ObservableCollection<Group>(context.Groups.Where(x => !x.IsOut));
    }
}
