using System;
using System.Collections.Generic;
using Volley.Players;
using Volley.Team;

namespace Volley.Standalone.Teams
{
    /// <summary>
    /// Singles team.
    /// </summary>
    public class SinglesTeam : ITeamInMatch
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="T:Volley.Standalone.Teams.SinglesTeam"/> class.
        /// </summary>
        /// <param name="player">Player.</param>
        /// <param name="identifier">Identifier.</param>
        public SinglesTeam(Player player, Guid identifier)
        {
            Player = player ?? throw new ArgumentNullException(nameof(player));
            Identifier = identifier;
        }

        public Player Player { get; }

        /// <summary>
        /// Gets the <see cref="T:Volley.Standalone.Teams.SinglesTeam"/> at the specified index.
        /// </summary>
        /// <param name="index">Index.</param>
        public Player this[int index] => index == 0 ? Player : null;

        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>The identifier.</value>
        public Guid Identifier { get; set; }

        /// <summary>
        /// Gets the name.
        /// </summary>
        /// <value>The name.</value>
        public string Name => Player.Name;

        /// <summary>
        /// Gets all players.
        /// </summary>
        /// <value>All players.</value>
        public IEnumerable<Player> AllPlayers { get { yield return Player; } }

        /// <summary>
        /// Gets the player count.
        /// </summary>
        /// <value>The player count.</value>
        public int PlayerCount => 1;

        public Player CurrentServicePlayer => Player;

        public void OnGameOver()
        {
            //DOES NOTHING BECAUSE THIS CLASS IS SINGLES ONLY
        }

        public void OnGameCancelled()
        {
            //DOES NOTHING BECAUSE THIS CLASS IS SINGLES ONLY
        }
    }
}