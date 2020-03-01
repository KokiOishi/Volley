using System;
using System.Collections.Generic;
using System.Text;
using Volley.Matches;

namespace Volley.Rules
{
    public interface IGameCounter
    {
        /// <summary>
        /// Gets the value which indicates whether the <see cref="Set{TTeam}"/> is over or not.
        /// </summary>
        bool IsSetOver { get; }

        /// <summary>
        /// Gets the game count for A team.
        /// </summary>
        int GameCountA { get; }

        /// <summary>
        /// Gets the game count for B team.
        /// </summary>
        int GameCountB { get; }

        /// <summary>
        /// Gets the point counter for current games.
        /// </summary>
        IPointCounter CurrentPointCounter { get; }

        EnumTeams CurrentServiceRight { get; }

        /// <summary>
        /// Handles a game addition to A team.
        /// </summary>
        void IncrementA();

        /// <summary>
        /// Handles a game addition to B team.
        /// </summary>
        void IncrementB();

        /// <summary>
        /// Handles a game rollbacks.
        /// </summary>
        void Rollback();
    }
}