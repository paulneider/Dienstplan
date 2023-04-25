using System;
using System.Collections.Generic;

namespace Dienstplan;

internal class Roster
{
    public int Id { get; set; }
    public ICollection<Day> Days { get; set; } = new List<Day>();

    public DateOnly Start { get; set; }
    public DateOnly End { get; set; }


    // Pro Tag eine Tabelle

    // Woche --> Zeitraum
    // einzelne Tag
    // Wunschliste
}
