using System;
using System.Collections.Generic;
using System.Text;

namespace Volley.Standalone.Matches.Rules
{
    public class MatchRulePrototype
    {
        /// <summary>
        /// Gets a value indicating whether this <see cref="T:Volley.Matches.Rules.MatchRule"/> is doubles.
        /// </summary>
        /// <value><c>true</c> if doubles; otherwise, <c>false</c>.</value>
        public bool Doubles { get; set; }

        /// <summary>
        /// Gets a value indicating whether this <see cref="T:Volley.Matches.Rules.MatchRule"/> has a deuce.
        /// </summary>
        /// <value><c>true</c> if deuce; otherwise, <c>false</c>.</value>
        public bool Deuce { get; set; }

        /// <summary>
        /// Gets how many sets required to win.
        /// </summary>
        /// <value>The sets.</value>
        public int Sets { get; set; }

        /// <summary>
        /// Gets the games per set.
        /// </summary>
        /// <value>The games per set.</value>
        public int GamesPerSet { get; set; }

        /// <summary>
        /// Gets a value indicating whether this <see cref="T:Volley.Matches.Rules.MatchRule"/>'s tie break enabled.
        /// </summary>
        /// <value><c>true</c> if tie break enabled; otherwise, <c>false</c>.</value>
        public bool TieBreakEnabled { get; set; }

        /// <summary>
        /// Gets a value indicating whether this <see cref="T:Volley.Matches.Rules.MatchRule"/> has enabled challenge system.
        /// </summary>
        /// <value><c>true</c> if is challenge enabled; otherwise, <c>false</c>.</value>
        public bool IsChallengeEnabled { get; set; }

        public MatchRuleStandalone Built => new MatchRuleStandalone(Doubles, Deuce, Sets, GamesPerSet, TieBreakEnabled);
    }
}
