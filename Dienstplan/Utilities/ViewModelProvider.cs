using System;
using System.Windows.Markup;

namespace Dienstplan;

internal class ViewModelProvider : MarkupExtension
{
    public Type? Type { get; set; }
    public ViewModelProvider() { }
    public ViewModelProvider(Type type)
    {
        Type = type;
    }
    public override object? ProvideValue(IServiceProvider serviceProvider)
    {
        return App.Current?.Services?.GetService(Type);
    }
}
