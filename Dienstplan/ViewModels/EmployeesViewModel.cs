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

    private readonly List<Employee> newEmployees = new List<Employee>();
    public EditEmployeeViewModel EditEmployeeViewModel { get; init; } = new EditEmployeeViewModel();

    private ObservableCollection<EmployeeItemViewModel> employees = new ObservableCollection<EmployeeItemViewModel>();
    public ObservableCollection<EmployeeItemViewModel> Employees
    {
        get => employees;
        set => SetProperty(ref employees, value);
    }
    private EmployeeItemViewModel selectedItem;
    public EmployeeItemViewModel SelectedItem
    {
        get => selectedItem;
        set => SetProperty(ref selectedItem, value);
    }

    public ICommand AddEmployeeCommand => new RelayCommand(AddEmployee);
    public ICommand DeleteEmployeeCommand => new RelayCommand(DeleteEmployee);
    public ICommand UpdateEmployeeCommand => new RelayCommand(UpdateEmployee);
    public ICommand SaveCommand => new RelayCommand(Save);
    public EmployeesViewModel()
    {
        EditEmployeeViewModel.EmployeeAdded += EmployeeAdded;
    }

    private void EmployeeAdded(object? sender, Employee newEmployee)
    {
        newEmployees.Add(newEmployee);
        Employees.Add(new EmployeeItemViewModel(newEmployee));
    }
    private void AddEmployee()
    {
        EditEmployeeViewModel.Add();
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

        EditEmployeeViewModel.Update(SelectedItem);
    }
    private void Save()
    {
        SaveEmployees?.Invoke(this, newEmployees);
        newEmployees.Clear();
    }
}
