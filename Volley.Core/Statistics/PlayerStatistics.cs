using System;
using System.Collections.Generic;
using System.Text;
using Volley.Players;
using Volley.Pointing;
using Volley.Team;

namespace Volley.Statistics
{
    /// <summary>
    /// Represents a statistics of a player.
    /// </summary>
    public readonly struct PlayerStatistics
    {
        /// <summary>
        /// Gets the value which represents a statistics of the player's first service.
        /// </summary>
        public ServiceStatistics FirstServiceStatistics { get; }

        /// <summary>
        /// Gets the value which represents a statistics of the player's second service.
        /// </summary>
        public ServiceStatistics SecondServiceStatistics { get; }

        public PointKindStatistics AceStatistics { get; }

        public PointKindStatistics DoubleFaultStatistics { get; }

        public PointKindStatistics WinnerStatistics { get; }

        public PointKindStatistics UnforcedErrorStatistics { get; }
    }
}