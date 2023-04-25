using System;

namespace Dienstplan;

internal class ShiftViewModel : VMBase
{
    private readonly Shift shift;
    public ShiftViewModel(Shift shift)
    {
        this.shift = shift;
    }
    public TimeOnly Start 
    {
        get => shift.Start;
        set
        {
            shift.Start = value;
            OnPropertChanged(nameof(Start));
        }
    }
    public TimeOnly End 
    {
        get => shift.End;
        set
        {
            shift.End = value;
            OnPropertChanged(nameof(End));
        }
    }
    public TimeSpan Break 
    {
        get => shift.Break;
        set
        {
            shift.Break = value;
            OnPropertChanged(nameof(Break));
        }
    }

    // DependsUpOn
    public TimeSpan Time 
    { 
        get
        {
            return End - Start - Break;
        } 
    }
}
