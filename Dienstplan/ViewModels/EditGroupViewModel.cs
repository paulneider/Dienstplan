using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using System.Windows;
using System.Windows.Input;

namespace Dienstplan;

internal class EditGroupViewModel : ObservableObject, IRecipient<AddGroupMessage>, IRecipient<UpdateGroupMessage>
{
    private readonly ApplicationDbContext context;
    private readonly IMessenger messenger;

    private GroupItemViewModel updateGroup;
    private bool isAdd = false;
    public string Caption
    {
        get => isAdd ? "Neue Gruppe:" : "Gruppe bearbeiten:";
    }
    private Visibility visibility = Visibility.Collapsed;
    public Visibility Visibility
    {
        get => visibility;
        set => SetProperty(ref visibility, value);
    }
    private string newName;
    public string NewName
    {
        get => newName;
        set => SetProperty(ref newName, value);
    }
    private GroupType newType;
    public GroupType NewType
    {
        get => newType;
        set => SetProperty(ref newType, value);
    }
    public ICommand OkayCommand => new RelayCommand(Okay);
    public ICommand CancleCommand => new RelayCommand(Cancle);
    public EditGroupViewModel() { }
    public EditGroupViewModel(ApplicationDbContext context, IMessenger messenger)
    {
        this.context = context;
        this.messenger = messenger;

        messenger.RegisterAll(this);
    }
    private void Okay()
    {
        if (string.IsNullOrWhiteSpace(NewName))
            return;

        if (isAdd)
        {
            Group newGroup = new Group();

            newGroup.Name = NewName;
            newGroup.Type = NewType;

            context.Groups.Add(newGroup);
            messenger.Send(new NewGroupMessage(newGroup));
        }
        else
        {
            updateGroup.Name = NewName;
            updateGroup.Type = NewType;
        }

        context.SaveChanges();

        messenger.Send(new GroupsUpdateMessage());
        
        Visibility = Visibility.Collapsed;
    }
    private void Cancle()
    {
        Visibility = Visibility.Collapsed;
    }
    public void Receive(AddGroupMessage message)
    {
        NewName = "";
        NewType = GroupType.Small;

        Visibility = Visibility.Visible;
        isAdd = true;
        OnPropertyChanged(nameof(Caption));
    }
    public void Receive(UpdateGroupMessage message)
    {
        NewName = message.GroupItem.Name;
        NewType = message.GroupItem.Type;

        updateGroup = message.GroupItem;

        Visibility = Visibility.Visible;
        isAdd = false;
        OnPropertyChanged(nameof(Caption));
    }
}
