using System;
using System.Reactive.Linq;
using Reactive.Bindings;
using ReactiveUI;
using ReactiveCommand = ReactiveUI.ReactiveCommand;
using Volley.Players;
using Volley.Standalone.Teams;
using Xamarin.Forms;
using System.Diagnostics;
using System.Windows.Input;
using Xamarin.Forms.Xaml;
using System.Collections.Generic;
using System.Linq;

namespace Volley.Mobile.Forms.Controls
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MatchInput : ContentView
    {
        #region BindableProperties

        public static readonly BindableProperty IsFlippedProperty
            = BindableProperty.Create(nameof(IsFlipped), typeof(bool), typeof(MatchInput),
                defaultValue: false, propertyChanged: (b, o, n) =>
                 {
                     if (b is MatchInput matchInput)
                         matchInput.IsFlippedInternal.Value = (bool)n;
                 });

        public static readonly BindableProperty CommandOnReceiverErroredProperty
            = BindableProperty.Create(nameof(CommandOnReceiverErrored), typeof(ICommand), typeof(MatchInput));

        public static readonly BindableProperty CommandOnReceiverReceivedProperty
            = BindableProperty.Create(nameof(CommandOnReceiverReceived), typeof(ICommand), typeof(MatchInput));

        public static readonly BindableProperty CommandOnReceivedErroredProperty
            = BindableProperty.Create(nameof(CommandOnReceivedErrored), typeof(ICommand), typeof(MatchInput));

        public static readonly BindableProperty CommandOnReceivedWonAPointProperty
            = BindableProperty.Create(nameof(CommandOnReceivedWonAPoint), typeof(ICommand), typeof(MatchInput));

        public static readonly BindableProperty CommandOnReceivedNetProperty
            = BindableProperty.Create(nameof(CommandOnReceivedNet), typeof(ICommand), typeof(MatchInput));

        public static readonly BindableProperty CommandOnCanceledProperty
            = BindableProperty.Create(nameof(CommandOnCanceled), typeof(ICommand), typeof(MatchInput));

        public static readonly BindableProperty CommandOnLetProperty
                    = BindableProperty.Create(nameof(CommandOnLet), typeof(ICommand), typeof(MatchInput));

        public static readonly BindableProperty PlayersAProperty
            = BindableProperty.Create(nameof(PlayersA), typeof(IEnumerable<PlayerModel>), typeof(MatchInput)
                , propertyChanged: (b, o, n) =>
                {
                    if (b is MatchInput matchInput && n is IEnumerable<PlayerModel> npm)
                        matchInput.PlayersAInternal.Value = npm;
                });

        public static readonly BindableProperty PlayersBProperty
            = BindableProperty.Create(nameof(PlayersB), typeof(IEnumerable<PlayerModel>), typeof(MatchInput)
                , propertyChanged: (b, o, n) =>
                {
                    if (b is MatchInput matchInput && n is IEnumerable<PlayerModel> npm)
                        matchInput.PlayersBInternal.Value = npm;
                });

        public static readonly BindableProperty IsServiceProperty
                            = BindableProperty.Create(nameof(IsService), typeof(bool), typeof(MatchInput), false, propertyChanged: (b, o, n) =>
                            {
                                if (n is bool q && b is MatchInput mi && !(mi.IsServiceInternal is null))
                                {
                                    mi.IsServiceInternal.Value = q;
                                }
                            });

        public static readonly BindableProperty IsPostServiceProperty
                                    = BindableProperty.Create(nameof(IsPostService), typeof(bool), typeof(MatchInput), false, propertyChanged: (b, o, n) =>
                                    {
                                        if (n is bool q && b is MatchInput mi && !(mi.IsPostServiceInternal is null))
                                        {
                                            mi.IsPostServiceInternal.Value = q;
                                        }
                                    });

        public static readonly BindableProperty PreviousPlayerProperty
                            = BindableProperty.Create(nameof(PreviousPlayer), typeof(Player), typeof(MatchInput), propertyChanged: (b, o, n) =>
                            {
                                if (n is Player q && b is MatchInput mi && !(mi.PreviousPlayerInternal is null))
                                {
                                    mi.PreviousPlayerInternal.Value = q;
                                }
                            });

        #endregion BindableProperties

        #region ReactiveProperties

        public ReadOnlyReactiveProperty<GridLength> ReceiverWidth { get; }
        public ReadOnlyReactiveProperty<GridLength> ServerWidth { get; }
        public ReadOnlyReactiveProperty<int> ReceiverColumn { get; }
        public ReadOnlyReactiveProperty<int> ServerColumn { get; }

        public ReactiveProperty<bool> IsFlippedInternal { get; }

        public ReadOnlyReactiveProperty<bool> IsReceivable { get; }

        public ReadOnlyReactiveProperty<bool> IsDoubles { get; }

        public ReadOnlyReactiveProperty<IEnumerable<PlayerModel>> Receivers { get; }
        public ReadOnlyReactiveProperty<IEnumerable<PlayerModelWithEnability>> Servers { get; }

        public ReactiveProperty<double> ControlRowHeight { get; } = new ReactiveProperty<double>();

        public ReactiveProperty<bool> IsServiceInternal { get; } = new ReactiveProperty<bool>(false);
        public ReactiveProperty<bool> IsPostServiceInternal { get; } = new ReactiveProperty<bool>(false);

        public ReactiveProperty<Player> PreviousPlayerInternal { get; } = new ReactiveProperty<Player>();

        public ReactiveProperty<IEnumerable<PlayerModel>> PlayersAInternal { get; } = new ReactiveProperty<IEnumerable<PlayerModel>>();

        public ReactiveProperty<IEnumerable<PlayerModel>> PlayersBInternal { get; } = new ReactiveProperty<IEnumerable<PlayerModel>>();

        public ReadOnlyReactiveProperty<string> LetButtonText { get; }

        #endregion ReactiveProperties

        #region Properties

        public bool IsFlipped
        {
            get => (bool)GetValue(IsFlippedProperty);
            set => SetValue(IsFlippedProperty, value);
        }

        public ICommand CommandOnReceiverErrored
        {
            get => (ICommand)GetValue(CommandOnReceiverErroredProperty);
            set => SetValue(CommandOnReceiverErroredProperty, value);
        }

        public ICommand CommandOnReceiverReceived
        {
            get => (ICommand)GetValue(CommandOnReceiverReceivedProperty);
            set => SetValue(CommandOnReceiverReceivedProperty, value);
        }

        public ICommand CommandOnReceivedErrored
        {
            get => (ICommand)GetValue(CommandOnReceivedErroredProperty);
            set => SetValue(CommandOnReceivedErroredProperty, value);
        }

        public ICommand CommandOnReceivedWonAPoint
        {
            get => (ICommand)GetValue(CommandOnReceivedWonAPointProperty);
            set => SetValue(CommandOnReceivedWonAPointProperty, value);
        }

        public ICommand CommandOnReceivedNet
        {
            get => (ICommand)GetValue(CommandOnReceivedNetProperty);
            set => SetValue(CommandOnReceivedNetProperty, value);
        }

        public ICommand CommandOnCanceled
        {
            get => (ICommand)GetValue(CommandOnCanceledProperty);
            set => SetValue(CommandOnCanceledProperty, value);
        }

        public ICommand CommandOnLet
        {
            get => (ICommand)GetValue(CommandOnLetProperty);
            set => SetValue(CommandOnLetProperty, value);
        }

        public IEnumerable<PlayerModel> PlayersA
        {
            get => (IEnumerable<PlayerModel>)GetValue(PlayersAProperty);
            set => SetValue(PlayersAProperty, value);
        }

        public IEnumerable<PlayerModel> PlayersB
        {
            get => (IEnumerable<PlayerModel>)GetValue(PlayersBProperty);
            set => SetValue(PlayersBProperty, value);
        }

        public bool IsService
        {
            get => (bool)GetValue(IsServiceProperty);
            set => SetValue(IsServiceProperty, value);
        }

        public bool IsPostService
        {
            get => (bool)GetValue(IsPostServiceProperty);
            set => SetValue(IsPostServiceProperty, value);
        }

        public Player PreviousPlayer
        {
            get => (Player)GetValue(PreviousPlayerProperty);
            set => SetValue(PreviousPlayerProperty, value);
        }

        #endregion Properties

        public MatchInput()
        {
            try
            {
                IsFlippedInternal = new ReactiveProperty<bool>(initialValue: IsFlipped);
                ReceiverWidth = IsFlippedInternal.Select(a => GetColumnWidth(a, true)).ToReadOnlyReactiveProperty();
                ServerWidth = IsFlippedInternal.Select(a => GetColumnWidth(a, false)).ToReadOnlyReactiveProperty();
                ReceiverColumn = IsFlippedInternal.Select(a => GetColumn(a, true)).ToReadOnlyReactiveProperty();
                ServerColumn = IsFlippedInternal.Select(a => GetColumn(a, false)).ToReadOnlyReactiveProperty();
                IsReceivable = IsServiceInternal.Select(a => !a).ToReadOnlyReactiveProperty();
                var ggg = PlayersAInternal.CombineLatest(PlayersBInternal, (a, b) => (pA: a, pB: b)).CombineLatest(IsFlippedInternal, (a, b) => (isf: b, pls: a));
                Receivers = ggg.Select(a => !a.isf ? a.pls.pA : a.pls.pB).ToReadOnlyReactiveProperty();
                Servers = ggg.Select(a => a.isf ? a.pls.pA : a.pls.pB).CombineLatest(PreviousPlayerInternal, (a, b) => (a, b))
                    .Select(a => a.a?.Select(b => new PlayerModelWithEnability(b.Player, b.Player == a.b)))
                    .ToReadOnlyReactiveProperty();
                LetButtonText = IsPostServiceInternal.Select(a => a ? "サービスレット" : "レット").ToReadOnlyReactiveProperty();
                //BindingContext = this;
                InitializeComponent();
                IsFlippedInternal.Value = !IsFlipped;
                IsFlippedInternal.Value = IsFlipped;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                throw;
            }
            /*
            RcInput.CommandOnReceive = ReactiveCommand.Create<Receive>(a => OnReceiverReceived(a));
            RcInput.CommandOnErrored = ReactiveCommand.Create(() => OnReceiverErrored());
            RdInput.CommandOnWinner = ReactiveCommand.Create(() => OnReceivedWonAPoint());
            RdInput.CommandOnErrored = ReactiveCommand.Create(() => OnReceivedErrored());
            RdInput.CommandOnNet = ReactiveCommand.Create(() => OnReceivedNet());
            */
            //BindingContext = this;
        }

        #region Reactions

        protected static GridLength GetColumnWidth(bool isFlipped, bool isReceiver)
        {
            switch (isFlipped ^ isReceiver)
            {
#pragma warning disable S125 // Sections of code should not be commented out
                case true:  //(isReceiver && !isFlipped) || (!isReceiver && isFlipped)
#pragma warning restore S125 // Sections of code should not be commented out
                    return new GridLength(2, GridUnitType.Star);

                case false:
                    return new GridLength(2, GridUnitType.Star);

                default:
                    return new GridLength(0);
            }
        }

        protected static int GetColumn(bool isFlipped, bool isReceiver)
        {
            switch (isFlipped ^ isReceiver)
            {
#pragma warning disable S125 // Sections of code should not be commented out
                case true:  //(isReceiver && !isFlipped) || (!isReceiver && isFlipped)
#pragma warning restore S125 // Sections of code should not be commented out
                    return 0;

                case false:
                    return 1;

                default:
                    return -1;
            }
        }

        #endregion Reactions

        #region MatchEvent

        private void OnReceiverErrored() =>
            //Receiver has lost a ball
            CommandOnReceiverErrored?.Execute(null);

        private void OnReceiverReceived(Receive receive) =>
            //Receiver successfully Received
            CommandOnReceiverReceived?.Execute(receive);

        private void OnReceivedErrored() =>
            //Received player has done something wrong
            CommandOnReceivedErrored?.Execute(null);

        private void OnReceivedWonAPoint() =>
            //Received player won a point
            CommandOnReceivedWonAPoint?.Execute(null);

        private void OnReceivedNet() =>
            //Receiver did a net play
            CommandOnReceivedNet?.Execute(null);

        #endregion MatchEvent
    }
}