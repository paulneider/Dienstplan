using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.ObjectModel;
using System.Reflection.Metadata;
using System.Windows;
using System.Windows.Input;

namespace Dienstplan;

internal class EditEmployeeViewModel : ObservableObject
{
    public event EventHandler<Employee> EmployeeAdded;

    private bool isAdd = true;
    private EmployeeItemViewModel updateEmployee;
    public string Caption
    {
        get => isAdd ? "Neuer Mitarbeiter:" : "Mitarbeiter bearbeiten:";
    }

    private Group selectedGroup;
    public Group SelectedGroup
    {
        get => selectedGroup;
        set => SetProperty(ref selectedGroup, value);
    }
    private Visibility visibility = Visibility.Collapsed;
    public Visibility Visibility
    {
        get => visibility;
        set => SetProperty(ref visibility, value);
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
    public ICommand OkayCommand => new RelayCommand(Okay);
    public ICommand CancleCommand => new RelayCommand(Cancle);
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

            EmployeeAdded?.Invoke(this, newEmployee);
        }
        else
        {
            updateEmployee.FirstName = NewFirstName;
            updateEmployee.LastName = NewLastName;
            updateEmployee.Hours = NewHours;
            updateEmployee.WrittingHours = NewWrittingHours;
            updateEmployee.Group = SelectedGroup;

        }

        Visibility = Visibility.Collapsed;
    }
    private void Cancle()
    {
        Visibility = Visibility.Collapsed;
    }
    public void Add()
    {
        NewFirstName = "";
        NewLastName = "";
        NewHours = 38;

        Visibility = Visibility.Visible;
        isAdd = true;
        OnPropertyChanged(nameof(Caption));
    }
    public void Update(EmployeeItemViewModel employee)
    {
        NewFirstName = employee.FirstName;
        NewLastName = employee.LastName;
        NewHours = employee.Hours;

        updateEmployee = employee;

        Visibility = Visibility.Visible;
        isAdd = false;
        OnPropertyChanged(nameof(Caption));
    }
}
