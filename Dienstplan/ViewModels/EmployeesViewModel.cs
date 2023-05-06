using System.Collections.Generic;
using System;
using System.Windows;
using System.Collections.ObjectModel;
using System.Windows.Input;
using System.Linq;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.ComponentModel;

namespace Dienstplan;

internal partial class EmployeesViewModel : ObservableObject
{
    public event EventHandler<IList<Employee>> SaveEmployees;

    private Visibility editGridVisibility = Visibility.Collapsed;
    public Visibility EditGridVisibility
    {
        get => editGridVisibility;
        set => SetProperty(ref editGridVisibility, value);
    }
    private bool gridIsEnabled = true;
    public bool GridIsEnabled
    {
        get => gridIsEnabled;
        set => SetProperty(ref gridIsEnabled, value);
    }
    private ObservableCollection<Group> groups;
    public ObservableCollection<Group> Groups
    {
        get => groups;
        set => SetProperty(ref groups, value);
    }
    private ObservableCollection<Employee> employees;
    public ObservableCollection<Employee> Employees
    {
        get => employees;
        set => SetProperty(ref employees, value);
    }
    private Employee selectedItem;
    public Employee SelectedItem
    {
        get => selectedItem;
        set => SetProperty(ref selectedItem, value);
    }
    private string newFirstName;
    public string NewFirstName
    {
        get => newFirstName;
        set => SetProperty(ref newFirstName, value);
    }
    private string newLastName;
    public string NewLastName
    {
        get => newLastName;
        set => SetProperty(ref newLastName, value);
    }
    private double newHours = 38;
    public double NewHours
    {
        get => newHours;
        set
        {
            SetProperty(ref newHours, value);
            OnPropertyChanged(nameof(NewWrittingHours));
        }
    }
    public double NewWrittingHours
    {
        get => NewHours <= 32 ? 1 : 2;
    }
    private Group selectedGroup;
    public Group SelectedGroup
    {
        get => selectedGroup;
        set => SetProperty(ref selectedGroup, value);
    }

    private bool isAdd = true;
    private readonly List<Employee> newEmployees = new List<Employee>();
    public ICommand AddEmployCommand => new RelayCommand(AddEmployee);
    public ICommand DeleteEmployeeCommand => new RelayCommand(DeleteEmployee);
    public ICommand UpdateEmployeeCommand => new RelayCommand(UpdateEmployee);
    public ICommand SaveCommand => new RelayCommand(Save);
    public ICommand OkayCommand => new RelayCommand(Okay);
    public ICommand CancleCommand => new RelayCommand(Cancle);

    private void AddEmployee()
    {
        NewFirstName = "";
        NewLastName = "";
        NewHours = 38;
        SelectedGroup = Groups.FirstOrDefault();

        EditGridVisibility = Visibility.Visible;
        GridIsEnabled = false;
        isAdd = true;
    }
    private void DeleteEmployee()
    {
        if (SelectedItem is null)
            return;

        SelectedItem.IsOut = true;
        Employees.Remove(SelectedItem);
    }
    private void UpdateEmployee()
    {
        if (SelectedItem is null)
            return;

        NewFirstName = SelectedItem.FirstName;
        NewLastName = SelectedItem.LastName;
        NewHours = SelectedItem.Hours;
        SelectedGroup = SelectedItem.Group;

        EditGridVisibility = Visibility.Visible;
        GridIsEnabled = false;
        isAdd = false;
    }
    private void Save()
    {
        SaveEmployees?.Invoke(this, newEmployees);
        newEmployees.Clear();
    }
    private void Okay()
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
    private void Cancle()
    {
        GridIsEnabled = true;
        EditGridVisibility = Visibility.Collapsed;
    }
}
