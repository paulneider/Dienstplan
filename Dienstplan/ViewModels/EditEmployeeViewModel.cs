using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Dienstplan.Properties;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace Dienstplan;

internal class EditEmployeeViewModel : ObservableObject, IRecipient<UpdateEmployeeMessage>, IRecipient<AddEmployeeMessage>, IRecipient<GroupsUpdateMessage>
{
    private readonly ApplicationDbContext context;
    private readonly IMessenger messenger;

    private bool isAdd = true;
    private EmployeeItemViewModel? updateEmployee;
    public string Caption
    {
        get => isAdd ? Resources.NewEmployee_ : Resources.EditEmployee_;
    }
    private ObservableCollection<Group> groups;
    public ObservableCollection<Group> Groups
    {
        get => groups;
        set => SetProperty(ref groups, value);
    }
    private Group? selectedGroup;
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
    private string? newFirstName;
    public string NewFirstName
    {
        get => newFirstName;
        set => SetProperty(ref newFirstName, value);
    }
    private string? newLastName;
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
    public EditEmployeeViewModel() { }
    public EditEmployeeViewModel(ApplicationDbContext context, IMessenger messenger)
    {
        this.context = context;
        this.messenger = messenger;

        messenger.RegisterAll(this);

        Groups = new ObservableCollection<Group>(context.Groups.Where(x => !x.IsOut));
        SelectedGroup = Groups.FirstOrDefault();
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

            context.Employees.Add(newEmployee);
            messenger.Send(new NewEmployeeMessage(newEmployee));
        }
        else
        {
            updateEmployee.FirstName = NewFirstName;
            updateEmployee.LastName = NewLastName;
            updateEmployee.Hours = NewHours;
            updateEmployee.WrittingHours = NewWrittingHours;
            updateEmployee.Group = SelectedGroup;
        }

        context.SaveChanges();
        Visibility = Visibility.Collapsed;
    }
    private void Cancle()
    {
        Visibility = Visibility.Collapsed;
    }
    public void Receive(UpdateEmployeeMessage message)
    {
        NewFirstName = message.EmployeeItem.FirstName;
        NewLastName = message.EmployeeItem.LastName;
        NewHours = message.EmployeeItem.Hours;

        updateEmployee = message.EmployeeItem;

        Visibility = Visibility.Visible;
        isAdd = false;
        OnPropertyChanged(nameof(Caption));
    }
    public void Receive(AddEmployeeMessage message)
    {
        NewFirstName = "";
        NewLastName = "";
        NewHours = 38;

        Visibility = Visibility.Visible;
        isAdd = true;
        OnPropertyChanged(nameof(Caption));
    }
    public void Receive(GroupsUpdateMessage message)
    {
        Groups = new ObservableCollection<Group>(context.Groups.Where(x => !x.IsOut));
        SelectedGroup = Groups.FirstOrDefault();
    }   
}
