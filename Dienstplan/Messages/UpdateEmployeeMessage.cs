namespace Dienstplan;

internal class UpdateEmployeeMessage 
{ 
    public EmployeeItemViewModel EmployeeItem { get; set; }
    public UpdateEmployeeMessage(EmployeeItemViewModel employeeItem)
    {
        EmployeeItem = employeeItem;
    }
}
