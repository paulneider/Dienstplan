using System;
using System.Collections.Generic;

namespace Dienstplan;

internal class Day
{
    public int? Id { get; set; }
    public bool IsFree { get; set; }
    public DateOnly Date { get; set; }
    public Roster Roster { get; set; }
    public ICollection<Shift> Shifts { get; set;} = new List<Shift>();
    //public TimeOnly Start { get; set; } = new TimeOnly(7, 30);
    //public TimeOnly End { get; set; } = new TimeOnly(16, 0);
}
