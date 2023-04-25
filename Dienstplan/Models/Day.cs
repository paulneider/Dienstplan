﻿using System;
using System.Collections.Generic;

namespace Dienstplan;

internal class Day
{
    public int Id { get; set; }
    public bool IsFree { get; set; }
    public DateOnly Date { get; set; }
    public ICollection<Shift> Shifts { get; set;} = new List<Shift>();
}