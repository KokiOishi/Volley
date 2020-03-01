using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Volley.Players;
using Volley.Team;

namespace Volley.Standalone.Teams
{
    /// <summary>
    /// The GUI-Friendly Team object for Xamarin.Forms
    /// </summary>
    public sealed class GenericTeam : ITeamInMatch
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="players"></param>
        /// <param name="identifier"></param>
        /// <param name="name"></param>
        public GenericTeam(IReadOnlyList<Player> players, Player currentServicePlayer, Guid identifier, string name)
        {
            Players = players ?? throw new ArgumentNullException(nameof(players));
            CurrentServicePlayer = currentServicePlayer ?? throw new ArgumentNullException(nameof(currentServicePlayer));
            Identifier = identifier;
            Name = name ?? throw new ArgumentNullException(nameof(name));
        }

        private IReadOnlyList<Player> Players { get; }

        public Player this[int index] => Players[index];

        public Player CurrentServicePlayer { get; }
        public Guid Identifier { get; }
        public string Name { get; }
        public int PlayerCount => Players.Count;
        public IEnumerable<Player> AllPlayers => Players.Select(a => a);

        public void OnGameCancelled()
        {
        }

        public void OnGameOver()
        {
        }
    }
}