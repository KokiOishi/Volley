using System;
using System.Collections.Generic;
using System.Text;
using Volley.Standalone.Players;

namespace Volley.Standalone.Stats
{
    public struct PlayerStatsPair<TValue>
    {
        public PlayerStatsPair(PlayerStandalone player, IEnumerable<IStatItem<TValue>> stats)
        {
            Player = player ?? throw new ArgumentNullException(nameof(player));
            Stats = stats ?? throw new ArgumentNullException(nameof(stats));
        }

        public PlayerStandalone Player { get; }

        public IEnumerable<IStatItem<TValue>> Stats { get; }
    }
}
