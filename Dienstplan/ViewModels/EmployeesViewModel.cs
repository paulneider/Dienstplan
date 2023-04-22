using System.Windows;

namespace Dienstplan;
internal class EmployeesViewModel : VMBase
{
    private Visibility visibility = Visibility.Collapsed;
    public Visibility Visibility
    {
        get { return visibility; }
        set
        {
            visibility = value;
            OnPropertChanged(nameof(Visibility));
        }
    }

    public EmployeesViewModel()
    {
        
    }
}
