using Reactive.Bindings;
using System.Reactive.Linq;
using System.Windows.Input;
using Xamarin.Forms;
using ReactiveUI;
using ReactiveCommand = ReactiveUI.ReactiveCommand;

namespace Volley.Mobile.Forms.Controls.Singleplayer
{
    public partial class ReceiverInputSingleplayer : ContentView
    {
        public static readonly BindableProperty IsFlippedProperty
            = BindableProperty.Create(nameof(IsFlipped), typeof(bool), typeof(MatchInput),
                defaultValue: false);

        public static readonly BindableProperty CommandOnPointProperty
            = BindableProperty.Create(nameof(CommandOnPoint), typeof(ICommand), typeof(MatchInput));

        public static readonly BindableProperty CommandOnErrorProperty
            = BindableProperty.Create(nameof(CommandOnError), typeof(ICommand), typeof(MatchInput));

        #region Commands

        public ICommand CommandOnPoint
        {
            get => (ICommand)GetValue(CommandOnPointProperty);
            set => SetValue(CommandOnPointProperty, value);
        }

        public ICommand CommandOnError
        {
            get => (ICommand)GetValue(CommandOnErrorProperty);
            set => SetValue(CommandOnErrorProperty, value);
        }

        #endregion Commands

        protected ReadOnlyReactiveProperty<int> VolleyColumn { get; }
        protected ReadOnlyReactiveProperty<int> StrokeColumn { get; }

        protected ReactiveProperty<bool> IsFlippedInternal { get; }

        public ReceiverInputSingleplayer()
        {
            BindingContext = this;
            InitializeComponent();
            IsFlippedInternal = new ReactiveProperty<bool>(initialValue: IsFlipped);
            StrokeColumn = IsFlippedInternal.Select(a => GetColumn(a, false)).ToReadOnlyReactiveProperty();
            VolleyColumn = IsFlippedInternal.Select(a => GetColumn(a, true)).ToReadOnlyReactiveProperty();
            ButtonSF.Command = ReactiveCommand.Create(() => CommandOnPoint.Execute(new Receive(null, Players.HandSide.Fore, Pointing.ShotKind.Stroke)));
        }

        public bool IsFlipped
        {
            get => (bool)GetValue(IsFlippedProperty);
            set
            {
                SetValue(IsFlippedProperty, value);
                IsFlippedInternal.Value = value;
            }
        }

        #region Reactions

        protected static int GetColumn(bool isFlipped, bool isVolley)
        {
            switch (isFlipped ^ isVolley)
            {
#pragma warning disable S125 // Sections of code should not be commented out
                case true:  //(isVolley && !isFlipped) || (!isVolley && isFlipped)
#pragma warning restore S125 // Sections of code should not be commented out
                    return 0;

                case false:
                    return 1;

                default:
                    return -1;
            }
        }

        #endregion Reactions
    }
}