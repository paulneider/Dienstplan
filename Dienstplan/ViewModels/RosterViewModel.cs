using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using CommunityToolkit.Mvvm.Messaging.Messages;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;

namespace Dienstplan;

internal class RosterViewModel : ObservableObject, IRecipient<ValueChangedMessage<DateTime>>
{
    private readonly ApplicationDbContext context;
    private readonly IMessenger messenger;
    private Roster roster;
    ObservableCollection<RosterEmployeeItemViewModel> employerItems = new ObservableCollection<RosterEmployeeItemViewModel>();
    public ObservableCollection<RosterEmployeeItemViewModel> EmployerItems
    {
        get => employerItems;
    }
    public string TimeSpanString
    {
        get
        {
            if (roster is null)
                return string.Empty;

            return "Woche vom " + roster.Start.ToString("dd.MM") + " bis " + roster.End.ToShortDateString();
        }
    }
    public ICommand SaveCommand => new RelayCommand(Save);
    public ICommand ResetCommand => new RelayCommand(Reset);
    public ICommand SelectWeekCommand => new RelayCommand(SelectWeek);
    public RosterViewModel() { }
    public RosterViewModel(ApplicationDbContext context, IMessenger messenger)
    {
        this.context = context;
        this.messenger = messenger;

        messenger.Register(this);

        Roster roster = context.Rosters.AsEnumerable().MaxBy(x => x.Start.DayNumber);
        if (roster is null)
        {
            DateOnly start = DateOnly.FromDateTime(DateTime.Today.AddDays(1 - ((int)DateTime.Today.DayOfWeek)));
            DateOnly end = start.AddDays(4);
            InitCreate(start, end, context.Employees.Where(x => !x.IsOut).ToList());
        }
        else
        {
            context.Entry(roster).Collection(b => b.Employees).Load();
            InitUpdate(roster);
        }
    }
    private void InitCreate(DateOnly start, DateOnly end, List<Employee> employees)
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

        OnPropertyChanged(nameof(TimeSpanString));
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
            RosterEmployeeItemViewModel viewModel = new RosterEmployeeItemViewModel(employee, roster.Days.ToList());
            EmployerItems.Add(viewModel);
        }
    }
    private void InitUpdate(Roster roster)
    {
        this.roster = roster;

        OnPropertyChanged(nameof(TimeSpanString));
        EmployerItems.Clear();

        foreach (Employee employee in roster.Employees)
        {
            RosterEmployeeItemViewModel viewModel = new RosterEmployeeItemViewModel(employee, roster.Days.ToList());
            EmployerItems.Add(viewModel);
        }
    }
    public void Save()
    {
        if (roster.Id is null)
            context.Rosters.Add(roster);
            
        context.SaveChanges();
    }
    public void Reset()
    {

    }
    public void SelectWeek()
    {
        messenger.Send(new ValueChangedMessage<DateTime>(roster.Start.ToDateTime(default)));
    }
    public void Receive(ValueChangedMessage<DateTime> message)
    {
        int dayIndex = 1 - ((int)message.Value.DayOfWeek);
        DateOnly start = DateOnly.FromDateTime(message.Value.AddDays(dayIndex == 1 ? -6 : dayIndex));

        Roster roster = context.Rosters.AsEnumerable().FirstOrDefault(x => x.Start == start);
        if (roster is null)
        {
            DateOnly end = start.AddDays(4);
            InitCreate(start, end, context.Employees.Where(x => !x.IsOut).ToList());
        }
        else
        {
            context.Entry(roster).Collection(b => b.Employees).Load();
            InitUpdate(roster);
        }
    }
}
