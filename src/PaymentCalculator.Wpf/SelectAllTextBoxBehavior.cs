using Microsoft.Xaml.Behaviors;
using System.Windows;
using System.Windows.Controls;

namespace PaymentCalculator.Wpf;

public class SelectAllTextBoxBehavior : Behavior<TextBox>
{
    public static void AddBehavior(params DependencyObject[] dependencyObjects)
    {
        foreach (var obj in dependencyObjects)
        {
            Interaction.GetBehaviors(obj).Add(new SelectAllTextBoxBehavior());
        }
    }

    protected override void OnAttached()
    {
        base.OnAttached();
        AssociatedObject.GotFocus += OnTextBoxGotFocus;
    }

    protected override void OnDetaching()
    {
        AssociatedObject.GotFocus -= OnTextBoxGotFocus;
        base.OnDetaching();
    }

    private void OnTextBoxGotFocus(object sender, RoutedEventArgs e)
    {
        AssociatedObject.SelectAll();
    }
}
