using Xamarin.Forms;

namespace Volley.Mobile.Forms.Views
{
    public partial class PlayerDetailsPage : ContentPage
    {
        public PlayerDetailsPage()
        {
            InitializeComponent();
        }

        protected override bool OnBackButtonPressed() => false;
    }
}