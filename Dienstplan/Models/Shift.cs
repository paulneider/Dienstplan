using System;

namespace Dienstplan;

internal class Shift
{
    public int Id { get; set; }
    public Day Day { get; set; }
    public Employee Employee { get; set; }
    public TimeOnly Start { get; set; }
    public TimeOnly End { get; set; }
    public TimeSpan Break { get; set; }

    // SubShifts
}
internal class SubShift
{
    public int Id { get; set; }
    public Shift Shift { get; set; }
    public TimeOnly Start { get; set; }
    public TimeOnly End { get; set; }
    public Group Group { get; set; }
}