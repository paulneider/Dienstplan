using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Windows;
using System.Windows.Input;

namespace Dienstplan;

internal class WeekSelectorViewModel : ObservableObject
{
    public event EventHandler<DateTime> NewWeekSelected;

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
    private void Okay()
    {
        NewWeekSelected?.Invoke(this, selectedDate);
        Visibility = Visibility.Collapsed;
    }
    public ICommand CancleCommand => new RelayCommand(Cancle);
    private void Cancle()
    {
        Visibility = Visibility.Collapsed;
    }
}
