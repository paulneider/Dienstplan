using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Documents;

namespace Dienstplan;

internal class RosterViewModel : VMBase
{
    public Visibility Visibility
    {
        get => GetValue(Visibility.Collapsed);
        set => SetValue(value);
    }

    public ObservableCollection<Employee> Employees 
    {
        get => GetValue(new ObservableCollection<Employee>());
        set => SetValue(value);
    }

    private Roster roster;

    public void InitCreate(DateOnly start, DateOnly end)
    {
        // Testing
        if (start.DayOfWeek != DayOfWeek.Monday)
            throw new ArgumentException("start must be a monday", nameof(start));

        if (end.DayOfWeek != DayOfWeek.Friday)
            throw new ArgumentException("end must be a friday", nameof(end));

        roster = new Roster();
        roster.Start = start;
        roster.End = end;

        Day monday = new Day();
        monday.Date = start;
        roster.Days.Add(monday);

        Day tuesday = new Day();
        tuesday.Date = start.AddDays(1);
        roster.Days.Add(tuesday);

        Day wednesday = new Day();
        wednesday.Date = start.AddDays(2);
        roster.Days.Add(wednesday);

        Day thursday = new Day();
        thursday.Date = start.AddDays(3);
        roster.Days.Add(thursday);

        Day friday = new Day();
        friday.Date = start.AddDays(4);
        roster.Days.Add(friday);

        foreach (Employee employee in Employees)
        {
            Shift mondayShift = new Shift();
            mondayShift.Day = monday;
            mondayShift.Employee = employee;

            monday.Shifts.Add(mondayShift);
            Monday.Add(new ShiftViewModel(mondayShift));

            Shift tuesdayShift = new Shift();
            tuesdayShift.Day = tuesday;
            tuesdayShift.Employee = employee;

            tuesday.Shifts.Add(tuesdayShift);
            Tuesday.Add(new ShiftViewModel(tuesdayShift));

            Shift wednesdayShift = new Shift();
            wednesdayShift.Day = wednesday;
            wednesdayShift.Employee = employee;

            wednesday.Shifts.Add(wednesdayShift);
            Wednesday.Add(new ShiftViewModel(wednesdayShift));

            Shift thursdayShift = new Shift();
            thursdayShift.Day = thursday;
            thursdayShift.Employee = employee;

            thursday.Shifts.Add(thursdayShift);
            Thursday.Add(new ShiftViewModel(thursdayShift));

            Shift fridayShift = new Shift();
            fridayShift.Day = friday;
            fridayShift.Employee = employee;

            friday.Shifts.Add(fridayShift);
            Friday.Add(new ShiftViewModel(fridayShift));
        }
    }

    public void InitUpdate(Roster roster)
    {
        this.roster = roster;

        //docs = docs.OrderBy(d => docsIds.IndexOf(d.Id)).ToList(); --> join
        Day monday = roster.Days.Single(x=> x.Date.DayOfWeek == DayOfWeek.Monday);
        foreach (Shift shift in monday.Shifts)
            Monday.Add(new ShiftViewModel(shift));

        Day tuesday = roster.Days.Single(x => x.Date.DayOfWeek == DayOfWeek.Tuesday);
        foreach (Shift shift in tuesday.Shifts)
            Tuesday.Add(new ShiftViewModel(shift));

        Day wednesday = roster.Days.Single(x => x.Date.DayOfWeek == DayOfWeek.Wednesday);
        foreach (Shift shift in wednesday.Shifts)
            Wednesday.Add(new ShiftViewModel(shift));

        Day thursday = roster.Days.Single(x => x.Date.DayOfWeek == DayOfWeek.Thursday);
        foreach (Shift shift in thursday.Shifts)
            Thursday.Add(new ShiftViewModel(shift));

        Day friday = roster.Days.Single(x => x.Date.DayOfWeek == DayOfWeek.Friday);
        foreach (Shift shift in friday.Shifts)
            Friday.Add(new ShiftViewModel(shift));
    }

    public ObservableCollection<ShiftViewModel> Monday { get; } = new ObservableCollection<ShiftViewModel>();
    public ObservableCollection<ShiftViewModel> Tuesday { get; } = new ObservableCollection<ShiftViewModel>();
    public ObservableCollection<ShiftViewModel> Wednesday { get; } = new ObservableCollection<ShiftViewModel>();
    public ObservableCollection<ShiftViewModel> Thursday { get; } = new ObservableCollection<ShiftViewModel>();
    public ObservableCollection<ShiftViewModel> Friday { get; } = new ObservableCollection<ShiftViewModel>();
}
