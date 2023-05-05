using System.Linq;
using System;
using System.Windows.Controls;

namespace Dienstplan;
public partial class WeekSelectorView : UserControl
{
    public WeekSelectorView()
    {
        InitializeComponent();
        calendar.SelectedDatesChanged += Calendar_SelectedDatesChanged;
    }
    private void Calendar_SelectedDatesChanged(object? sender, System.Windows.Controls.SelectionChangedEventArgs e)
    {
        calendar.SelectedDatesChanged -= Calendar_SelectedDatesChanged;

        DateTime date = e.AddedItems.OfType<DateTime>().FirstOrDefault();
        if (date == default)
            return;

        int daysAfterMonday = ((int)date.DayOfWeek) - 1;

        // if sunday -> daysAfterMonday = -1 -> other week then last monday
        daysAfterMonday = daysAfterMonday == -1 ? 6 : daysAfterMonday;

        // subtract daysAfterMonday to get the monday of the selected week
        DateTime start = date.AddDays(-daysAfterMonday);
        
        calendar.SelectedDates.Clear();
        calendar.SelectedDates.AddRange(start, start.AddDays(6));
        
        calendar.SelectedDatesChanged += Calendar_SelectedDatesChanged;
    }
}

