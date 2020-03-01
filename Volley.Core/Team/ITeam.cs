using System;
using System.Collections.Generic;
using System.Text;
using Volley.Players;

namespace Volley.Team
{
    /// <summary>
    /// Defines a base infrastructure of a team.
    /// </summary>
    public interface ITeam
    {
        /// <summary>
        /// Gets the identifier of the team.
        /// </summary>
        Guid Identifier { get; }

        /// <summary>
        /// Gets the name of the team.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Gets a player of the team by <paramref name="index"/>.
        /// </summary>
        /// <param name="index">The player index.</param>
        /// <returns></returns>
        Player this[int index] { get; }

        /// <summary>
        /// Gets the number of players.
        /// </summary>
        /// <value>The player count.</value>
        int PlayerCount { get; }

        /// <summary>
        /// Gets the whole list of players in the team.
        /// </summary>
        IEnumerable<Player> AllPlayers { get; }
    }
}
