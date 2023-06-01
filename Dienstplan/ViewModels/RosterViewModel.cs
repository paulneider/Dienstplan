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
        get => IsMondayFree ? null : mondayStart;
        set => SetProperty(ref mondayStart, value);
    }
    private TimeOnly? mondayEnd;
    public TimeOnly? MondayEnd
    {
        get => IsMondayFree ? null : mondayEnd;
        set => SetProperty(ref mondayEnd, value);
    }
    public bool IsMondayFree
    {
        get => roster.Days.ElementAt(0).IsFree;
        set
        {
            roster.Days.ElementAt(0).IsFree = value;

            MondayStart = value ? null : defaultStart;
            MondayEnd = value ? null : defaultEnd;

            OnPropertyChanged(nameof(MondayStart));
            OnPropertyChanged(nameof(MondayEnd));

            foreach (var employee in EmployerItems)
            {
                employee.MondayStart = value ? null : new TimeOnly(7, 30);
                employee.MondayEnd = value ? null : new TimeOnly(16, 0);
            }
        }
    }
    private TimeOnly? tuesdayStart;
    public TimeOnly? TuesdayStart
    {
        get => IsTuesdayFree ? null : tuesdayStart;
        set => SetProperty(ref tuesdayStart, value);
    }
    private TimeOnly? tuesdayEnd;
    public TimeOnly? TuesdayEnd
    {
        get => IsTuesdayFree ? null : tuesdayEnd;
        set => SetProperty(ref tuesdayEnd, value);
    }
    public bool IsTuesdayFree
    {
        get => roster.Days.ElementAt(1).IsFree;
        set
        {
            roster.Days.ElementAt(1).IsFree = value;

            TuesdayStart = value ? null : defaultStart;
            TuesdayEnd = value ? null : defaultEnd;

            OnPropertyChanged(nameof(TuesdayStart));
            OnPropertyChanged(nameof(TuesdayEnd));

            foreach (var employee in EmployerItems)
            {
                employee.TuesdayStart = value ? null : new TimeOnly(7, 30);
                employee.TuesdayEnd = value ? null : new TimeOnly(16, 0);
            }
        }
    }
    private TimeOnly? wednesdayStart;
    public TimeOnly? WednesdayStart
    {
        get => IsWednesdayFree ? null : wednesdayStart;
        set => SetProperty(ref wednesdayStart, value);
    }
    private TimeOnly? wednesdayEnd;
    public TimeOnly? WednesdayEnd
    {
        get => IsWednesdayFree ? null : wednesdayEnd;
        set => SetProperty(ref wednesdayEnd, value);
    }
    public bool IsWednesdayFree
    {
        get => roster.Days.ElementAt(2).IsFree;
        set
        {
            roster.Days.ElementAt(2).IsFree = value;

            WednesdayStart = value ? null : defaultStart;
            WednesdayEnd = value ? null : defaultEnd;

            OnPropertyChanged(nameof(WednesdayStart));
            OnPropertyChanged(nameof(WednesdayEnd));

            foreach (var employee in EmployerItems)
            {
                employee.WednesdayStart = value ? null : new TimeOnly(7, 30);
                employee.WednesdayEnd = value ? null : new TimeOnly(16, 0);
            }
        }
    }
    private TimeOnly? thursdayStart;
    public TimeOnly? ThursdayStart
    {
        get => IsThursdayFree ? null : thursdayStart;
        set => SetProperty(ref thursdayStart, value);
    }
    private TimeOnly? thursdayEnd;
    public TimeOnly? ThursdayEnd
    {
        get => IsThursdayFree ? null : thursdayEnd;
        set => SetProperty(ref thursdayEnd, value);
    }
    public bool IsThursdayFree
    {
        get => roster.Days.ElementAt(3).IsFree;
        set
        {
            roster.Days.ElementAt(3).IsFree = value;

            ThursdayStart = value ? null : defaultStart;
            ThursdayEnd = value ? null : defaultEnd;

            OnPropertyChanged(nameof(ThursdayStart));
            OnPropertyChanged(nameof(ThursdayEnd));

            foreach (var employee in EmployerItems)
            {
                employee.ThursdayStart = value ? null : new TimeOnly(7, 30);
                employee.ThursdayEnd = value ? null : new TimeOnly(16, 0);
            }
        }
    }
    private TimeOnly? fridayStart;
    public TimeOnly? FridayStart
    {
        get => IsFridayFree ? null : fridayStart;
        set => SetProperty(ref fridayStart, value);
    }
    private TimeOnly? fridayEnd;
    public TimeOnly? FridayEnd
    {
        get => IsFridayFree ? null : fridayEnd;
        set => SetProperty(ref fridayEnd, value);
    }
    public bool IsFridayFree
    {
        get => roster.Days.ElementAt(4).IsFree;
        set
        {
            roster.Days.ElementAt(4).IsFree = value;

            FridayStart = value ? null : defaultStart;
            FridayEnd = value ? null : defaultEnd;

            OnPropertyChanged(nameof(FridayStart));
            OnPropertyChanged(nameof(FridayEnd));

            foreach (var employee in EmployerItems)
            {
                employee.FridayStart = value ? null : new TimeOnly(7, 30);
                employee.FridayEnd = value ? null : new TimeOnly(16, 0);
            }
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

        OnPropertyChanged(nameof(MondayLabel));
        OnPropertyChanged(nameof(TuesdayLabel));
        OnPropertyChanged(nameof(WednesdayLabel));
        OnPropertyChanged(nameof(ThursdayLabel));
        OnPropertyChanged(nameof(FridayLabel));
    }
}
