using System;
using System.ComponentModel;
using Volley.Games;
using Volley.Pointing;
using Volley.Sets;
using Volley.Team;

namespace Volley.Standalone.Games
{
    public class GameStandalone<TTeam> : Game<TTeam>, INotifyPropertyChanged where TTeam : class, ITeamInMatch
    {
        public GameStandalone(Set<TTeam> set) : base(set)
        {
        }

        /// <summary>
        ///
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        private string pointA, pointB;

        public override string PointA
        {
            get => pointA;
            protected set => this.SetAndNotifyIfChanged(PropertyChanged, pointA, pointA = value, nameof(PointA));
        }

        public override string PointB
        {
            get => pointB;
            protected set => this.SetAndNotifyIfChanged(PropertyChanged, pointB, pointB = value, nameof(PointB));
        }

        /// <summary>
        /// Called when the game is over.
        /// Move onto the next game.
        /// </summary>
        protected override void OnGameOver()
        {
        }

        /// <summary>
        /// Called when the game is starting.
        /// </summary>
        protected override void OnGameStart()
        {
        }

        /// <summary>
        /// Called when the rally is canceled.
        /// </summary>
        /// <param name="point">Point.</param>
        protected override void OnRallyCanceled(Point<TTeam> point)
        {
        }

        /// <summary>
        /// Called when the rally is over.
        /// Return <see langword="true"/> if the game continues.
        /// </summary>
        /// <param name="point">Point.</param>
        protected override bool OnRallyFinished(Point<TTeam> point) => !PointCounter.IsGameSet;
    }
}