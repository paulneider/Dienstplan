using CommunityToolkit.Mvvm.ComponentModel;

namespace Dienstplan;

internal class EmployeeItemViewModel : ObservableObject
{
    private readonly Employee employee;

    public string FirstName
    {
        get => employee.FirstName;
        set
        {
            employee.FirstName = value;
            OnPropertyChanged();
        }
    }
    public string LastName
    {
        get => employee.LastName;
        set
        {
            employee.LastName = value;
            OnPropertyChanged();
        }
    }
    public double Hours
    {
        get => employee.Hours;
        set
        {
            employee.Hours = value;
            OnPropertyChanged();
        }
    }
    public double WrittingHours
    {
        get => employee.WrittingHours;
        set
        {
            employee.WrittingHours = value;
            OnPropertyChanged();
        }
    }
    public Group Group
    {
        get => employee.Group;
        set
        {
            employee.Group = value;
            OnPropertyChanged();
        }
    }
    public EmployeeItemViewModel(Employee employee)
    {
        this.employee = employee;
    }
}
