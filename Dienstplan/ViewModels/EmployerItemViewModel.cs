using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dienstplan;

class EmployerItemViewModel : VMBase
{
    private Employee employee;

    private Shift mondayShift;
    private Shift tuesdayShift;
    private Shift wednesdayShift;
    private Shift thursdayShift;
    private Shift fridayShift;

    public EmployerItemViewModel(Employee employee, List<Day> days)
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
    public double Hours => employee.Hours;
    public double WrittingHours => employee.WrittingHours;
    public TimeOnly MondayStart
    {
        get => mondayShift.Start;
        set
        {
            mondayShift.Start = value;
            OnPropertyChanged(nameof(MondayStart));
            OnPropertyChanged(nameof(MondayBreak));
            OnPropertyChanged(nameof(MondayTime));
            OnPropertyChanged(nameof(TotalHours));
            OnPropertyChanged(nameof(OverTime));
            OnPropertyChanged(nameof(TotalOverTime));
        }
    }
    public TimeOnly MondayEnd
    {
        get => mondayShift.End;
        set
        {
            mondayShift.End = value;
            OnPropertyChanged(nameof(MondayEnd));
            OnPropertyChanged(nameof(MondayBreak));
            OnPropertyChanged(nameof(MondayTime));
            OnPropertyChanged(nameof(TotalHours));
            OnPropertyChanged(nameof(OverTime));
            OnPropertyChanged(nameof(TotalOverTime));
        }
    }
    public TimeSpan MondayBreak
    {
        get
        {
            return (MondayEnd - MondayStart).TotalHours >= 6 ? TimeSpan.FromMinutes(30) : TimeSpan.Zero;
        }
    }
    public double MondayTime
    {
        get
        {
            return (MondayEnd - MondayStart - MondayBreak).TotalMinutes / 60;
        }
    }
    public TimeOnly TuesdayStart
    {
        get => tuesdayShift.Start;
        set
        {
            tuesdayShift.Start = value;
            OnPropertyChanged(nameof(TuesdayStart));
            OnPropertyChanged(nameof(TuesdayBreak));
            OnPropertyChanged(nameof(TuesdayTime));
            OnPropertyChanged(nameof(TotalHours));
            OnPropertyChanged(nameof(OverTime));
            OnPropertyChanged(nameof(TotalOverTime));
        }
    }
    public TimeOnly TuesdayEnd
    {
        get => tuesdayShift.End;
        set
        {
            tuesdayShift.End = value;
            OnPropertyChanged(nameof(TuesdayEnd));
            OnPropertyChanged(nameof(TuesdayBreak));
            OnPropertyChanged(nameof(TuesdayTime));
            OnPropertyChanged(nameof(TotalHours));
            OnPropertyChanged(nameof(OverTime));
            OnPropertyChanged(nameof(TotalOverTime));
        }
    }
    public TimeSpan TuesdayBreak
    {
        get
        {
            return (TuesdayEnd - TuesdayStart).TotalHours >= 6 ? TimeSpan.FromMinutes(30) : TimeSpan.Zero;
        }
    }
    public double TuesdayTime
    {
        get
        {
            return (TuesdayEnd - TuesdayStart - TuesdayBreak).TotalMinutes / 60;
        }
    }
    public TimeOnly WednesdayStart
    {
        get => wednesdayShift.Start;
        set
        {
            wednesdayShift.Start = value;
            OnPropertyChanged(nameof(WednesdayStart));
            OnPropertyChanged(nameof(WednesdayBreak));
            OnPropertyChanged(nameof(WednesdayTime));
            OnPropertyChanged(nameof(TotalHours));
            OnPropertyChanged(nameof(OverTime));
            OnPropertyChanged(nameof(TotalOverTime));
        }
    }
    public TimeOnly WednesdayEnd
    {
        get => wednesdayShift.End;
        set
        {
            wednesdayShift.End = value;
            OnPropertyChanged(nameof(WednesdayEnd));
            OnPropertyChanged(nameof(WednesdayBreak));
            OnPropertyChanged(nameof(WednesdayTime));
            OnPropertyChanged(nameof(TotalHours));
            OnPropertyChanged(nameof(OverTime));
            OnPropertyChanged(nameof(TotalOverTime));
        }
    }
    public TimeSpan WednesdayBreak
    {
        get
        {
            return (WednesdayEnd - WednesdayStart).TotalHours >= 6 ? TimeSpan.FromMinutes(30) : TimeSpan.Zero;
        }
    }
    public double WednesdayTime
    {
        get
        {
            return (WednesdayEnd - WednesdayStart - WednesdayBreak).TotalMinutes / 60; 
        }
    }
    public TimeOnly ThursdayStart
    {
        get => thursdayShift.Start;
        set
        {
            thursdayShift.Start = value;
            OnPropertyChanged(nameof(ThursdayStart));
            OnPropertyChanged(nameof(ThursdayBreak));
            OnPropertyChanged(nameof(ThursdayTime));
            OnPropertyChanged(nameof(TotalHours));
            OnPropertyChanged(nameof(OverTime));
            OnPropertyChanged(nameof(TotalOverTime));
        }
    }
    public TimeOnly ThursdayEnd
    {
        get => thursdayShift.End;
        set
        {
            thursdayShift.End = value;
            OnPropertyChanged(nameof(ThursdayEnd));
            OnPropertyChanged(nameof(ThursdayBreak));
            OnPropertyChanged(nameof(ThursdayTime));
            OnPropertyChanged(nameof(TotalHours));
            OnPropertyChanged(nameof(OverTime));
            OnPropertyChanged(nameof(TotalOverTime));
        }
    }
    public TimeSpan ThursdayBreak
    {
        get
        {
            return (ThursdayEnd - ThursdayStart).TotalHours >= 6 ? TimeSpan.FromMinutes(30) : TimeSpan.Zero;
        }
    }
    public double ThursdayTime
    {
        get
        {
            return (ThursdayEnd - ThursdayStart - ThursdayBreak).TotalMinutes / 60;
        }
    }
    public TimeOnly FridayStart
    {
        get => fridayShift.Start;
        set
        {
            fridayShift.Start = value;
            OnPropertyChanged(nameof(FridayStart));
            OnPropertyChanged(nameof(FridayBreak));
            OnPropertyChanged(nameof(FridayTime));
            OnPropertyChanged(nameof(TotalHours));
            OnPropertyChanged(nameof(OverTime));
            OnPropertyChanged(nameof(TotalOverTime));
        }
    }
    public TimeOnly FridayEnd
    {
        get => fridayShift.End;
        set
        {
            fridayShift.End = value;
            OnPropertyChanged(nameof(FridayEnd));
            OnPropertyChanged(nameof(FridayBreak));
            OnPropertyChanged(nameof(FridayTime));
            OnPropertyChanged(nameof(TotalHours));
            OnPropertyChanged(nameof(OverTime));
            OnPropertyChanged(nameof(TotalOverTime));
        }
    }
    public TimeSpan FridayBreak
    {
        get
        {
            return (FridayEnd - FridayStart).TotalHours >= 6 ? TimeSpan.FromMinutes(30) : TimeSpan.Zero;
        }
    }
    public double FridayTime
    {
        get
        {
            return (FridayEnd - FridayStart - FridayBreak).TotalMinutes / 60;
        }
    }
    public double TotalHours
    {
        get
        {
            return MondayTime + TuesdayTime + WednesdayTime + ThursdayTime + FridayTime;
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
