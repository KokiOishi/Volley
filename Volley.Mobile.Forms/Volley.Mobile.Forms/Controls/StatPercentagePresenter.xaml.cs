using System;
using System.Reactive.Linq;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Reactive.Bindings;
using ReactiveUI;
using ReactiveCommand = ReactiveUI.ReactiveCommand;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.Diagnostics;

namespace Volley.Mobile.Forms.Controls
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class StatPercentagePresenter : ContentView
    {
        public static readonly BindableProperty ItemsProperty
            = BindableProperty.Create(nameof(Items), typeof(IEnumerable<StatItem>), typeof(StatPercentagePresenter), validateValue: (b, o) =>
            {
                Debug.WriteLine(o?.GetType());
                return o is null || o is IEnumerable<StatItem>;
            }, propertyChanged: (b, o, n) =>
              {
                  if (b is StatPercentagePresenter presenter && !(presenter.ItemsInternal is null))
                  {
                      presenter.ItemsInternal.Value = n as IEnumerable<StatItem>;
                  }
              });

        public static readonly BindableProperty ValueProperty
            = BindableProperty.Create(nameof(Value), typeof(double), typeof(StatPercentagePresenter), 0.0, validateValue: (b, o) =>
            {
                Debug.WriteLine(o.GetType());
                return o is double;
            }, propertyChanged: (b, o, n) =>
            {
                if (b is StatPercentagePresenter presenter && !(presenter.ValueInternal is null))
                {
                    presenter.ValueInternal.Value = (double)n;
                }
            });

        public static readonly BindableProperty ColorProperty
            = BindableProperty.Create(nameof(Color), typeof(Color), typeof(StatPercentagePresenter), Color.Black, validateValue: (b, o) =>
             {
                 Debug.WriteLine(o.GetType());
                 return o is Color;
             });

        protected ReactiveProperty<IEnumerable<StatItem>> ItemsInternal { get; } = new ReactiveProperty<IEnumerable<StatItem>>();

        protected ReactiveProperty<double> ValueInternal { get; } = new ReactiveProperty<double>(0);

        public ReadOnlyReactiveProperty<IEnumerable<StatItem>> ItemsToShow { get; }

        public IEnumerable<StatItem> Items
        {
            get => (IEnumerable<StatItem>)GetValue(ItemsProperty);
            set => SetValue(ItemsProperty, value);
        }

        public double Value
        {
            get => (double)GetValue(ValueProperty);
            set => SetValue(ValueProperty, value);
        }

        public Color Color
        {
            get => (Color)GetValue(ColorProperty);
            set => SetValue(ColorProperty, value);
        }

        public StatPercentagePresenter()
        {
            ItemsInternal.Value = Items;
            ItemsToShow = ItemsInternal.CombineLatest(ValueInternal, (a, b) =>
            {
                switch (a)
                {
                    case null:
                        return null;

                    default:
                        {
                            var g = 1.0 / a.Sum(h => h.Value);
                            g *= b;
                            return a.Select(k => new StatItem(k.Color, k.Value * g));
                        }
                }
            }).ToReadOnlyReactiveProperty();
            InitializeComponent();
        }
    }
}