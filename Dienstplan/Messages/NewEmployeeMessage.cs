namespace Dienstplan;

internal class NewEmployeeMessage
{
    public Employee Employee { get; set; }
    public NewEmployeeMessage(Employee employee)
    {
        Employee = employee;
    }
}
