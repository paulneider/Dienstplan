using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dienstplan;

internal class Employee
{
    public long Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public double Hours { get; set; }
    public double WrittingHours { get; set; }
    public bool IsOut { get; set; }
    public Group Group { get; set; }
    public ICollection<Shift> Shifts { get; set; } = new List<Shift>();
}
