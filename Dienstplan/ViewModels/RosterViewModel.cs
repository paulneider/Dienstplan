using System.Windows;

namespace Dienstplan;
internal class RosterViewModel : VMBase
{
    public Visibility Visibility
    {
        get => GetValue(Visibility.Collapsed);
        set => SetValue(value);
    }

}
