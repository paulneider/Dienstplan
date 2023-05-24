using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;

namespace Dienstplan;

internal partial class GroupsViewModel : ObservableObject, IRecipient<NewGroupMessage>
{
    private readonly ApplicationDbContext context;
    private readonly IMessenger messenger;

    private ObservableCollection<GroupItemViewModel> groups = new ObservableCollection<GroupItemViewModel>();
    public ObservableCollection<GroupItemViewModel> Groups
    {
        get => groups;
        set => SetProperty(ref groups, value);
    }
    private GroupItemViewModel selectedItem;
    public GroupItemViewModel SelectedItem
    {
        get => selectedItem;
        set => SetProperty(ref selectedItem, value);
    }
    public ICommand AddGroupCommand => new RelayCommand(AddGroup);
    public ICommand DeleteGroupCommand => new RelayCommand(DeleteGroup);
    public ICommand UpdateGroupCommand => new RelayCommand(UpdateGroup);
    public GroupsViewModel() { }
    public GroupsViewModel(ApplicationDbContext context, IMessenger messenger)
    {
        this.context = context;
        this.messenger = messenger;

        messenger.RegisterAll(this);

        foreach (Group group in context.Groups.Where(x => !x.IsOut))
            Groups.Add(new GroupItemViewModel(group));
    }

    private void AddGroup()
    {
        messenger.Send(new AddGroupMessage());
    }
    private void DeleteGroup()
    {
        if (SelectedItem is null)
            return;

        SelectedItem.IsOut = true;
        Groups.Remove(SelectedItem);
        context.SaveChanges();

        messenger.Send(new GroupsUpdateMessage());
    }
    private void UpdateGroup()
    {
        if (SelectedItem is null)
            return;

        messenger.Send(new UpdateGroupMessage(SelectedItem));
    }
    public void Receive(NewGroupMessage message)
    {
        Groups.Add(new GroupItemViewModel(message.Group));
    }
}
