using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Volley.Matches;
using Volley.Matches.Competitors;
using Volley.Players;
using Volley.Pointing;
using Volley.Rules;
using Volley.Sets;
using Volley.Team;

namespace Volley.Games
{
    /// <summary>
    /// Defines a base infrastructure of the tennis "game".
    /// </summary>
    public abstract class Game<TTeam> where TTeam : class, ITeamInMatch
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="T:Volley.Games.Game`1"/> class.
        /// </summary>
        /// <param name="set">Set.</param>
        protected Game(Set<TTeam> set)
        {
            Set = set ?? throw new ArgumentNullException(nameof(set));
            //ServiceRight = set.ServiceRight;
            PointCounter = set.GameCounter.CurrentPointCounter;
            (PointA, PointB) = (PointCounter.PointA, PointCounter.PointB);
        }

        /// <summary>
        /// Gets the <see cref="Set{TTeam}"/> contains this <see cref="Game{TTeam}"/>.
        /// </summary>
        /// <value>The set.</value>
        public Set<TTeam> Set { get; }

        /// <summary>
        /// Gets the service team.
        /// </summary>
        /// <value>The service team.</value>
        public TTeam ServiceTeam => PointCounter.CurrentServiceRight.Switch(TeamA, TeamB);

        /// <summary>
        /// Gets the Receive team.
        /// </summary>
        /// <value>The Receive team.</value>
        public TTeam ReceiveTeam => PointCounter.CurrentServiceRight.Flip().Switch(TeamA, TeamB);

        /// <summary>
        /// Gets the point counter for both team.
        /// </summary>
        public IPointCounter PointCounter { get; }

        /// <summary>
        /// Gets the team a.
        /// </summary>
        /// <value>The team a.</value>
        public virtual TTeam TeamA => Set.TeamA;

        /// <summary>
        /// Gets the point count of a team.
        /// </summary>
        /// <value>The point count a.</value>
        public abstract string PointA { get; protected set; }

        /// <summary>
        /// Gets the team b.
        /// </summary>
        /// <value>The team b.</value>
        public virtual TTeam TeamB => Set.TeamB;

        /// <summary>
        /// Gets the point count of b team.
        /// </summary>
        /// <value>The point count b.</value>
        public abstract string PointB { get; protected set; }

        /// <summary>
        /// Gets the service right.
        /// </summary>
        /// <value>The service right.</value>
        [Obsolete("Use PointCounter.CurrentServiceRight instead.", true)]
        public virtual ServiceRightManager<TTeam> ServiceRight { get; }

        /// <summary>
        /// Gets the winner team.
        /// </summary>
        /// <value>The winner team.</value>
        public TTeam WinnerTeam { get; private set; }

        /// <summary>
        /// Gets the service player.
        /// </summary>
        /// <value>The service player.</value>
        public Player ServicePlayer => ServiceTeam.CurrentServicePlayer;

        /// <summary>
        /// Gets the list of points.
        /// </summary>
        /// <value>The points.</value>
        public IList<Point<TTeam>> Points { get; } = new List<Point<TTeam>>();

        /// <summary>
        /// Starts the game.
        /// </summary>
        public void StartGame() => OnGameStart();

        /// <summary>
        /// Called when the game is starting.
        /// </summary>
        protected abstract void OnGameStart();

        /// <summary>
        /// Called when the rally is over.
        /// </summary>
        /// <param name="point">Point.</param>
        public void RallyFinished(Point<TTeam> point)
        {
            Points.Add(point);
            if (point.WinnerTeam == TeamA)
            {
                PointCounter.IncrementA();
            }
            else if (point.WinnerTeam == TeamB)
            {
                PointCounter.IncrementB();
            }
            //Allows child to raise INotifyPropertyChanged.PropertyChanged event.
            (PointA, PointB) = (PointCounter.PointA, PointCounter.PointB);
            if (PointCounter.IsGameSet)
            {
                WinnerTeam = point.WinnerTeam;
                OnGameOver();
                Set.CurrentGameFinished();
            }
        }

        /// <summary>
        /// Cancels the prvious rally.
        /// </summary>
        public IEnumerable<Receive> CancelRally()
        {
            if (Points.Count < 1) throw new InvalidOperationException("Cannot rollback from empty state!");
            var point = Points.Last();
            Points.Remove(point);
            PointCounter.Rollback();
            (PointA, PointB) = (PointCounter.PointA, PointCounter.PointB);
            OnRallyCanceled(point);
            return point.Receives;
        }

        /// <summary>
        /// Called when the rally is over.
        /// Return <see langword="true"/> if the game continues.
        /// </summary>
        /// <param name="point">Point.</param>
        protected abstract bool OnRallyFinished(Point<TTeam> point);

        /// <summary>
        /// Called when the rally is canceled.
        /// </summary>
        /// <param name="point">Point.</param>
        protected abstract void OnRallyCanceled(Point<TTeam> point);

        /// <summary>
        /// Called when the game is over.
        /// Move onto the next game.
        /// </summary>
        protected abstract void OnGameOver();
    }
}