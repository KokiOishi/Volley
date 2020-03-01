using System;
using System.Collections.Generic;
using System.Text;
using Volley.Matches;
using Volley.Players;

namespace Volley.Team
{
    /// <summary>
    /// Defines a base infrastructure of the teams in <see cref="Match{TTeam}"/>.
    /// </summary>
    public interface ITeamInMatch : ITeam
    {
        /// <summary>
        /// Gets the current service player.
        /// </summary>
        /// <value>The current service player.</value>
        Player CurrentServicePlayer { get; }
        /// <summary>
        /// Called when the game is over.
        /// </summary>
        void OnGameOver();
        /// <summary>
        /// Called when the game is cancelled under recorrection of internal data.
        /// </summary>
        void OnGameCancelled();
    }
}
