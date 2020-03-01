using System;
using System.Collections.Generic;
using System.Text;
using Volley.Matches;

namespace Volley.Rules
{
    public interface ISetCounter
    {
        /// <summary>
        /// Gets the value which indicates whether the <see cref="Match{TTeam}"/> is over or not.
        /// </summary>
        bool IsMatchOver { get; }

        /// <summary>
        /// Gets the set count for A team.
        /// </summary>
        int SetCountA { get; }

        /// <summary>
        /// Gets the set count for B team.
        /// </summary>
        int SetCountB { get; }

        EnumTeams CurrentServiceRight { get; }

        /// <summary>
        /// Gets the game counter for current set.
        /// </summary>
        IGameCounter CurrentGameCounter { get; }

        /// <summary>
        /// Handles a set addition to A team.
        /// </summary>
        void IncrementA();

        /// <summary>
        /// Handles a set addition to B team.
        /// </summary>
        void IncrementB();

        /// <summary>
        /// Handles a set rollbacks.
        /// </summary>
        void Rollback();
    }
}