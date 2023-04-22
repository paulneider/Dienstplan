using System.Collections.Generic;
using System;
using System.Windows;
using System.Collections.ObjectModel;
using System.Windows.Input;
using System.Linq;

namespace Dienstplan;
internal class EmployeesViewModel : VMBase
{
    public event EventHandler<IList<Employee>> SaveAndClose;
    public Visibility Visibility
    {
        get => GetValue(Visibility.Collapsed);
        set => SetValue(value);
    }
    public Visibility EditGridVisibility
    {
        get => GetValue(Visibility.Collapsed);
        set => SetValue(value);
    }
    public bool GridIsEnabled
    {
        get => GetValue(true);
        set => SetValue(value);
    }
    public ObservableCollection<Group> Groups
    {
        get => GetValue<ObservableCollection<Group>>();
        set => SetValue(value);
    }
    public ObservableCollection<Employee> Employees
    {
        get => GetValue<ObservableCollection<Employee>>();
        set => SetValue(value);
    }
    public Employee SelectedItem
    {
        get => GetValue<Employee>();
        set => SetValue(value);
    }
    public string NewFirstName
    {
        get => GetValue<string>();
        set => SetValue(value);
    }
    public string NewLastName
    {
        get => GetValue<string>();
        set => SetValue(value);
    }
    public double NewHours
    {
        get => GetValue<double>();
        set => SetValue(value);
    }
    public double NewWrittingHours
    {
        get => GetValue<double>();
        set => SetValue(value);
    }
    public Group SelectedGroup
    {
        get => GetValue<Group>();
        set => SetValue(value);
    }

    public ICommand AddEmployeeCommand => new Command(AddEmployee);
    public ICommand DeleteEmployeeCommand => new Command(DeleteEmployee);
    public ICommand UpdateEmployeeCommand => new Command(UpdateEmployee);
    public ICommand SaveCommand => new Command(Save);
    public ICommand OkayCommand => new Command(Okay);
    public ICommand CancleCommand => new Command(Cancle);

    private bool isAdd = true;
    private readonly List<Employee> newEmployees = new List<Employee>();

    private void AddEmployee(object param)
    {
        NewFirstName = "";
        NewLastName = "";
        NewHours = 0;
        NewWrittingHours = 0;
        SelectedGroup = Groups.FirstOrDefault();

        EditGridVisibility = Visibility.Visible;
        GridIsEnabled = false;
        isAdd = true;
    }
    private void DeleteEmployee(object param)
    {
        if (SelectedItem is null)
            return;

        SelectedItem.IsOut = true;
        Employees.Remove(SelectedItem);
    }
    private void UpdateEmployee(object param)
    {
        if (SelectedItem is null)
            return;

        NewFirstName = SelectedItem.FirstName;
        NewLastName = SelectedItem.LastName;
        NewHours = SelectedItem.Hours;
        NewWrittingHours = SelectedItem.WrittingHours;
        SelectedGroup = SelectedItem.Group;

        EditGridVisibility = Visibility.Visible;
        GridIsEnabled = false;
        isAdd = false;
    }
    private void Save(object param)
    {
        SaveAndClose?.Invoke(this, newEmployees);
        newEmployees.Clear();
    }
    private void Okay(object param)
    {
        if (string.IsNullOrWhiteSpace(NewFirstName))
            return;

        if (string.IsNullOrWhiteSpace(NewLastName)) 
            return;

        if (isAdd)
        {
            Employee newEmployee = new Employee();
            newEmployee.FirstName = NewFirstName;
            newEmployee.LastName = NewLastName;
            newEmployee.Hours = NewHours;
            newEmployee.WrittingHours = NewWrittingHours;
            newEmployee.Group = SelectedGroup;
            Employees.Add(newEmployee);
            newEmployees.Add(newEmployee);
        }
        else
        {
            SelectedItem.FirstName = NewFirstName;
            SelectedItem.LastName = NewLastName;
            SelectedItem.Hours = NewHours;
            SelectedItem.WrittingHours = NewWrittingHours;
            SelectedItem.Group = SelectedGroup;

            Employees = new ObservableCollection<Employee>(Employees);
        }

        GridIsEnabled = true;
        EditGridVisibility = Visibility.Collapsed;
    }
    private void Cancle(object param)
    {
        GridIsEnabled = true;
        EditGridVisibility = Visibility.Collapsed;
    }
}
