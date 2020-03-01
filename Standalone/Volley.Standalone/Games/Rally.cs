using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using Volley.Games;
using Volley.Matches;
using Volley.Players;
using Volley.Pointing;
using Volley.Sets;
using Volley.Standalone.Matches;
using Volley.Standalone.Teams;
using Volley.Team;

namespace Volley.Standalone.Games
{
    /// <summary>
    /// Handles UI's Input and processes the internal things.
    /// </summary>
    /// <typeparam name="TTeam"></typeparam>
    public class Rally<TTeam> : INotifyPropertyChanged where TTeam : class, ITeamInMatch
    {
        private readonly List<Receive> receives = new List<Receive>();

        private readonly List<Receive> serviceFaults = new List<Receive>();

        private EnumTeams currentBallTeam;

        public EnumTeams CurrentBallTeam
        {
            get => currentBallTeam;
            private set => this.SetAndNotifyIfChanged(PropertyChanged, currentBallTeam, currentBallTeam = value, nameof(CurrentBallTeam));
        }

        public TTeam CurrentServiceTeam => Match.CurrentSet.CurrentGame.ServiceTeam;

        public bool FlipInput => CurrentBallTeam != EnumTeams.TeamA;

        public bool IsAceBall => receives.Count - serviceFaults.Count < 2;

        public bool IsService => receives.Count - serviceFaults.Count < 1;

        public MatchStandalone<TTeam> Match { get; }

        public Player PreviousPlayer => receives.LastOrDefault().ReceivedPlayer;

        private TTeam BallTeam => CurrentBallTeam == EnumTeams.TeamA ? Match.TeamA : Match.TeamB;

        private Game<TTeam> Game => Match.CurrentSet.CurrentGame;

        private bool IsPreviousServiceFault => serviceFaults.Count > 0;

        private TTeam PreviousReceiverTeam => BallTeam == Match.TeamA ? Match.TeamB : Match.TeamA;

        private TTeam ReceiverTeam => CurrentServiceTeam == Match.TeamA ? Match.TeamB : Match.TeamA;

        private Set<TTeam> Set => Match.CurrentSet;

        public event PropertyChangedEventHandler PropertyChanged;

        public Rally(MatchStandalone<TTeam> match)
        {
            Match = match ?? throw new ArgumentNullException(nameof(match));
            ResetReceiver();
            Match.PropertyChanged += Match_PropertyChanged;
        }

        public IEnumerable<Point<TTeam>> AllPoints => Match.Sets.SelectMany(a => a.Games).SelectMany(a => a.Points)
            .Concat(Game?.Points ?? Array.Empty<Point<TTeam>>());

        public void Cancel()
        {
            //throw new NotImplementedException();
            if (receives.Count < 1 && serviceFaults.Count < 1)
            {
                if (Game.Points.Count < 1)
                {
                    if (Set.Games.Count < 1)
                    {
                        if (Match.Sets.Count > 0)
                        {
                            var p = Match.CancelPreviousSet();
                            RollbackReceives(p.Item1.Item1);
                        }
                    }
                    else
                    {
                        var p = Set.CancelPreviousGame();
                        RollbackReceives(p.Item1);
                    }
                }
                else
                {
                    var p = Game.CancelRally();
                    RollbackReceives(p);
                    //Match.ServiceRight.OnGameCancelled();
                }
            }
            else
            {
                RollbackReceiver();
                receives.RemoveAt(receives.Count - 1);
                if (receives.Count < serviceFaults.Count) serviceFaults.RemoveAt(serviceFaults.Count - 1);
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Match)));
            }
        }

        public void Let()
        {
            if (IsAceBall && IsPreviousServiceFault)
            {
                ResetReceiver();
                receives.Clear();
                receives.AddRange(serviceFaults);
            }
            else
            {
                ResetReceiver();
                receives.Clear();
                serviceFaults.Clear();
            }
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Match)));
        }

        public void ReceivedError(Player player)
        {
            if (IsAceBall)  //service fault
            {
                OnServiceFault(player);
            }
            else
            {
                OnErroredAfterShot();
            }
        }

        public void ReceivedErroredInforcefully(Player player)
        {
            if (!IsAceBall)
            {
                Receive receive = receives.Last();
                Game.RallyFinished(new PointError<TTeam>(BallTeam, receive.Kind, receive.Side, receives, receive.ReceivedPlayer, false));
                RefreshServiceRight();
            }
            else
            {
                OnServiceFault(player);
            }
        }

        public void ReceivedWinAPoint(Player player)
        {
            if (receives.Count < 1) throw new InvalidOperationException();
            //ace or winner
            Receive Receive = receives.Last();
            var point = new PointWinner<TTeam>(PointKinds.Winner, PreviousReceiverTeam, Receive.Kind, Receive.Side, receives, Receive.ReceivedPlayer);
            Game.RallyFinished(point);
            RefreshServiceRight();
        }

        public void ReceiverError(Player player)
        {
            if (!IsService)
            {
                Receive Receive = receives.Last();
                Game.RallyFinished(new PointWinner<TTeam>(PointKinds.Winner, PreviousReceiverTeam, Receive.Kind, Receive.Side, receives, Receive.ReceivedPlayer));
                RefreshServiceRight();
            }
            else
            {
                OnServiceFault(player);
            }
        }

        public void ReceiverReceive(Receive receive)
        {
            receives.Add(receive);
            FlipReceiver();
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Match)));
        }

        private void FlipReceiver() => CurrentBallTeam = CurrentBallTeam.Flip();

        private void Match_PropertyChanged(object sender, PropertyChangedEventArgs e) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Match)));

        private void OnDoubleFault(Player player)
        {
            Game.RallyFinished(new PointServiceDoubleFault<TTeam>(ReceiverTeam, player, receives));
            RefreshServiceRight();
        }

        private void OnErroredAfterShot()
        {
            Receive lastReceive = receives.Last();
            Game.RallyFinished(new PointError<TTeam>(BallTeam, lastReceive.Kind, lastReceive.Side, receives, lastReceive.ReceivedPlayer, false));
            RefreshServiceRight();
        }

        private void OnFirstServiceFault(Player player)
        {
            serviceFaults.AddRange(receives);
            ResetReceiver();
        }

        private void OnServiceFault(Player player)
        {
            if (receives.Count == 0)
            {
                receives.Add(new Receive(player, HandSide.None, ShotKind.Volley));
            }
            else
            {
                var h = receives.Last();
                receives.RemoveAt(receives.Count - 1);
                h = new Receive(h.ReceivedPlayer, HandSide.None, ShotKind.Volley);
                receives.Add(h);
            }
            if (IsPreviousServiceFault) //double fault
            {
                OnDoubleFault(player);
            }
            else        //first service fault
            {
                OnFirstServiceFault(player);
            }
        }

        private void RefreshServiceRight()
        {
            //Match.ServiceRight.OnGameOver();
            ResetReceiver();
            receives.Clear();
            serviceFaults.Clear();
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Match)));
        }

        private void ResetReceiver() => CurrentBallTeam = Match.CurrentSet?.CurrentGame?.PointCounter?.CurrentServiceRight ?? EnumTeams.TeamA;

        private void RollbackReceiver()
        {
            if (IsService) ResetReceiver();
            else FlipReceiver();
        }

        private void RollbackReceives(IEnumerable<Receive> rr)
        {
            //Match.ServiceRight.OnGameCancelled();
            receives.AddRange(rr);
            serviceFaults.AddRange(rr.Where(a => a.IsServiceFault));
            CurrentBallTeam = Match.TeamA.AllPlayers.Contains(receives.Last().ReceivedPlayer) ^ receives.Last().IsServiceFault ? EnumTeams.TeamB : EnumTeams.TeamA;
        }
    }
}
