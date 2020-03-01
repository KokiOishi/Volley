using System;
using System.Collections.Generic;
using System.Text;

namespace Volley.Statistics
{
    /// <summary>
    /// Represents a statistics of the point kinds.
    /// </summary>
    public readonly struct PointKindStatistics
    {
        public PointKindStatistics(double forehandRatio, double backhandRatio, double strokeRatio, double volleyRatio)
        {
            ForehandRatio = forehandRatio;
            BackhandRatio = backhandRatio;
            StrokeRatio = strokeRatio;
            VolleyRatio = volleyRatio;
        }

        /// <summary>
        /// Gets the value which indicates how often the player uses forehand in %.
        /// </summary>
        public double ForehandRatio { get; }

        /// <summary>
        /// Gets the value which indicates how often the player uses backhand in %.
        /// </summary>
        public double BackhandRatio { get; }

        /// <summary>
        /// Gets the value which indicates how often the player strokes in %.
        /// </summary>
        public double StrokeRatio { get; }

        /// <summary>
        /// Gets the value which indicates how often the player volleies in %.
        /// </summary>
        public double VolleyRatio { get; }
    }
}