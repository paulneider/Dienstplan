using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;

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

    public RosterViewModel()
    {
        
    }
    public RosterViewModel(Roster roster)
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
