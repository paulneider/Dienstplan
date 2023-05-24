using System.Collections.ObjectModel;
using System.Windows.Input;
using System.Linq;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;

namespace Dienstplan;

internal partial class EmployeesViewModel : ObservableObject, IRecipient<NewEmployeeMessage>
{
    private readonly ApplicationDbContext context;
    private readonly IMessenger messenger;

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
    public EmployeesViewModel() { }
    public EmployeesViewModel(ApplicationDbContext context, IMessenger messenger)
    {
        this.context = context;
        this.messenger = messenger;

        messenger.Register(this);

        foreach (Employee employee in context.Employees.Where(x => !x.IsOut))
            Employees.Add(new EmployeeItemViewModel(employee));
    }
    private void AddEmployee()
    {
        messenger.Send(new AddEmployeeMessage());
    }
    private void DeleteEmployee()
    {
        if (SelectedItem is null)
            return;

        SelectedItem.IsOut = true;
        Employees.Remove(SelectedItem);
        context.SaveChanges();
    }
    private void UpdateEmployee()
    {
        if (SelectedItem is null)
            return;

        messenger.Send(new UpdateEmployeeMessage(SelectedItem));
    }
    public void Receive(NewEmployeeMessage message)
    {
        Employees.Add(new EmployeeItemViewModel(message.Employee));
    }
}
