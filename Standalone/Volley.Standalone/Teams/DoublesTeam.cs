using System;
using System.Collections.Generic;
using System.Text;
using Volley.Players;
using Volley.Team;

namespace Volley.Standalone.Teams
{
    public class DoublesTeam : ITeamInMatch
    {
        public DoublesTeam(Player playerA, Player playerB, Player currentServicePlayer, Guid identifier, string name)
        {
            PlayerA = playerA ?? throw new ArgumentNullException(nameof(playerA));
            PlayerB = playerB ?? throw new ArgumentNullException(nameof(playerB));
            CurrentServicePlayer = currentServicePlayer ?? throw new ArgumentNullException(nameof(currentServicePlayer));
            Identifier = identifier;
            Name = name ?? throw new ArgumentNullException(nameof(name));
        }

        public Player PlayerA { get; }
        public Player PlayerB { get; }

        public Player this[int index]
        {
            get
            {
                switch (index)
                {
                    case 0:
                        return PlayerA;

                    case 1:
                        return PlayerB;

                    default:
                        return null;
                }
            }
        }

        public Player CurrentServicePlayer { get; }
        public Guid Identifier { get; }
        public string Name { get; }
        public int PlayerCount => 2;

        public IEnumerable<Player> AllPlayers
        {
            get
            {
                yield return PlayerA;
                yield return PlayerB;
            }
        }

        public void OnGameCancelled()
        {
            //
        }

        public void OnGameOver()
        {
            //
        }
    }
}