using System;
using System.ComponentModel;
using Volley.Games;
using Volley.Matches;
using Volley.Sets;
using Volley.Standalone.Games;
using Volley.Team;

namespace Volley.Standalone.Sets
{
    public class SetStandalone<TTeam> : Set<TTeam>, INotifyPropertyChanged where TTeam : class, ITeamInMatch
    {
        //public Func<Set<TTeam>, Game<TTeam>> CreateNewGame { get; }

        public SetStandalone(Match<TTeam> match/*, Func<Set<TTeam>, Game<TTeam>> createNewGameCallback = null*/) : base(match)
        {
            //CreateNewGame = createNewGameCallback ?? OnCreateNewGame;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private int gameCountA, gameCountB;

        public override int GameCountA
        {
            get => gameCountA;
            protected set => this.SetAndNotifyIfChanged(PropertyChanged, gameCountA, gameCountA = value, nameof(GameCountA));
        }

        public override int GameCountB
        {
            get => gameCountB;
            protected set => this.SetAndNotifyIfChanged(PropertyChanged, gameCountB, gameCountB = value, nameof(GameCountB));
        }

        protected override Game<TTeam> OnCurrentGameFinished(Game<TTeam> game)
        {
            if (game is INotifyPropertyChanged notifyPropertyChanged) notifyPropertyChanged.PropertyChanged -= CurrentGame_PropertyChanged;
            //TODO: Dependency Injection
            if (GameCounter.IsSetOver) return null;

            var g = OnCreateNewGame(this);
            if (g is INotifyPropertyChanged npc) npc.PropertyChanged += CurrentGame_PropertyChanged;
            return g;
        }

        private void CurrentGame_PropertyChanged(object sender, PropertyChangedEventArgs e) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(CurrentGame)));

        private static Game<TTeam> OnCreateNewGame(Set<TTeam> set)
        {
            var g = new GameStandalone<TTeam>(set);
            g.StartGame();
            return g;
        }

        protected override void OnPreviousGameCanceled(Game<TTeam> game)
        {
            // Do nothing
        }

        protected override void OnSetOver()
        {
            // Do nothing
        }

        protected override Game<TTeam> OnSetStart()
        {
            return OnCreateNewGame(this);
        }
    }
}