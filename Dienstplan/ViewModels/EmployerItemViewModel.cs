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
    public TimeOnly MondayStart
    {
        get => mondayShift.Start;
        set
        {
            mondayShift.Start = value;
            OnPropertChanged(nameof(MondayStart));
            OnPropertChanged(nameof(MondayTime));
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
        }
    }
    public TimeSpan MondayTime
    {
        get
        {
            return MondayEnd - MondayStart - MondayBreak;
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
        }
    }
    public TimeSpan TuesdayTime
    {
        get
        {
            return TuesdayEnd - TuesdayStart - TuesdayBreak;
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
        }
    }
    public TimeSpan WednesdayTime
    {
        get
        {
            return WednesdayEnd - WednesdayStart - WednesdayBreak;
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
        }
    }
    public TimeSpan ThursdayTime
    {
        get
        {
            return ThursdayEnd - ThursdayStart - ThursdayBreak;
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
        }
    }
    public TimeSpan FridayTime
    {
        get
        {
            return FridayEnd - FridayStart - FridayBreak;
        }
    }
}
