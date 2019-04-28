using System.Windows;
using System.Windows.Controls;
using System.Windows.Interactivity;

namespace PaymentCalculator.Wpf
{
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
            this.AssociatedObject.GotFocus += this.OnTextBoxGotFocus;
        }

        protected override void OnDetaching()
        {
            this.AssociatedObject.GotFocus -= this.OnTextBoxGotFocus;
            base.OnDetaching();
        }

        private void OnTextBoxGotFocus(object sender, RoutedEventArgs e)
        {
            this.AssociatedObject.SelectAll();
        }
    }
}
