using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace Dienstplan;

abstract class VMBase : ObservableObject
{
    private Dictionary<string, object> propertyValues = new Dictionary<string, object>();

    //public event PropertyChangedEventHandler? PropertyChanged;
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
        OnPropertyChanged(propertyName);
    }
    internal void OnPropertyChanged(string propertyName)
    {
        base.OnPropertyChanged(new PropertyChangedEventArgs(propertyName));
    }
}
