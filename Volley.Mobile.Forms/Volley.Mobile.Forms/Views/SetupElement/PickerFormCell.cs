using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using Reactive.Bindings;
using Xamarin.Forms;

namespace Volley.Mobile.Forms.Views.SetupElement
{
    public class PickerFormCell : ViewCell
    {
        #region BindableProperties

        public static readonly BindableProperty TextProperty
            = BindableProperty.Create(nameof(Text), typeof(string), typeof(PickerFormCell));

        public static readonly BindableProperty SelectedItemProperty
            = BindableProperty.Create(nameof(SelectedItem), typeof(object), typeof(PickerFormCell), defaultBindingMode: BindingMode.TwoWay);

        public static readonly BindableProperty SelectedIndexProperty
            = BindableProperty.Create(nameof(SelectedIndex), typeof(int), typeof(PickerFormCell), 0, defaultBindingMode: BindingMode.TwoWay);

        public static readonly BindableProperty ItemsProperty
            = BindableProperty.Create(nameof(Items), typeof(IList<string>), typeof(PickerFormCell), propertyChanged: (t, o, n) =>
             {
                 var nn = n as IList<string>;
                 /* if (t is PickerFormCell b)
                      b.ItemsInternal.Value = nn;*/
             });

        public static readonly BindableProperty TitleProperty
            = BindableProperty.Create(nameof(Title), typeof(string), typeof(PickerFormCell));

        //public static readonly

        #endregion BindableProperties

        #region CLR Properties

        public string Text { get => GetValue(TextProperty) as string; set => SetValue(TextProperty, value); }
        public string Title { get => GetValue(TitleProperty) as string; set => SetValue(TitleProperty, value); }
        public object SelectedItem { get => GetValue(SelectedItemProperty); set => SetValue(SelectedItemProperty, value); }
        public int SelectedIndex { get => (int)GetValue(SelectedIndexProperty); set => SetValue(SelectedIndexProperty, value); }
        public IList<string> Items { get => (IList<string>)GetValue(ItemsProperty); set => SetValue(ItemsProperty, value); }

        #endregion CLR Properties

        public PickerFormCell()
        {
            View = new PickerFormCellXaml
            {
                BindingContext = this
            };
        }
    }
}