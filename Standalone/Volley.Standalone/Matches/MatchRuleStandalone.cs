using System;
using Volley.Matches.Rules;

namespace Volley.Standalone.Matches
{
    /// <summary>
    /// Represents and manipulates a Match rule.
    /// </summary>
    public class MatchRuleStandalone : MatchRule
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="T:Volley.Standalone.Matches.MatchRuleStandalone"/> class.
        /// </summary>
        /// <param name="doubles">If set to <c>true</c> doubles.</param>
        /// <param name="deuce">If set to <c>true</c> deuce.</param>
        /// <param name="sets">Sets.</param>
        /// <param name="gamesPerSet">Games per set.</param>
        /// <param name="tieBreakEnabled">If set to <c>true</c> tie break enabled.</param>
        public MatchRuleStandalone(bool doubles, bool deuce, int sets, int gamesPerSet, bool tieBreakEnabled) : base(doubles, deuce, sets, gamesPerSet, tieBreakEnabled)
        {
        }
    }
}
