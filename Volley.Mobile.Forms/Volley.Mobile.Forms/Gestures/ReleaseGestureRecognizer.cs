using System.Windows.Input;

namespace Xamarin.Forms
{
    public class ReleaseGestureRecognizer : Element, IGestureRecognizer
    {
        public static readonly BindableProperty CommandProperty =
            BindableProperty.Create("Command", typeof(ICommand), typeof(ReleaseGestureRecognizer), null, BindingMode.OneWay);

        public static readonly BindableProperty CommandParameterProperty =
            BindableProperty.Create("CommandParameter", typeof(object), typeof(ReleaseGestureRecognizer), null, BindingMode.TwoWay);

        public ICommand Command
        {
            get => (ICommand)GetValue(CommandProperty);
            set => SetValue(CommandProperty, value);
        }

        public object CommandParameter
        {
            get => GetValue(CommandParameterProperty);
            set => SetValue(CommandParameterProperty, value);
        }
    }
}