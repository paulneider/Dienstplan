using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace Dienstplan;

internal class RosterViewModel : VMBase
{
    public event EventHandler<Roster?>? SaveRoster;
    private Roster roster;
    public WeekSelectorViewModel WeekSelectorViewModel { get; init; } = new WeekSelectorViewModel();

    ObservableCollection<EmployerItemViewModel> employerItems = new ObservableCollection<EmployerItemViewModel>();
    public ObservableCollection<EmployerItemViewModel> EmployerItems
    {
        get => employerItems;
    }
    public ICommand SaveCommand => new Command(Save);
    public ICommand ResetCommand => new Command(Reset);
    public ICommand SelectWeekCommand => new Command(SelectWeek);
    public string TimeSpanString
    {
        get
        {
            if (roster is null)
                return string.Empty;

            return "Woche vom " + roster.Start.ToString("dd.MM") + " bis " + roster.End.ToShortDateString();
        }
    }
    public void InitCreate(DateOnly start, DateOnly end, List<Employee> employees)
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
        roster.Employees = employees;

        OnPropertChanged(nameof(TimeSpanString));
        EmployerItems.Clear();

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

        foreach (Employee employee in employees)
        {
            EmployerItemViewModel viewModel = new EmployerItemViewModel(employee, roster.Days.ToList());
            EmployerItems.Add(viewModel);
        }
    }
    public void InitUpdate(Roster roster)
    {
        this.roster = roster;
        EmployerItems.Clear();

        foreach (Employee employee in roster.Employees)
        {
            EmployerItemViewModel viewModel = new EmployerItemViewModel(employee, roster.Days.ToList());
            EmployerItems.Add(viewModel);
        }
    }
    public void Save(object param)
    {
        SaveRoster?.Invoke(this, roster.Id is null ? roster : null);
    }
    public void Reset(object param)
    {

    }
    public void SelectWeek(object param)
    {
        WeekSelectorViewModel.Visibility = Visibility.Visible;
        WeekSelectorViewModel.SelectedDate = roster.Start.ToDateTime(default);
    }
}
