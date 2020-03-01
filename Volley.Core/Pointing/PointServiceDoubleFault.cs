using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Volley.Players;
using Volley.Team;

namespace Volley.Pointing
{
    /// <summary>
    ///
    /// </summary>
    /// <typeparam name="TTeam"></typeparam>
    public class PointServiceDoubleFault<TTeam> : Point<TTeam> where TTeam : class, ITeamInMatch
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="T:Volley.Points.PointError`1"/> class.
        /// </summary>
        /// <param name="winnerTeam">Winner team.</param>
        /// <param name="erroredPlayer">The player errored.</param>
        public PointServiceDoubleFault(TTeam winnerTeam, Player erroredPlayer, IEnumerable<Receive> receives)
             : base(PointKinds.Winner, winnerTeam, ShotKind.Volley, HandSide.Fore, receives)
        {
            ErroredPlayer = erroredPlayer ?? throw new ArgumentNullException(nameof(erroredPlayer));
            if (winnerTeam.AllPlayers.Contains(erroredPlayer))
                throw new ArgumentException($"{nameof(erroredPlayer)} must not be one of the {nameof(winnerTeam)}!", nameof(erroredPlayer));
        }

        /// <summary>
        /// Gets the errored player.
        /// </summary>
        /// <value>The errored player.</value>
        public Player ErroredPlayer { get; }
    }
}
