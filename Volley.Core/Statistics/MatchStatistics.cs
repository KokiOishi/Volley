using System;
using System.Collections.Generic;
using System.Text;
using Volley.Matches;
using Volley.Team;

namespace Volley.Statistics
{
    /// <summary>
    /// Represents some statistics of play data.
    /// </summary>
    public class MatchStatistics
    {
        /// <summary>
        /// Calculates some statistics from play data.
        /// </summary>
        /// <typeparam name="TTeam">Type of Team.</typeparam>
        /// <param name="match"></param>
        /// <returns></returns>
        public static MatchStatistics CalculateMatchStatistics<TTeam>(Match<TTeam> match) where TTeam : class, ITeamInMatch
        {
            throw new NotImplementedException();
        }


    }
}
