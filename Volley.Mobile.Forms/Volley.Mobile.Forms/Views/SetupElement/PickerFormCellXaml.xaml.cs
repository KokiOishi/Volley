using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Volley.Mobile.Forms.Views.SetupElement
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PickerFormCellXaml : ContentView
    {
        public void RefreshItems()
        {
        }

        public PickerFormCellXaml()
        {
            InitializeComponent();
        }
    }
}