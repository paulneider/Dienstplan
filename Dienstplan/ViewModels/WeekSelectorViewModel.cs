using System;
using System.Windows;
using System.Windows.Input;

namespace Dienstplan;

class WeekSelectorViewModel : VMBase
{
    public event EventHandler<DateTime> NewWeekSelected;
    public Visibility Visibility 
    {
        get => GetValue<Visibility>(Visibility.Collapsed);
        set => SetValue(value); 
    }
    private DateTime selectedDate;
    public DateTime SelectedDate
    {
        get
        {
            return selectedDate;
        }
        set
        {
            selectedDate = value;
            OnPropertChanged(nameof(SelectedDate));
            OnPropertChanged(nameof(LabelContent));
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

    public ICommand OkayCommand => new Command(Okay);
    public ICommand CancleCommand => new Command(Cancle);
    
    private void Okay(object param)
    {
        NewWeekSelected?.Invoke(this, selectedDate);
        Visibility = Visibility.Collapsed;
    }
    private void Cancle(object param)
    {
        Visibility = Visibility.Collapsed;
    }
}
