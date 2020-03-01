using System;
using System.Reactive.Linq;
using System.Collections.Generic;
using System.Linq;
using Reactive.Bindings;
using ReactiveUI;
using ReactiveCommand = ReactiveUI.ReactiveCommand;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Volley.Matches;
using System.Collections.ObjectModel;
using System.Diagnostics;
using Volley.Players;

namespace Volley.Mobile.Forms.Controls.MVCarousel
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Scoreboard : ContentView
    {
        #region BindableProperties

        public static readonly BindableProperty SetCountAProperty
            = BindableProperty.Create(nameof(SetCountA), typeof(string), typeof(Scoreboard), defaultValue: "0");

        public static readonly BindableProperty SetCountBProperty
                    = BindableProperty.Create(nameof(SetCountB), typeof(string), typeof(Scoreboard), defaultValue: "0");

        public static readonly BindableProperty GameCountAProperty
                    = BindableProperty.Create(nameof(GameCountA), typeof(string), typeof(Scoreboard), defaultValue: "0");

        public static readonly BindableProperty GameCountBProperty
                    = BindableProperty.Create(nameof(GameCountB), typeof(string), typeof(Scoreboard), defaultValue: "0");

        public static readonly BindableProperty PointCountAProperty
                    = BindableProperty.Create(nameof(PointCountA), typeof(string), typeof(Scoreboard), defaultValue: "0");

        public static readonly BindableProperty PointCountBProperty
                    = BindableProperty.Create(nameof(PointCountB), typeof(string), typeof(Scoreboard), defaultValue: "0");

        public static readonly BindableProperty IsFlippedProperty
            = BindableProperty.Create(nameof(IsFlipped), typeof(bool), typeof(Scoreboard),
                defaultValue: false, propertyChanged: (b, o, n) =>
                {
                    if (b is Scoreboard matchInput)
                        matchInput.IsFlippedInternal.Value = (bool)n;
                });

        public static readonly BindableProperty CurrentBallTeamProperty
            = BindableProperty.Create(nameof(CurrentBallTeam), typeof(EnumTeams), typeof(Scoreboard),
                defaultValue: EnumTeams.TeamA, propertyChanged: (b, o, n) =>
                {
                    if (b is Scoreboard matchInput)
                        matchInput.CurrentBallTeamInternal.Value = (EnumTeams)n;
                });

        public static readonly BindableProperty PlayersAProperty
            = BindableProperty.Create(nameof(PlayersA), typeof(IEnumerable<PlayerModel>), typeof(Scoreboard)
                , propertyChanged: (b, o, n) =>
                {
                    if (b is Scoreboard matchInput && n is IEnumerable<PlayerModel> npm)
                        matchInput.PlayersAInternal.Value = npm;
                });

        public static readonly BindableProperty PlayersBProperty
            = BindableProperty.Create(nameof(PlayersB), typeof(IEnumerable<PlayerModel>), typeof(Scoreboard)
                , propertyChanged: (b, o, n) =>
                {
                    if (b is Scoreboard matchInput && n is IEnumerable<PlayerModel> npm)
                        matchInput.PlayersBInternal.Value = npm;
                });

        #endregion BindableProperties

        #region Properties

        public string SetCountA
        {
            get => (string)GetValue(SetCountAProperty);
            set => SetValue(SetCountAProperty, value);
        }

        public string SetCountB
        {
            get => (string)GetValue(SetCountBProperty);
            set => SetValue(SetCountBProperty, value);
        }

        public string GameCountA
        {
            get => (string)GetValue(GameCountAProperty);
            set => SetValue(GameCountAProperty, value);
        }

        public string GameCountB
        {
            get => (string)GetValue(GameCountBProperty);
            set => SetValue(GameCountBProperty, value);
        }

        public string PointCountA
        {
            get => (string)GetValue(PointCountAProperty);
            set => SetValue(PointCountAProperty, value);
        }

        public string PointCountB
        {
            get => (string)GetValue(PointCountBProperty);
            set => SetValue(PointCountBProperty, value);
        }

        public bool IsFlipped
        {
            get => (bool)GetValue(IsFlippedProperty);
            set => SetValue(IsFlippedProperty, value);
        }

        public EnumTeams CurrentBallTeam
        {
            get => (EnumTeams)GetValue(CurrentBallTeamProperty);
            set => SetValue(CurrentBallTeamProperty, value);
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

        #endregion Properties

        #region ReactiveProperties

        public ReadOnlyReactiveProperty<double> PlayersFontSize { get; }

        public ReadOnlyReactiveProperty<double> PlayersRowHeight { get; }

        public ReadOnlyReactiveProperty<double> PlayersHeightRequest { get; }

        public ReadOnlyReactiveProperty<double> SetCountFontSize { get; }
        public ReadOnlyReactiveProperty<double> GameCountFontSize { get; }
        public ReactiveProperty<double> PointCountFontSize { get; }

        public ReactiveProperty<string> Hyphen { get; } = new ReactiveProperty<string>("―");

        public ReadOnlyReactiveProperty<double> BallIndicatorOpacityLeft { get; }

        public ReadOnlyReactiveProperty<double> BallIndicatorOpacityRight { get; }

        public ReadOnlyReactiveProperty<int> GridColumnA { get; }
        public ReadOnlyReactiveProperty<int> GridColumnSetA { get; }
        public ReadOnlyReactiveProperty<int> GridColumnB { get; }
        public ReadOnlyReactiveProperty<int> GridColumnSetB { get; }

        public ReadOnlyReactiveProperty<int> PListGridColumnA { get; }
        public ReadOnlyReactiveProperty<int> PListGridColumnB { get; }

        public ReadOnlyReactiveProperty<TextAlignment> HorizontalTextAlignmentA { get; }
        public ReadOnlyReactiveProperty<TextAlignment> HorizontalTextAlignmentB { get; }

        public ReactiveProperty<bool> IsFlippedInternal { get; } = new ReactiveProperty<bool>(false);

        public ReactiveProperty<EnumTeams> CurrentBallTeamInternal { get; } = new ReactiveProperty<EnumTeams>(EnumTeams.TeamA);

        public ReactiveProperty<IEnumerable<PlayerModel>> PlayersAInternal { get; } = new ReactiveProperty<IEnumerable<PlayerModel>>();

        public ReactiveProperty<IEnumerable<PlayerModel>> PlayersBInternal { get; } = new ReactiveProperty<IEnumerable<PlayerModel>>();

        #endregion ReactiveProperties

        public Scoreboard()
        {
            try
            {
                var dummy = new PlayerModel(new Standalone.Players.PlayerStandalone(Guid.NewGuid(), "88888888", Gender.Unidentified, 159, 50, Hands.Right, new Standalone.Color(0, 0, 0)));
                PlayersAInternal.Value = PlayersA = new PlayerModel[] { dummy };
                PlayersBInternal.Value = PlayersB = new PlayerModel[] { dummy };
                PointCountFontSize = new ReactiveProperty<double>(80);
                GameCountFontSize = PointCountFontSize.Select(a => a * 0.75).ToReadOnlyReactiveProperty();
                SetCountFontSize = PointCountFontSize.Select(a => a * 0.5).ToReadOnlyReactiveProperty();
                PlayersFontSize = PointCountFontSize.Select(a => a * 0.25).ToReadOnlyReactiveProperty();
                PlayersRowHeight = PlayersFontSize.Select(a => a * 1.25).ToReadOnlyReactiveProperty();
                PlayersHeightRequest = PlayersAInternal.CombineLatest(PlayersFontSize, (b, a) => a * 1.25 * b.Count()).ToReadOnlyReactiveProperty();
                BallIndicatorOpacityLeft = CurrentBallTeamInternal.CombineLatest(IsFlippedInternal, (a, b) => (b ^ (a == EnumTeams.TeamA)) ? 1 : 0.0).ToReadOnlyReactiveProperty();
                BallIndicatorOpacityRight = CurrentBallTeamInternal.CombineLatest(IsFlippedInternal, (a, b) => (b ^ (a == EnumTeams.TeamB)) ? 1 : 0.0).ToReadOnlyReactiveProperty();
                GridColumnA = IsFlippedInternal.Select(a => !a ? 1 : 4).ToReadOnlyReactiveProperty();
                GridColumnSetA = IsFlippedInternal.Select(a => !a ? 1 : 5).ToReadOnlyReactiveProperty();
                GridColumnB = IsFlippedInternal.Select(a => a ? 1 : 4).ToReadOnlyReactiveProperty();
                GridColumnSetB = IsFlippedInternal.Select(a => a ? 1 : 5).ToReadOnlyReactiveProperty();
                PListGridColumnA = IsFlippedInternal.Select(a => !a ? 0 : 4).ToReadOnlyReactiveProperty();
                PListGridColumnB = IsFlippedInternal.Select(a => a ? 0 : 4).ToReadOnlyReactiveProperty();
                HorizontalTextAlignmentA = IsFlippedInternal.Select(a => !a ? TextAlignment.End : TextAlignment.Start).ToReadOnlyReactiveProperty();
                HorizontalTextAlignmentB = IsFlippedInternal.Select(a => a ? TextAlignment.End : TextAlignment.Start).ToReadOnlyReactiveProperty();
                //BindingContext = this;
                InitializeComponent();
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.ToString());
                throw;
            }
            //BindingContext = this;
        }
    }
}