using Xamarin.Forms;

namespace Volley.Mobile.Forms.Views.SetupElement
{
    public class StepperFormCell : ViewCell
    {
        #region BindableProperties

        public static readonly BindableProperty IsEditableProperty = BindableProperty.Create(nameof(IsEditable), typeof(bool), typeof(StepperFormCell), true);
        public static readonly BindableProperty TextProperty = BindableProperty.Create(nameof(Text), typeof(string), typeof(StepperFormCell));
        public static readonly BindableProperty DescriptionProperty = BindableProperty.Create(nameof(Description), typeof(string), typeof(StepperFormCell));
        public static readonly BindableProperty ValueProperty = BindableProperty.Create(nameof(Value), typeof(double), typeof(StepperFormCell), 0d);
        public static readonly BindableProperty IncrementProperty = BindableProperty.Create(nameof(Increment), typeof(double), typeof(StepperFormCell), 1d);
        public static readonly BindableProperty MaximumProperty = BindableProperty.Create(nameof(Maximum), typeof(double), typeof(StepperFormCell), 100d);
        public static readonly BindableProperty MinimumProperty = BindableProperty.Create(nameof(Minimum), typeof(double), typeof(StepperFormCell), 0d);
        //public static readonly

        #endregion BindableProperties

        #region CLR Properties

        public string Text { get => GetValue(TextProperty) as string; set => SetValue(TextProperty, value); }

        public bool IsEditable { get => (bool)GetValue(IsEditableProperty); set => SetValue(IsEditableProperty, value); }

        public string Description { get => GetValue(DescriptionProperty) as string; set => SetValue(DescriptionProperty, value); }

        public double Value { get => (double)GetValue(ValueProperty); set => SetValue(ValueProperty, value); }

        public double Increment { get => (double)GetValue(IncrementProperty); set => SetValue(IncrementProperty, value); }

        public double Maximum { get => (double)GetValue(MaximumProperty); set => SetValue(MaximumProperty, value); }

        public double Minimum { get => (double)GetValue(MinimumProperty); set => SetValue(MinimumProperty, value); }

        #endregion CLR Properties

        public StepperFormCell()
        {
            View = new StepperFormCellXaml
            {
                BindingContext = this
            };
        }
    }
}
