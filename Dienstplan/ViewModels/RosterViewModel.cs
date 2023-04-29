using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace Dienstplan;

internal class RosterViewModel : VMBase
{
    public event EventHandler<Roster> SaveRoster;
    private bool isCreate = false;
    private Roster roster;
    public ObservableCollection<Employee> Employees 
    {
        get => GetValue(new ObservableCollection<Employee>());
        set => SetValue(value);
    }
    ObservableCollection<EmployerItemViewModel> employerItems = new ObservableCollection<EmployerItemViewModel>();
    public ObservableCollection<EmployerItemViewModel> EmployerItems
    {
        get => employerItems;
    }
    public ICommand SaveCommand => new Command(Save);
    public ICommand ResetCommand => new Command(Reset);
    public string TimeSpanString
    {
        get
        {
            if (roster is null)
                return string.Empty;

            return "Woche vom " + roster.Start.ToString("dd.MM") + " bis " + roster.End.ToShortDateString();
        }
    }
    public void InitCreate(DateOnly start, DateOnly end)
    {
        // Testing
        if (start.DayOfWeek != DayOfWeek.Monday)
            throw new ArgumentException("start must be a monday", nameof(start));

        if (end.DayOfWeek != DayOfWeek.Friday)
            throw new ArgumentException("end must be a friday", nameof(end));

        if (start.AddDays(4) != end)
            throw new ArgumentException("end must be 4 days after start");

        roster = new Roster();
        roster.Start = start;
        roster.End = end;

        isCreate = true;
        OnPropertChanged(nameof(TimeSpanString));

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
            EmployerItemViewModel viewModel = new EmployerItemViewModel(employee, roster.Days.ToList());
            EmployerItems.Add(viewModel);
        }
    }
    public void InitUpdate(Roster roster)
    {
        this.roster = roster;
        isCreate = false;

        foreach (Employee employee in Employees)
            EmployerItems.Add(new EmployerItemViewModel(employee, roster.Days.ToList()));
    }

    public void Save(object param)
    {
        SaveRoster?.Invoke(this, isCreate ? roster : null);
        roster = null;
        EmployerItems.Clear();
    }
    public void Reset(object param)
    {

    }
}
