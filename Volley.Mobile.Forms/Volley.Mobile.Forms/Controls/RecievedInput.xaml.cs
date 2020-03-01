using System.Windows.Input;
using System.Reactive.Linq;
using ReactiveCommand = ReactiveUI.ReactiveCommand;
using Xamarin.Forms;
using Volley.Players;
using Reactive.Bindings;
using ReactiveUI;

namespace Volley.Mobile.Forms.Controls
{
    public partial class ReceivedInput : ContentView
    {
        #region BindableProperties

        public static readonly BindableProperty CommandOnWinnerProperty
            = BindableProperty.Create(nameof(CommandOnWinner), typeof(ICommand), typeof(ReceivedInput));

        public static readonly BindableProperty CommandOnErroredProperty
            = BindableProperty.Create(nameof(CommandOnErrored), typeof(ICommand), typeof(ReceivedInput));

        public static readonly BindableProperty CommandOnNetProperty
            = BindableProperty.Create(nameof(CommandOnNet), typeof(ICommand), typeof(ReceivedInput));

        public static readonly BindableProperty PlayerProperty
            = BindableProperty.Create(nameof(Player), typeof(Player), typeof(ReceivedInput));

        #endregion BindableProperties

        #region Properties

        public ICommand CommandOnWinner
        {
            get => (ICommand)GetValue(CommandOnWinnerProperty);
            set => SetValue(CommandOnWinnerProperty, value);
        }

        public ICommand CommandOnErrored
        {
            get => (ICommand)GetValue(CommandOnErroredProperty);
            set => SetValue(CommandOnErroredProperty, value);
        }

        public ICommand CommandOnNet
        {
            get => (ICommand)GetValue(CommandOnNetProperty);
            set => SetValue(CommandOnNetProperty, value);
        }

        public Player Player
        {
            get => (Player)GetValue(PlayerProperty);
            set => SetValue(PlayerProperty, value);
        }

        #endregion Properties

        public ReadOnlyReactiveProperty<Color> ButtonsBGColor { get; }

        public ReceivedInput()
        {
            ButtonsBGColor = this.ObservableForProperty(a => a.IsEnabled)
                .Select(a => (bool)GetValue(IsEnabledProperty) ? Color.Transparent : new Color(0.5, 0.5, 0.5, 0.75)).ToReadOnlyReactiveProperty();
            //BindingContext = this;
            InitializeComponent();
            //BindingContext = this;
            var q = IsEnabled;
            IsEnabled = !q;
            IsEnabled = q;
            ButtonWinner.Command = ReactiveCommand.Create(() =>
                CommandOnWinner?.Execute(Player));
            ButtonErrored.Command = ReactiveCommand.Create(() =>
                CommandOnErrored?.Execute(Player));
            ButtonNet.Command = ReactiveCommand.Create(() =>
                CommandOnNet?.Execute(Player));
        }
    }
}