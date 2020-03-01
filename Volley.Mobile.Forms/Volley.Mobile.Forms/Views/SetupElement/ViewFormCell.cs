using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace Volley.Mobile.Forms.Views.SetupElement
{
    public class ViewFormCell : ViewCell
    {
        #region BindableProperties

        public static readonly BindableProperty TextProperty = BindableProperty.Create(nameof(Text), typeof(string), typeof(ViewFormCell));
        public static readonly BindableProperty ContentProperty = BindableProperty.Create(nameof(Content), typeof(View), typeof(ViewFormCell));

        //public static readonly

        #endregion BindableProperties

        #region CLR Properties

        public string Text { get => GetValue(TextProperty) as string; set => SetValue(TextProperty, value); }
        public View Content { get => (View)GetValue(ContentProperty); set => SetValue(ContentProperty, value); }

        #endregion CLR Properties

        public ViewFormCell()
        {
            base.View = new ViewFormCellXaml()
            {
                BindingContext = this
            };
        }
    }
}