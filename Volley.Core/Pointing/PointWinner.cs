using System;
using System.Collections.Generic;
using System.Linq;
using Volley.Players;
using Volley.Team;

namespace Volley.Pointing
{
    /// <summary>
    /// Represents a winner point.
    /// </summary>
    public class PointWinner<TTeam> : Point<TTeam> where TTeam : class, ITeamInMatch
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="T:Volley.Points.PointWinner`1"/> class.
        /// </summary>
        /// <param name="pointKind">Point kind.</param>
        /// <param name="winnerTeam">Winner team.</param>
        /// <param name="shotKind">Shot kind to win the point.</param>
        /// <param name="numberRallies">Number of rallies.</param>
        /// <param name="actualWinner">Actual winner.</param>
        /// <param name="side">Side won the point.</param>
        public PointWinner(PointKinds pointKind, TTeam winnerTeam, ShotKind shotKind, HandSide side, IEnumerable<Receive> receives, Player actualWinner)
             : base(pointKind, winnerTeam, shotKind, side, receives)
        {
            ActualWinner = actualWinner ?? throw new ArgumentNullException(nameof(actualWinner));
            if (!winnerTeam.AllPlayers.Contains(actualWinner))
                throw new ArgumentException($"{nameof(actualWinner)} must be one of the {nameof(winnerTeam)}!", nameof(actualWinner));
        }

        /// <summary>
        /// Gets the actual winner.
        /// </summary>
        /// <value>The actual winner.</value>
        public Player ActualWinner { get; }
    }
}
