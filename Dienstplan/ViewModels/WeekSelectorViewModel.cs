using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using CommunityToolkit.Mvvm.Messaging.Messages;
using System;
using System.Windows;
using System.Windows.Input;

namespace Dienstplan;

internal class WeekSelectorViewModel : ObservableRecipient, IRecipient<ValueChangedMessage<DateTime>>
{
    private readonly IMessenger messenger;
    private Visibility visibility = Visibility.Collapsed;
    public Visibility Visibility
    {
        get => visibility;
        set => SetProperty(ref visibility, value);
    }
    private DateTime selectedDate;
    public DateTime SelectedDate
    {
        get => selectedDate;
        set
        {
            SetProperty(ref selectedDate, value);
            OnPropertyChanged(nameof(LabelContent));
        }
    }
    public string LabelContent
    {
        get
        {
            int dayIndex = 1 - ((int)SelectedDate.DayOfWeek);
            DateOnly start = DateOnly.FromDateTime(SelectedDate.AddDays(dayIndex == 1 ? -6 : dayIndex));

            return start.ToString("dd.MM") + " - " + start.AddDays(4).ToShortDateString();
        }
    }
    public ICommand OkayCommand => new RelayCommand(Okay);
    public ICommand CancleCommand => new RelayCommand(Cancle);

    public WeekSelectorViewModel() { }
    public WeekSelectorViewModel(IMessenger messenger)
    {
        this.messenger = messenger;
        messenger.Register(this);
    }

    private void Okay()
    {
        messenger.Send(new ValueChangedMessage<DateTime>(selectedDate));
        Visibility = Visibility.Collapsed;
    }
    private void Cancle()
    {
        Visibility = Visibility.Collapsed;
    }
    public void Receive(ValueChangedMessage<DateTime> message)
    {
        Visibility = Visibility.Visible;
        SelectedDate = message.Value;
    }
}
