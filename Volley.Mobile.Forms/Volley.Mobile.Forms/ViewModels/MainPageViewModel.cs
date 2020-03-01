using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Reactive.Linq;
using System.Threading.Tasks;
using System.Windows.Input;

using Prism.Navigation;

using Reactive.Bindings;

using Volley.Games;
using Volley.Matches;
using Volley.Mobile.Forms.ViewModels.Parameters;
using Volley.Players;
using Volley.Sets;
using Volley.Standalone.Games;
using Volley.Standalone.Matches;
using Volley.Standalone.Stats;
using Volley.Standalone.Stats.Calculators;
using Volley.Standalone.Teams;
using Xamarin.Forms;
using ReactiveCommand = ReactiveUI.ReactiveCommand;
using TTeam = Volley.Standalone.Teams.GenericTeam;

namespace Volley.Mobile.Forms.ViewModels
{
    public class MainPageViewModel : ViewModelBase, IInitializeAsync
    {
        private const double VibrationDurationOnCancelLet = 500;
        private const double VibrationDurationOnPoint = 200;
        private const double VibrationDurationOnPress = 50;
        private MatchStandalone<TTeam> match;
        private Rally<TTeam> rally;
        private TTeam teamA, teamB;

        private List<IDisposable> DisposableObjects { get; } = new List<IDisposable>();

        Game<TTeam> Game => match.CurrentSet.CurrentGame;

        private ReactiveProperty<(IEnumerable<PlayerModel> A, IEnumerable<PlayerModel> B)> PlayerModels { get; } =
            new ReactiveProperty<(IEnumerable<PlayerModel> A, IEnumerable<PlayerModel> B)>();

        private Set<TTeam> Set => match.CurrentSet;

        private List<IStatCalculator<double>> StatCalculators { get; }

        private IEnumerable<PlayerModel> players { get; set; }

        public MainPageViewModel(INavigationService navigationService)
                                                    : base(navigationService)
        {
            StatPlayersItemSource.Value = new ObservableCollection<StatPlayerModel>();
            Title = "Main Page";
            TeamLeft = IsEndFlipped.Select(a => !a ? teamA : teamB).ToReadOnlyReactiveProperty();
            TeamRight = IsEndFlipped.Select(a => a ? teamB : teamA).ToReadOnlyReactiveProperty();

            CommandOnReceivedErrored = ReactiveCommand.Create<Player>(a =>
            {
                Debug.WriteLine($"Received \"{a.Name}\"({a.Identifier}) errored");
                DumpError(() => rally?.ReceivedError(a));
                Vibrate(VibrationDurationOnPoint);
            });
            CommandOnReceivedNet = ReactiveCommand.Create<Player>(a =>
            {
                Debug.WriteLine($"Received \"{a.Name}\"({a.Identifier}) net");
                DumpError(() => rally?.ReceivedErroredInforcefully(a));
                Vibrate(VibrationDurationOnPoint);
            });
            CommandOnReceivedWonAPoint = ReactiveCommand.Create<Player>(a =>
            {
                Debug.WriteLine($"Received \"{a.Name}\"({a.Identifier}) won a point");
                DumpError(() => rally?.ReceivedWinAPoint(a));
                Vibrate(VibrationDurationOnPoint);
            });
            CommandOnReceiverErrored = ReactiveCommand.Create<Player>(a =>
            {
                Debug.WriteLine($"Receiver \"{a.Name}\"({a.Identifier}) forcefully errored");
                DumpError(() => rally?.ReceiverError(a));
                Vibrate(VibrationDurationOnPoint);
            });
            CommandOnReceiverReceived = ReactiveCommand.Create<Receive>(a =>
            {
                Debug.WriteLine($"Receiver \"{a.ReceivedPlayer.Name}\"({a.ReceivedPlayer.Identifier}) Received with {a.Side}, {a.Kind}");
                DumpError(() => rally?.ReceiverReceive(a));
                Vibrate(VibrationDurationOnPress);
            });
            CommandOnCanceled = ReactiveCommand.Create(() =>
            {
                Debug.WriteLine($"The previous input has been canceled");
                DumpError(() => rally?.Cancel());
                RefreshPoints();
                Vibrate(VibrationDurationOnCancelLet);
            });
            CommandOnLet = ReactiveCommand.Create(() =>
            {
                Debug.WriteLine($"The previous input has been canceled");
                DumpError(() => rally?.Let());
                RefreshPoints();
                Vibrate(VibrationDurationOnCancelLet);
            });
            StatCalculators = new List<IStatCalculator<double>>
            {
                new ServiceStatCalculator(),
                new PointStatCalculator()
            };
            InputPlayerA = PlayerModels.CombineLatest(IsEndFlipped, (a, b) => (a, b)).Select(a => a.b ? a.a.B : a.a.A).ToReadOnlyReactiveProperty();
            InputPlayerB = PlayerModels.CombineLatest(IsEndFlipped, (a, b) => (a, b)).Select(a => !a.b ? a.a.B : a.a.A).ToReadOnlyReactiveProperty();
        }

        #region ReactiveProperties

        public ReactiveProperty<EnumTeams> CurrentBallTeam { get; private set; } = new ReactiveProperty<EnumTeams>();

        public ReactiveProperty<string> GameCountA { get; } = new ReactiveProperty<string>();

        public ReactiveProperty<string> GameCountB { get; } = new ReactiveProperty<string>();

        public ReadOnlyReactiveProperty<IEnumerable<PlayerModel>> InputPlayerA { get; private set; }

        public ReadOnlyReactiveProperty<IEnumerable<PlayerModel>> InputPlayerB { get; private set; }

        public ReactiveProperty<ObservableCollection<StatPlayerModel>> StatPlayersItemSource { get; } = new ReactiveProperty<ObservableCollection<StatPlayerModel>>();

        public ReactiveProperty<bool> IsEndFlipped { get; } = new ReactiveProperty<bool>(false);

        public ReactiveProperty<bool> IsFlipped { get; } = new ReactiveProperty<bool>(false);

        public ReactiveProperty<bool> IsInputFlipped { get; private set; } = new ReactiveProperty<bool>();

        public ReactiveProperty<bool> IsPostService { get; private set; } = new ReactiveProperty<bool>(false);

        public ReactiveProperty<bool> IsService { get; private set; } = new ReactiveProperty<bool>(false);

        public ReactiveProperty<string> PointCountA { get; } = new ReactiveProperty<string>();

        public ReactiveProperty<string> PointCountB { get; } = new ReactiveProperty<string>();

        public ReactiveProperty<Player> PreviousPlayer { get; } = new ReactiveProperty<Player>();

        public ReactiveProperty<string> SetCountA { get; } = new ReactiveProperty<string>();

        public ReactiveProperty<string> SetCountB { get; } = new ReactiveProperty<string>();

        public ReadOnlyReactiveProperty<TTeam> TeamLeft { get; }

        public ReadOnlyReactiveProperty<TTeam> TeamRight { get; }

        #endregion ReactiveProperties

        #region Commands

        public ICommand CommandOnCanceled { get; }

        public ICommand CommandOnLet { get; }

        public ICommand CommandOnReceivedErrored { get; }

        public ICommand CommandOnReceivedNet { get; }

        public ICommand CommandOnReceivedWonAPoint { get; }

        public ICommand CommandOnReceiverErrored { get; }

        public ICommand CommandOnReceiverReceived { get; }

        #endregion Commands

        public async Task InitializeAsync(INavigationParameters parameters)
        {
            try
            {
                var p = parameters.GetValue<MatchStartParameters>("P");
                match = p.Match;
                teamA = p.Match.TeamA;
                teamB = p.Match.TeamB;
                match.StartMatch();
                //match.CurrentSet.StartSet();
                //match.CurrentSet.CurrentGame.StartGame();

                match.PropertyChanged += Match_PropertyChanged;

                PlayerModels.Value = match.GeneratePlayerModels();
                players = PlayerModels.Value.A.Concat(PlayerModels.Value.B).ToArray();
                rally = new Rally<TTeam>(match);
                rally.PropertyChanged += Match_PropertyChanged;
                /*DisposableObjects.Add(IsEndFlipped.Subscribe(a =>
                            {
                                MIMain.PlayersA.Value = SB.PlayersA.Value = a ? PlayerModels.B : PlayerModels.A;
                                MIMain.PlayersB.Value = SB.PlayersB.Value = a ? PlayerModels.A : PlayerModels.B;
                                MIMain.IsFlipped = rally.FlipInput ^ a;
                                SB.CurrentBallTeam = a ? rally.CurrentBallTeam.Flip() : rally.CurrentBallTeam;
                            }));*/
                StatPlayersItemSource.Value.Clear();
                foreach (var item in GenerateStats(players).ToArray())
                {
                    StatPlayersItemSource.Value.Add(item);
                }
                IsEndFlipped.Value = true;
                IsEndFlipped.Value = false;
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.ToString());
                throw;
            }
        }

        protected IEnumerable<StatPlayerModel> GenerateStats(IEnumerable<PlayerModel> players)
        {
            foreach (var item in players)
            {
                yield return new StatPlayerModel(new PlayerStatsPair<double>(item.Player, StatCalculators.SelectMany(a => a.Calculate(rally, item.Player))));
            }
        }

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            RefreshPoints();
        }

        private static void DumpError(Action action)
        {
            try
            {
                action?.Invoke();
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.ToString());
                throw;
            }
        }

        private void Match_PropertyChanged(object sender, PropertyChangedEventArgs e) => RefreshPoints();

        private void RefreshPoints()
        {
            IsService.Value = rally.IsService;
            IsInputFlipped.Value = rally.FlipInput ^ IsEndFlipped.Value;
            IsFlipped.Value = rally.FlipInput ^ IsEndFlipped.Value;
            IsPostService.Value = rally.IsAceBall ^ rally.IsService;
            PreviousPlayer.Value = rally.PreviousPlayer;
            CurrentBallTeam.Value = IsEndFlipped.Value ? rally.CurrentBallTeam.Flip() : rally.CurrentBallTeam;
            if (!(match.Winner is null))
            {
                PointCountA.Value = "";
                PointCountB.Value = "";
                GameCountA.Value = "";
                GameCountB.Value = "";
                SetCountA.Value = match.Winner == match.TeamA ? "WON" : "LOST";
                SetCountB.Value = match.Winner == match.TeamB ? "WON" : "LOST";
            }
            else
            {
                PointCountA.Value = Game?.PointA ?? "88";
                PointCountB.Value = Game?.PointB ?? "88";
                GameCountA.Value = Set?.GameCountA.ToString() ?? "88";
                GameCountB.Value = Set?.GameCountB.ToString() ?? "88";
                SetCountA.Value = match.SetCountA.ToString() ?? "88";
                SetCountB.Value = match.SetCountB.ToString() ?? "88";
            }
            StatPlayersItemSource.Value.Clear();
            foreach (var item in GenerateStats(players).ToArray())
            {
                StatPlayersItemSource.Value.Add(item);
            }
        }

        private void Vibrate(double duration = 100)
        {
            DependencyService.Get<IDeviceService>().Vibrate(duration);
        }
    }
}
