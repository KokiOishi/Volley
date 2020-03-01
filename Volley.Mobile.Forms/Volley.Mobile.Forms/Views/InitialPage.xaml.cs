using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Volley.Mobile.Forms.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class InitialPage : ContentPage
    {
        public InitialPage()
        {
            InitializeComponent();
        }

        private async void BMatchStart_ClickedAsync(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new MatchSetupPage());
        }

        private void BMatchHistory_Clicked(object sender, EventArgs e)
        {
        }
    }
}
