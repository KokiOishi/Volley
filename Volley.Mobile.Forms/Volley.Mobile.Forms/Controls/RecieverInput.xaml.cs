using System.Reactive.Linq;
using System.Windows.Input;
using Reactive.Bindings;
using Xamarin.Forms;
using ReactiveUI;
using ReactiveCommand = ReactiveUI.ReactiveCommand;
using Volley.Team;
using System.Diagnostics;
using Volley.Players;

namespace Volley.Mobile.Forms.Controls
{
    public partial class ReceiverInput : ContentView
    {
        #region BindableProperties

        public static readonly BindableProperty IsFlippedProperty =
            BindableProperty.Create(nameof(IsFlipped), typeof(bool), typeof(ReceiverInput), propertyChanged: UpdateIsFlipped);

        public static readonly BindableProperty IsCompactProperty =
            BindableProperty.Create(nameof(IsCompact), typeof(bool), typeof(ReceiverInput));

        public static readonly BindableProperty IsServiceProperty =
                    BindableProperty.Create(nameof(IsService), typeof(bool), typeof(ReceiverInput), propertyChanged: (b, o, n) =>
                     {
                         if (b is ReceiverInput receiverInput && n is bool bvalue)
                         {
                             receiverInput.IsServiceInternal.Value = bvalue;
                         }
                     });

        public static readonly BindableProperty CommandOnReceiveProperty
            = BindableProperty.Create(nameof(CommandOnReceive), typeof(ICommand), typeof(ReceiverInput));

        public static readonly BindableProperty CommandOnErroredProperty
            = BindableProperty.Create(nameof(CommandOnErrored), typeof(ICommand), typeof(ReceiverInput));

        public static readonly BindableProperty PlayerProperty
            = BindableProperty.Create(nameof(Player), typeof(Player), typeof(ReceiverInput));

        #endregion BindableProperties

        #region ReactiveProperties

        public ReactiveProperty<bool> IsFlippedInternal { get; }

        public ReactiveProperty<bool> IsServiceInternal { get; }

        public ReadOnlyReactiveProperty<bool> IsNotService { get; }

        public ReadOnlyReactiveProperty<int> VolleyColumn { get; }
        public ReadOnlyReactiveProperty<int> StrokeColumn { get; }

        public ICommand CommandOnClickedPlayer { get; } = ReactiveCommand.Create(() =>
        {
        });

        #endregion ReactiveProperties

        #region Properties

        /// <summary>
        /// Indicates whether to flip the view or not.
        /// The volley buttons comes further from your finger than stroke buttons if this is set correctly.
        /// A team: always false
        /// B team: always true
        /// </summary>
        public bool IsFlipped { get => (bool)GetValue(IsFlippedProperty); set => SetValue(IsFlippedProperty, value); }

        /// <summary>
        /// Indicates whether to show a compact view for multiplayer.
        /// </summary>
        public bool IsCompact { get => (bool)GetValue(IsCompactProperty); set => SetValue(IsCompactProperty, value); }

        public bool IsService { get => (bool)GetValue(IsServiceProperty); set => SetValue(IsServiceProperty, value); }

        public bool IsPlayerFirst { get; set; }

        public ICommand CommandOnReceive
        {
            get => (ICommand)GetValue(CommandOnReceiveProperty);
            set => SetValue(CommandOnReceiveProperty, value);
        }

        public ICommand CommandOnErrored
        {
            get => (ICommand)GetValue(CommandOnErroredProperty);
            set => SetValue(CommandOnErroredProperty, value);
        }

        public Player Player
        {
            get => (Player)GetValue(PlayerProperty);
            set => SetValue(PlayerProperty, value);
        }

        #endregion Properties

        #region Property Reactions

        protected static void UpdateIsFlipped(BindableObject sender, object oldValue, object newValue)
        {
            if (sender is ReceiverInput ReceiverInput)
            {
                var o = oldValue as bool? ?? false;
                var n = newValue as bool? ?? false;
                if (o ^ n)
                {
                    ReceiverInput.IsFlippedInternal.Value = n;
                }
            }
        }

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

        #endregion Property Reactions

        public ReceiverInput()
        {
            IsFlippedInternal = new ReactiveProperty<bool>(initialValue: IsFlipped);
            IsServiceInternal = new ReactiveProperty<bool>(initialValue: IsService);
            StrokeColumn = IsFlippedInternal.Select(a => GetColumn(a, false)).ToReadOnlyReactiveProperty();
            VolleyColumn = IsFlippedInternal.Select(a => GetColumn(a, true)).ToReadOnlyReactiveProperty();
            IsNotService = IsServiceInternal.Select(a => !a).ToReadOnlyReactiveProperty();
            //BindingContext = this;
            InitializeComponent();
            //Debug.WriteLine(BindingContext?.ToString() ?? "null");
            //BindingContext = this;
            ButtonSF.Command = ReactiveCommand.Create(() =>
                CommandOnReceive?.Execute(new Receive(Player, HandSide.Fore, Pointing.ShotKind.Stroke)));
            ButtonVF.Command = ReactiveCommand.Create(() =>
                CommandOnReceive?.Execute(new Receive(Player, HandSide.Fore, Pointing.ShotKind.Volley)));
            ButtonErrored.Command = ReactiveCommand.Create(() =>
                CommandOnErrored?.Execute(Player));
            ButtonSB.Command = ReactiveCommand.Create(() =>
                CommandOnReceive?.Execute(new Receive(Player, HandSide.Back, Pointing.ShotKind.Stroke)));
            ButtonVB.Command = ReactiveCommand.Create(() =>
                CommandOnReceive?.Execute(new Receive(Player, HandSide.Back, Pointing.ShotKind.Volley)));

#if DEBUG
            //CommandOnReceive = ReactiveCommand.Create<Receive>(a =>
            //{
            //    Debug.WriteLine($"{a.Side}, {a.Kind}");
            //    IsFlipped = !IsFlipped;
            //});
#endif
        }
    }
}