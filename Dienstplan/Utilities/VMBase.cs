using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace Dienstplan;
abstract class VMBase : INotifyPropertyChanged
{
    private Dictionary<string, object> propertyValues = new Dictionary<string, object>();

    public event PropertyChangedEventHandler? PropertyChanged;
    public T? GetValue<T>(T defaultValue = default, [CallerMemberName] string propertyName = "")
    {
        return propertyValues.TryGetValue(propertyName, out object value) ? (T)value : defaultValue;
    }
    public void SetValue(object value, [CallerMemberName] string propertyName = "")
    {
        if (propertyValues.TryGetValue(propertyName, out object storedValue))
            if (storedValue == value)
                return;

        propertyValues[propertyName] = value;
        OnPropertChanged(propertyName);
    }
    internal void OnPropertChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    public class Command : ICommand
    {
        public event EventHandler? CanExecuteChanged;

        private readonly Predicate<object> canExecute;
        private readonly Action<object> execute;

        public Command(Action<object> execute) : this(execute, null) { }
        public Command(Action<object> execute, Predicate<object> canExecute)
        {
            this.execute = execute;
            this.canExecute = canExecute;
        }

        public bool CanExecute(object parameter)
        {
            return canExecute?.Invoke(parameter) ?? true;
        }
        public void Execute(object parameter)
        {
            execute?.Invoke(parameter);
        }
        public void RaiseCanExecuteChanged()
        {
            CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}
