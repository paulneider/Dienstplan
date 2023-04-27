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

            tuesdayShift = new Shift();
            tuesdayShift.Day = days[1];

            wednesdayShift = new Shift();
            wednesdayShift.Day = days[2];

            thursdayShift = new Shift();
            thursdayShift.Day = days[3];

            fridayShift = new Shift();
            fridayShift.Day = days[4];
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
            OnPropertChanged(nameof(MondayStart));
            OnPropertChanged(nameof(MondayTime));
            OnPropertChanged(nameof(TotalHours));
            OnPropertChanged(nameof(OverTime));
            OnPropertChanged(nameof(TotalOverTime));
        }
    }
    public TimeOnly MondayEnd
    {
        get => mondayShift.End;
        set
        {
            mondayShift.End = value;
            OnPropertChanged(nameof(MondayEnd));
            OnPropertChanged(nameof(MondayTime));
            OnPropertChanged(nameof(TotalHours));
            OnPropertChanged(nameof(OverTime));
            OnPropertChanged(nameof(TotalOverTime));
        }
    }
    public TimeSpan MondayBreak
    {
        get => mondayShift.Break;
        set
        {
            mondayShift.Break = value;
            OnPropertChanged(nameof(MondayBreak));
            OnPropertChanged(nameof(MondayTime));
            OnPropertChanged(nameof(TotalHours));
            OnPropertChanged(nameof(OverTime));
            OnPropertChanged(nameof(TotalOverTime));
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
            OnPropertChanged(nameof(TuesdayStart));
            OnPropertChanged(nameof(TuesdayTime));
            OnPropertChanged(nameof(TotalHours));
            OnPropertChanged(nameof(OverTime));
            OnPropertChanged(nameof(TotalOverTime));
        }
    }
    public TimeOnly TuesdayEnd
    {
        get => tuesdayShift.End;
        set
        {
            tuesdayShift.End = value;
            OnPropertChanged(nameof(TuesdayEnd));
            OnPropertChanged(nameof(TuesdayTime));
            OnPropertChanged(nameof(TotalHours));
            OnPropertChanged(nameof(OverTime));
            OnPropertChanged(nameof(TotalOverTime));
        }
    }
    public TimeSpan TuesdayBreak
    {
        get => tuesdayShift.Break;
        set
        {
            tuesdayShift.Break = value;
            OnPropertChanged(nameof(TuesdayBreak));
            OnPropertChanged(nameof(TuesdayTime));
            OnPropertChanged(nameof(TotalHours));
            OnPropertChanged(nameof(OverTime));
            OnPropertChanged(nameof(TotalOverTime));
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
            OnPropertChanged(nameof(WednesdayStart));
            OnPropertChanged(nameof(WednesdayTime));
            OnPropertChanged(nameof(TotalHours));
            OnPropertChanged(nameof(OverTime));
            OnPropertChanged(nameof(TotalOverTime));
        }
    }
    public TimeOnly WednesdayEnd
    {
        get => wednesdayShift.End;
        set
        {
            wednesdayShift.End = value;
            OnPropertChanged(nameof(WednesdayEnd));
            OnPropertChanged(nameof(WednesdayTime));
            OnPropertChanged(nameof(TotalHours));
            OnPropertChanged(nameof(OverTime));
            OnPropertChanged(nameof(TotalOverTime));
        }
    }
    public TimeSpan WednesdayBreak
    {
        get => wednesdayShift.Break;
        set
        {
            wednesdayShift.Break = value;
            OnPropertChanged(nameof(WednesdayBreak));
            OnPropertChanged(nameof(WednesdayTime));
            OnPropertChanged(nameof(TotalHours));
            OnPropertChanged(nameof(OverTime));
            OnPropertChanged(nameof(TotalOverTime));
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
            OnPropertChanged(nameof(ThursdayStart));
            OnPropertChanged(nameof(ThursdayTime));
            OnPropertChanged(nameof(TotalHours));
            OnPropertChanged(nameof(OverTime));
            OnPropertChanged(nameof(TotalOverTime));
        }
    }
    public TimeOnly ThursdayEnd
    {
        get => thursdayShift.End;
        set
        {
            thursdayShift.End = value;
            OnPropertChanged(nameof(ThursdayEnd));
            OnPropertChanged(nameof(ThursdayTime));
            OnPropertChanged(nameof(TotalHours));
            OnPropertChanged(nameof(OverTime));
            OnPropertChanged(nameof(TotalOverTime));
        }
    }
    public TimeSpan ThursdayBreak
    {
        get => thursdayShift.Break;
        set
        {
            thursdayShift.Break = value;
            OnPropertChanged(nameof(ThursdayBreak));
            OnPropertChanged(nameof(ThursdayTime));
            OnPropertChanged(nameof(TotalHours));
            OnPropertChanged(nameof(OverTime));
            OnPropertChanged(nameof(TotalOverTime));
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
            OnPropertChanged(nameof(FridayStart));
            OnPropertChanged(nameof(FridayTime));
            OnPropertChanged(nameof(TotalHours));
            OnPropertChanged(nameof(OverTime));
            OnPropertChanged(nameof(TotalOverTime));
        }
    }
    public TimeOnly FridayEnd
    {
        get => fridayShift.End;
        set
        {
            fridayShift.End = value;
            OnPropertChanged(nameof(FridayEnd));
            OnPropertChanged(nameof(FridayTime));
            OnPropertChanged(nameof(TotalHours));
            OnPropertChanged(nameof(OverTime));
            OnPropertChanged(nameof(TotalOverTime));
        }
    }
    public TimeSpan FridayBreak
    {
        get => fridayShift.Break;
        set
        {
            fridayShift.Break = value;
            OnPropertChanged(nameof(FridayBreak));
            OnPropertChanged(nameof(FridayTime));
            OnPropertChanged(nameof(TotalHours));
            OnPropertChanged(nameof(OverTime));
            OnPropertChanged(nameof(TotalOverTime));
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
