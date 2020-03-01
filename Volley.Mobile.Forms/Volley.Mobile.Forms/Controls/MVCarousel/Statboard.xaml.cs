using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volley.Standalone.Stats;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Volley.Mobile.Forms.Controls.MVCarousel
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Statboard : ContentView
    {
        public static readonly BindableProperty StatsProperty
            = BindableProperty.Create(nameof(Stats), typeof(IEnumerable<IStatItem<double>>), typeof(Statboard));

        public Statboard()
        {
            InitializeComponent();
        }

        public IEnumerable<IStatItem<double>> Stats
        { get => (IEnumerable<IStatItem<double>>)GetValue(StatsProperty); set => SetValue(StatsProperty, value); }
    }
}
