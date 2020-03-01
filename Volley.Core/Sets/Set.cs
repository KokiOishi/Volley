using System;
using System.Collections.Generic;
using System.Linq;
using Volley.Games;
using Volley.Matches;
using Volley.Matches.Competitors;
using Volley.Rules;
using Volley.Team;

namespace Volley.Sets
{
    /// <summary>
    /// Represents the tennis "set".
    /// </summary>
    public abstract class Set<TTeam> where TTeam : class, ITeamInMatch
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="T:Volley.Sets.Set`1"/> class.
        /// </summary>
        /// <param name="match">Match.</param>
        protected Set(Match<TTeam> match)
        {
            Match = match ?? throw new ArgumentNullException(nameof(match));
            GameCounter = match.SetCounter.CurrentGameCounter;
            //ServiceRight = match.ServiceRight;
        }

        /// <summary>
        /// Gets the match.
        /// </summary>
        /// <value>The match.</value>
        public Match<TTeam> Match { get; }

        protected internal IGameCounter GameCounter { get; }

        /// <summary>
        /// Gets the team a.
        /// </summary>
        /// <value>The team a.</value>
        public TTeam TeamA => Match.TeamA;

        /// <summary>
        /// Gets the game count of a team.
        /// </summary>
        /// <value>The game count a.</value>
        public abstract int GameCountA { get; protected set; }

        /// <summary>
        /// Gets the team b.
        /// </summary>
        /// <value>The team b.</value>
        public TTeam TeamB => Match.TeamB;

        /// <summary>
        /// Gets the game count of b team.
        /// </summary>
        /// <value>The game count b.</value>
        public abstract int GameCountB { get; protected set; }

        /// <summary>
        /// Gets the service right.
        /// </summary>
        /// <value>The service right.</value>
       // public ServiceRightManager<TTeam> ServiceRight { get; }

        /// <summary>
        /// Gets the set's winner.
        /// </summary>
        /// <value>The set's winner.</value>
        public TTeam Winner { get; private set; }

        /// <summary>
        /// Gets the games.
        /// </summary>
        /// <value>The games.</value>
        public IList<Game<TTeam>> Games { get; } = new List<Game<TTeam>>();

        /// <summary>
        /// Gets the current game.
        /// </summary>
        /// <value>The current game.</value>
        public Game<TTeam> CurrentGame { get; private set; }

        /// <summary>
        /// Starts the set.
        /// </summary>
        public void StartSet()
        {
            CurrentGame = OnSetStart();
        }

        /// <summary>
        /// Called when the set is starting.
        /// </summary>
        /// <returns>The set start.</returns>
        protected abstract Game<TTeam> OnSetStart();

        /// <summary>
        /// Called when the <see cref="CurrentGame"/> is over.
        /// </summary>
        public void CurrentGameFinished()
        {
            Game<TTeam> endingGame = CurrentGame;
            Games.Add(endingGame);
            if (endingGame.WinnerTeam == TeamA) GameCounter.IncrementA();
            else if (endingGame.WinnerTeam == TeamB) GameCounter.IncrementB();
            (GameCountA, GameCountB) = (GameCounter.GameCountA, GameCounter.GameCountB);
            //ServiceRight.OnGameOver();
            if (GameCounter.IsSetOver)
            {
                _ = OnCurrentGameFinished(endingGame);
                if (GameCountA == GameCountB) throw new InvalidProgramException("Cannot win the set while in tie!");
                Winner = GameCountA < 0 ? TeamA : TeamB;
                OnSetOver();
                Match.CurrentSetFinished();
            }
            else
            {
                CurrentGame = OnCurrentGameFinished(endingGame);
            }
        }

        /// <summary>
        /// Cancels the previous game.
        /// Used when the previous set's information was incorrect.
        /// </summary>
        public (IEnumerable<Receive>, Game<TTeam>) CancelPreviousGame()
        {
            if (Games.Count < 1) throw new InvalidOperationException("Cannot rollback from empty state!");
            var g = CurrentGame = Games.Last();
            Games.Remove(g);
            GameCounter.Rollback();
            (GameCountA, GameCountB) = (GameCounter.GameCountA, GameCounter.GameCountB);
            //ServiceRight.OnGameCancelled();
            OnPreviousGameCanceled(g);
            return (g.CancelRally(), g);
        }

        /// <summary>
        /// Called when the <see cref="CurrentGame"/> is over.
        /// Prepare and return the next game.
        /// </summary>
        /// <returns>The current game finished.</returns>
        /// <param name="game">Game.</param>
        protected abstract Game<TTeam> OnCurrentGameFinished(Game<TTeam> game);

        /// <summary>
        /// Called when the previous game is cancelled.
        /// </summary>
        /// <param name="game">Game.</param>
        protected abstract void OnPreviousGameCanceled(Game<TTeam> game);

        /// <summary>
        /// Called when the set is over.
        /// Move onto the next set.
        /// </summary>
        protected abstract void OnSetOver();
    }
}