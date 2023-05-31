using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using CommunityToolkit.Mvvm.Messaging.Messages;
using Dienstplan.Properties;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Linq;
using System.Windows.Input;

namespace Dienstplan;

internal class RosterViewModel : ObservableObject, IRecipient<ValueChangedMessage<DateTime>>
{
    private readonly ApplicationDbContext context;
    private readonly IMessenger messenger;
    private readonly TimeOnly defaultStart;
    private readonly TimeOnly defaultEnd;
    private Roster roster;
    private ObservableCollection<RosterEmployeeItemViewModel> employerItems = new ObservableCollection<RosterEmployeeItemViewModel>();
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

            return string.Format(Resources.WeekOf, roster.Start.ToString("dd.MM."), roster.End.ToString("dd.MM.yyyy"));
        }
    }

    public string MondayLabel
    {
        get
        {
            string label = Resources.Monday;
            if (roster is not null)
                label += " - " + roster.Days.ElementAt(0).Date.ToString("dd.MM.");

            return label;
        }
    }
    public string TuesdayLabel
    {
        get
        {
            string label = Resources.Tuesday;
            if (roster is not null)
                label += " - " + roster.Days.ElementAt(1).Date.ToString("dd.MM.");

            return label;
        }
    }
    public string WednesdayLabel
    {
        get
        {
            string label = Resources.Wednesday;
            if (roster is not null)
                label += " - " + roster.Days.ElementAt(2).Date.ToString("dd.MM.");

            return label;
        }
    }
    public string ThursdayLabel
    {
        get
        {
            string label = Resources.Thursday;
            if (roster is not null)
                label += " - " + roster.Days.ElementAt(3).Date.ToString("dd.MM.");

            return label;
        }
    }
    public string FridayLabel
    {
        get
        {
            string label = Resources.Friday;
            if (roster is not null)
                label += " - " + roster.Days.ElementAt(4).Date.ToString("dd.MM.");

            return label;
        }
    }
    private TimeOnly? mondayStart;
    public TimeOnly? MondayStart
    {
        get => mondayStart;
        set => SetProperty(ref mondayStart, value);
    }
    private TimeOnly? mondayEnd;
    public TimeOnly? MondayEnd
    {
        get => mondayEnd;
        set => SetProperty(ref mondayEnd, value);
    }
    private bool isMondayFree;
    public bool IsMondayFree
    {
        get => isMondayFree;
        set
        {
            MondayStart = value ? null : defaultStart;
            MondayEnd = value ? null : defaultEnd;

            roster.Days.ElementAt(0).IsFree = value;
            SetProperty(ref isMondayFree, value);
        }
    }
    private TimeOnly tuesdayStart;
    public TimeOnly TuesdayStart
    {
        get => tuesdayStart;
        set => SetProperty(ref tuesdayStart, value);
    }
    private TimeOnly tuesdayEnd;
    public TimeOnly TuesdayEnd
    {
        get => tuesdayEnd;
        set => SetProperty(ref tuesdayEnd, value);
    }
    private bool isTuesdayFree;
    public bool IsTuesdayFree
    {
        get => isTuesdayFree;
        set => SetProperty(ref isTuesdayFree, value);
    }
    private TimeOnly wednesdayStart;
    public TimeOnly WednesdayStart
    {
        get => wednesdayStart;
        set => SetProperty(ref wednesdayStart, value);
    }
    private TimeOnly wednesdayEnd;
    public TimeOnly WednesdayEnd
    {
        get => wednesdayEnd;
        set => SetProperty(ref wednesdayEnd, value);
    }
    private bool isWednesdayFree;
    public bool IsWednesdayFree
    {
        get => isWednesdayFree;
        set => SetProperty(ref isWednesdayFree, value);
    }
    private TimeOnly thursdayStart;
    public TimeOnly ThursdayStart
    {
        get => thursdayStart;
        set => SetProperty(ref thursdayStart, value);
    }
    private TimeOnly thursdayEnd;
    public TimeOnly ThursdayEnd
    {
        get => thursdayEnd;
        set => SetProperty(ref thursdayEnd, value);
    }
    private bool isThursdayFree;
    public bool IsThursdayFree
    {
        get => isThursdayFree;
        set => SetProperty(ref isThursdayFree, value);
    }
    private TimeOnly fridayStart;
    public TimeOnly FridayStart
    {
        get => fridayStart;
        set => SetProperty(ref fridayStart, value);
    }
    private TimeOnly fridayEnd;
    public TimeOnly FridayEnd
    {
        get => fridayEnd;
        set => SetProperty(ref fridayEnd, value);
    }
    private bool isFridayFree;
    public bool IsFridayFree
    {
        get => isFridayFree;
        set => SetProperty(ref isFridayFree, value);
    }

    public ICommand SaveCommand => new RelayCommand(Save);
    public ICommand ResetCommand => new RelayCommand(Reset);
    public ICommand SelectWeekCommand => new RelayCommand(SelectWeek);
    public RosterViewModel() { }
    public RosterViewModel(ApplicationDbContext context, IMessenger messenger)
    {
        this.context = context;
        this.messenger = messenger;

        string strStart = ConfigurationManager.AppSettings.Get("DefaultStartTime") ?? "06:00";
        defaultStart = TimeOnly.TryParse(strStart, out defaultStart) ? defaultStart : new TimeOnly(6,0);
        mondayStart = defaultStart;
        tuesdayStart = defaultStart;
        wednesdayStart = defaultStart;
        thursdayStart = defaultStart;
        fridayStart = defaultStart;

        string strEnd = ConfigurationManager.AppSettings.Get("DefaultEndTime") ?? "18:00";
        defaultEnd = TimeOnly.TryParse(strEnd, out defaultEnd) ? defaultEnd : new TimeOnly(18,0);
        mondayEnd = defaultEnd;
        tuesdayEnd = defaultEnd;
        wednesdayEnd = defaultEnd;
        thursdayEnd = defaultEnd;
        fridayEnd = defaultEnd;

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
