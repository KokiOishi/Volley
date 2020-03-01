using System;
using System.Collections.Generic;
using System.Linq;
using Volley.Players;
using Volley.Team;

namespace Volley.Pointing
{
    /// <summary>
    /// Represents an Error point.
    /// </summary>
    public class PointError<TTeam> : Point<TTeam> where TTeam : class, ITeamInMatch
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="T:Volley.Points.PointError`1"/> class.
        /// </summary>
        /// <param name="winnerTeam">Winner team.</param>
        /// <param name="shotKind">Shot kind that <paramref name="winnerTeam"/> won the point.</param>
        /// <param name="numberRallies">Number of rallies.</param>
        /// <param name="erroredPlayer">The player errored.</param>
        /// <param name="forced">The value which indicates whether the <paramref name="erroredPlayer"/> is forced to error.</param>
        public PointError(TTeam winnerTeam, ShotKind shotKind, HandSide side, IEnumerable<Receive> receives, Player erroredPlayer, bool forced)
             : base(forced ? PointKinds.ForcedError : PointKinds.UnforcedError, winnerTeam, shotKind, side, receives)
        {
            ErroredPlayer = erroredPlayer ?? throw new ArgumentNullException(nameof(erroredPlayer));
            if (winnerTeam.AllPlayers.Contains(erroredPlayer))
                throw new ArgumentException($"{nameof(erroredPlayer)} must not be one of the {nameof(winnerTeam)}!", nameof(erroredPlayer));
            Forced = forced;
        }

        /// <summary>
        /// Gets the errored player.
        /// </summary>
        /// <value>The errored player.</value>
        public Player ErroredPlayer { get; }

        /// <summary>
        /// Gets a value indicating whether this <see cref="T:Volley.Points.PointError`1"/> is forced error.
        /// </summary>
        /// <value><c>true</c> if forced; otherwise, <c>false</c>.</value>
        public bool Forced { get; }
    }
}
