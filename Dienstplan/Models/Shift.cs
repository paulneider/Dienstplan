using System;

namespace Dienstplan;

internal class Shift
{
    public int? Id { get; set; }
    public Day Day { get; set; }
    public Employee Employee { get; set; }
    public TimeOnly Start { get; set; } = new TimeOnly(7, 30);
    public TimeOnly End { get; set; } = new TimeOnly(16, 0);
    public TimeSpan Break { get; set; } = new TimeSpan(0, 30, 0);

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