using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dienstplan;

class RosterEmployeeItemViewModel : ObservableObject
{
    private Employee employee;

    private Shift mondayShift;
    private Shift tuesdayShift;
    private Shift wednesdayShift;
    private Shift thursdayShift;
    private Shift fridayShift;

    public RosterEmployeeItemViewModel(Employee employee, List<Day> days)
    {
        this.employee = employee;

        if (days[0].Shifts.Any(x => x.Employee == employee))
        {
            mondayShift = days[0].Shifts.First(x => x.Employee == employee);
            tuesdayShift = days[1].Shifts.First(x => x.Employee == employee);
            wednesdayShift = days[2].Shifts.First(x => x.Employee == employee);
            thursdayShift = days[3].Shifts.First(x => x.Employee == employee);
            fridayShift = days[4].Shifts.First(x => x.Employee == employee);
        }
        else
        {
            mondayShift = new Shift();
            mondayShift.Day = days[0];
            mondayShift.Employee = employee;
            days[0].Shifts.Add(mondayShift);
            employee.Shifts.Add(mondayShift);

            tuesdayShift = new Shift();
            tuesdayShift.Day = days[1];
            tuesdayShift.Employee = employee;
            days[1].Shifts.Add(tuesdayShift);
            employee.Shifts.Add(tuesdayShift);

            wednesdayShift = new Shift();
            wednesdayShift.Day = days[2];
            wednesdayShift.Employee = employee;
            days[2].Shifts.Add(wednesdayShift);
            employee.Shifts.Add(wednesdayShift);

            thursdayShift = new Shift();
            thursdayShift.Day = days[3];
            thursdayShift.Employee = employee;
            days[3].Shifts.Add(thursdayShift);
            employee.Shifts.Add(thursdayShift);

            fridayShift = new Shift();
            fridayShift.Day = days[4];
            fridayShift.Employee = employee;
            days[4].Shifts.Add(fridayShift);
            employee.Shifts.Add(fridayShift);
        }
    }

    public string Name => $"{employee.LastName}, {employee.FirstName}";
    public string LastName => employee.LastName;
    public double Hours
    {
        get
        {
            int days = 0;
            if (MondayTime is not null)
                days++;

            if (TuesdayTime is not null)
                days++;

            if (WednesdayTime is not null)
                days++;

            if (ThursdayTime is not null)
                days++;

            if (FridayTime is not null)
                days++;

            return employee.Hours * days / 5;
        }
    }
    public double WrittingHours => employee.WrittingHours;
    public TimeOnly? MondayStart
    {
        get => mondayShift.Start == TimeOnly.MinValue ? null : mondayShift.Start;
        set
        {
            mondayShift.Start = value ?? TimeOnly.MinValue;
            OnPropertyChanged(nameof(MondayStart));
            OnPropertyChanged(nameof(MondayBreak));
            OnPropertyChanged(nameof(MondayTime));
            OnPropertyChanged(nameof(TotalHours));
            OnPropertyChanged(nameof(OverTime));
            OnPropertyChanged(nameof(TotalOverTime));
            OnPropertyChanged(nameof(Hours));
        }
    }
    public TimeOnly? MondayEnd
    {
        get => mondayShift.End == TimeOnly.MinValue ? null : mondayShift.End;
        set
        {
            mondayShift.End = value ?? TimeOnly.MinValue;
            OnPropertyChanged(nameof(MondayEnd));
            OnPropertyChanged(nameof(MondayBreak));
            OnPropertyChanged(nameof(MondayTime));
            OnPropertyChanged(nameof(TotalHours));
            OnPropertyChanged(nameof(OverTime));
            OnPropertyChanged(nameof(TotalOverTime));
            OnPropertyChanged(nameof(Hours));
        }
    }
    public TimeSpan? MondayBreak
    {
        get
        {
            if (!MondayStart.HasValue || !MondayEnd.HasValue)
                return null;

            return (MondayEnd.Value - MondayStart.Value).TotalHours >= 6 ? TimeSpan.FromMinutes(30) : TimeSpan.Zero;
        }
    }
    public double? MondayTime
    {
        get
        {
            TimeSpan? mondayTime = MondayEnd - MondayStart - MondayBreak;
            if (!mondayTime.HasValue)
                return null;

            return mondayTime.Value.TotalMinutes / 60;
        }
    }
    public TimeOnly? TuesdayStart
    {
        get => tuesdayShift.Start == TimeOnly.MinValue ? null : tuesdayShift.Start;
        set
        {
            tuesdayShift.Start = value ?? TimeOnly.MinValue;
            OnPropertyChanged(nameof(TuesdayStart));
            OnPropertyChanged(nameof(TuesdayBreak));
            OnPropertyChanged(nameof(TuesdayTime));
            OnPropertyChanged(nameof(TotalHours));
            OnPropertyChanged(nameof(OverTime));
            OnPropertyChanged(nameof(TotalOverTime));
            OnPropertyChanged(nameof(Hours));
        }
    }
    public TimeOnly? TuesdayEnd
    {
        get => tuesdayShift.End == TimeOnly.MinValue ? null : tuesdayShift.End;
        set
        {
            tuesdayShift.End = value ?? TimeOnly.MinValue;
            OnPropertyChanged(nameof(TuesdayEnd));
            OnPropertyChanged(nameof(TuesdayBreak));
            OnPropertyChanged(nameof(TuesdayTime));
            OnPropertyChanged(nameof(TotalHours));
            OnPropertyChanged(nameof(OverTime));
            OnPropertyChanged(nameof(TotalOverTime));
            OnPropertyChanged(nameof(Hours));
        }
    }
    public TimeSpan? TuesdayBreak
    {
        get
        {
            if (!TuesdayStart.HasValue || !TuesdayEnd.HasValue)
                return null;

            return (TuesdayEnd.Value - TuesdayStart.Value).TotalHours >= 6 ? TimeSpan.FromMinutes(30) : TimeSpan.Zero;
        }
    }
    public double? TuesdayTime
    {
        get
        {
            TimeSpan? tuesdayTime = TuesdayEnd - TuesdayStart - TuesdayBreak;
            if (!tuesdayTime.HasValue)
                return null;

            return tuesdayTime.Value.TotalMinutes / 60;
        }
    }
    public TimeOnly? WednesdayStart
    {
        get => wednesdayShift.Start == TimeOnly.MinValue ? null : wednesdayShift.Start;
        set
        {
            wednesdayShift.Start = value ?? TimeOnly.MinValue;
            OnPropertyChanged(nameof(WednesdayStart));
            OnPropertyChanged(nameof(WednesdayBreak));
            OnPropertyChanged(nameof(WednesdayTime));
            OnPropertyChanged(nameof(TotalHours));
            OnPropertyChanged(nameof(OverTime));
            OnPropertyChanged(nameof(TotalOverTime));
            OnPropertyChanged(nameof(Hours));
        }
    }
    public TimeOnly? WednesdayEnd
    {
        get => wednesdayShift.End == TimeOnly.MinValue ? null : wednesdayShift.End;
        set
        {
            wednesdayShift.End = value ?? TimeOnly.MinValue;
            OnPropertyChanged(nameof(WednesdayEnd));
            OnPropertyChanged(nameof(WednesdayBreak));
            OnPropertyChanged(nameof(WednesdayTime));
            OnPropertyChanged(nameof(TotalHours));
            OnPropertyChanged(nameof(OverTime));
            OnPropertyChanged(nameof(TotalOverTime));
            OnPropertyChanged(nameof(Hours));
        }
    }
    public TimeSpan? WednesdayBreak
    {
        get
        {
            if (!WednesdayStart.HasValue || !WednesdayEnd.HasValue)
                return null;

            return (WednesdayEnd.Value - WednesdayStart.Value).TotalHours >= 6 ? TimeSpan.FromMinutes(30) : TimeSpan.Zero;
        }
    }
    public double? WednesdayTime
    {
        get
        {
            TimeSpan? wednesdayTime = WednesdayEnd - WednesdayStart - WednesdayBreak;
            if (!wednesdayTime.HasValue)
                return null;

            return wednesdayTime.Value.TotalMinutes / 60; 
        }
    }
    public TimeOnly? ThursdayStart
    {
        get => thursdayShift.Start == TimeOnly.MinValue ? null : thursdayShift.Start;
        set
        {
            thursdayShift.Start = value ?? TimeOnly.MinValue;
            OnPropertyChanged(nameof(ThursdayStart));
            OnPropertyChanged(nameof(ThursdayBreak));
            OnPropertyChanged(nameof(ThursdayTime));
            OnPropertyChanged(nameof(TotalHours));
            OnPropertyChanged(nameof(OverTime));
            OnPropertyChanged(nameof(TotalOverTime));
            OnPropertyChanged(nameof(Hours));
        }
    }
    public TimeOnly? ThursdayEnd
    {
        get => thursdayShift.End == TimeOnly.MinValue ? null : thursdayShift.End;
        set
        {
            thursdayShift.End = value ?? TimeOnly.MinValue;
            OnPropertyChanged(nameof(ThursdayEnd));
            OnPropertyChanged(nameof(ThursdayBreak));
            OnPropertyChanged(nameof(ThursdayTime));
            OnPropertyChanged(nameof(TotalHours));
            OnPropertyChanged(nameof(OverTime));
            OnPropertyChanged(nameof(TotalOverTime));
            OnPropertyChanged(nameof(Hours));
        }
    }
    public TimeSpan? ThursdayBreak
    {
        get
        {
            if (!ThursdayStart.HasValue || !ThursdayEnd.HasValue)
                return null;

            return (ThursdayEnd.Value - ThursdayStart.Value).TotalHours >= 6 ? TimeSpan.FromMinutes(30) : TimeSpan.Zero;
        }
    }
    public double? ThursdayTime
    {
        get
        {
            TimeSpan? thursdayTime = ThursdayEnd - ThursdayStart - ThursdayBreak;
            if (!thursdayTime.HasValue)
                return null;

            return thursdayTime.Value.TotalMinutes / 60;
        }
    }
    public TimeOnly? FridayStart
    {
        get => fridayShift.Start == TimeOnly.MinValue ? null : fridayShift.Start;
        set
        {
            fridayShift.Start = value ?? TimeOnly.MinValue;
            OnPropertyChanged(nameof(FridayStart));
            OnPropertyChanged(nameof(FridayBreak));
            OnPropertyChanged(nameof(FridayTime));
            OnPropertyChanged(nameof(TotalHours));
            OnPropertyChanged(nameof(OverTime));
            OnPropertyChanged(nameof(TotalOverTime));
            OnPropertyChanged(nameof(Hours));
        }
    }
    public TimeOnly? FridayEnd
    {
        get => fridayShift.End == TimeOnly.MinValue ? null : fridayShift.End;
        set
        {
            fridayShift.End = value ?? TimeOnly.MinValue;
            OnPropertyChanged(nameof(FridayEnd));
            OnPropertyChanged(nameof(FridayBreak));
            OnPropertyChanged(nameof(FridayTime));
            OnPropertyChanged(nameof(TotalHours));
            OnPropertyChanged(nameof(OverTime));
            OnPropertyChanged(nameof(TotalOverTime));
            OnPropertyChanged(nameof(Hours));
        }
    }
    public TimeSpan? FridayBreak
    {
        get
        {
            if (!FridayStart.HasValue || !FridayEnd.HasValue)
                return null;

            return (FridayEnd.Value - FridayStart.Value).TotalHours >= 6 ? TimeSpan.FromMinutes(30) : TimeSpan.Zero;
        }
    }
    public double? FridayTime
    {
        get
        {
            TimeSpan? fridayTime = FridayEnd - FridayStart - FridayBreak;
            if (!fridayTime.HasValue)
                return null;

            return fridayTime.Value.TotalMinutes / 60;
        }
    }
    public double TotalHours
    {
        get
        {
            return (MondayTime ?? default) + (TuesdayTime ?? default) + (WednesdayTime ?? default) + (ThursdayTime ?? default) + (FridayTime ?? default);
        }
    }
    public double OverTime
    {
        get
        {
            return TotalHours - Hours + WrittingHours;
        }
    }
    public double TotalOverTime
    {
        get
        {
            return 0;
        }
    }
}
