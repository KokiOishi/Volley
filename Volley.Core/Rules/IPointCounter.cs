using System;
using System.Collections.Generic;
using System.Text;
using Volley.Games;
using Volley.Matches;

namespace Volley.Rules
{
    /// <summary>
    /// Defines a base infrastructure that counts several points.
    /// </summary>
    public interface IPointCounter
    {
        /// <summary>
        /// Gets the point for A team.
        /// </summary>
        string PointA { get; }

        /// <summary>
        /// Gets the point for B team.
        /// </summary>
        string PointB { get; }

        /// <summary>
        /// Gets the value which indicates whether the <see cref="Game{TTeam}"/> is set or not.
        /// </summary>
        bool IsGameSet { get; }

        EnumTeams CurrentServiceRight { get; }

        /// <summary>
        /// Handles a point addition to A team.
        /// </summary>
        void IncrementA();

        /// <summary>
        /// Handles a point addition to B team.
        /// </summary>
        void IncrementB();

        /// <summary>
        /// Handles a point deprivation from A team.
        /// </summary>
        void DecrementA();

        /// <summary>
        /// Handles a point deprivation from B team.
        /// </summary>
        void DecrementB();

        /// <summary>
        /// Handles a point rollbacks.
        /// </summary>
        void Rollback();
    }
}