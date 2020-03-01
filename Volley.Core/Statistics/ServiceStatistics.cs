using System;
using System.Collections.Generic;
using System.Text;
using Volley.Players;

namespace Volley.Statistics
{
    /// <summary>
    /// Represents a statistics of the player's service.
    /// </summary>
    public struct ServiceStatistics
    {
        /// <summary>
        /// Gets the value which indicates how often the player succeeds the specified service.
        /// </summary>
        public double SuccessRate { get; }

        /// <summary>
        /// Gets the value which indicates how often the player wins points with the specified service.
        /// </summary>
        public double PointRate { get; }

        /// <summary>
        /// Gets the value which indicates how often the player uses forehand in %.
        /// </summary>
        public double ForehandRatio { get; }

        /// <summary>
        /// Gets the value which indicates how often the player uses backhand in %.
        /// </summary>
        public double BackhandRatio { get; }
    }
}