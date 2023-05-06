using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Windows;

namespace Dienstplan;

[INotifyPropertyChanged]
internal partial class WeekSelectorViewModel
{
    public event EventHandler<DateTime> NewWeekSelected;

    [ObservableProperty]
    private Visibility visibility = Visibility.Collapsed;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(LabelContent))]
    private DateTime selectedDate;
    public string LabelContent
    {
        get
        {
            int dayIndex = 1 - ((int)SelectedDate.DayOfWeek);
            DateOnly start = DateOnly.FromDateTime(SelectedDate.AddDays(dayIndex == 1 ? -6 : dayIndex));

            return start.ToString("dd.MM") + " - " + start.AddDays(4).ToShortDateString();
        }
    }

    [RelayCommand]
    private void Okay(object param)
    {
        NewWeekSelected?.Invoke(this, selectedDate);
        Visibility = Visibility.Collapsed;
    }
    [RelayCommand]
    private void Cancle(object param)
    {
        Visibility = Visibility.Collapsed;
    }
}
