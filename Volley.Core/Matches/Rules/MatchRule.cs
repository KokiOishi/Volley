using System;
using Volley;

namespace Volley.Matches.Rules
{
    /// <summary>
    /// Represents and manipulates a Match rule.
    /// </summary>
    public abstract class MatchRule
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="T:Volley.Matches.Rules.MatchRule"/> class.
        /// </summary>
        /// <param name="doubles">If set to <c>true</c> the match is a doubles match.</param>
        /// <param name="deuce">If set to <c>true</c> the deuce is enabled in the match.</param>
        /// <param name="sets">Sets required to win the match.</param>
        /// <param name="gamesPerSet">Games required to win the set.</param>
        /// <param name="tieBreakEnabled">If set to <c>true</c> the tie break enabled in the match.</param>
        protected MatchRule(bool doubles, bool deuce, int sets, int gamesPerSet, bool tieBreakEnabled)
        {
            Doubles = doubles;
            Deuce = deuce;
            Sets = sets.GraterThan(nameof(sets), 0);
            GamesPerSet = gamesPerSet.GraterThan(nameof(gamesPerSet), 0);
            TieBreakEnabled = tieBreakEnabled;
        }

        /// <summary>
        /// Gets a value indicating whether this <see cref="T:Volley.Matches.Rules.MatchRule"/> is doubles.
        /// </summary>
        /// <value><c>true</c> if doubles; otherwise, <c>false</c>.</value>
        public bool Doubles { get; }

        /// <summary>
        /// Gets a value indicating whether this <see cref="T:Volley.Matches.Rules.MatchRule"/> has a deuce.
        /// </summary>
        /// <value><c>true</c> if deuce; otherwise, <c>false</c>.</value>
        public bool Deuce { get; }

        /// <summary>
        /// Gets how many sets required to win.
        /// </summary>
        /// <value>The sets.</value>
        public int Sets { get; }

        /// <summary>
        /// Gets the games per set.
        /// </summary>
        /// <value>The games per set.</value>
        public int GamesPerSet { get; }

        /// <summary>
        /// Gets a value indicating whether this <see cref="T:Volley.Matches.Rules.MatchRule"/>'s tie break enabled.
        /// </summary>
        /// <value><c>true</c> if tie break enabled; otherwise, <c>false</c>.</value>
        public bool TieBreakEnabled { get; }

        /// <summary>
        /// Gets a value indicating whether this <see cref="T:Volley.Matches.Rules.MatchRule"/> has enabled challenge system.
        /// </summary>
        /// <value><c>true</c> if is challenge enabled; otherwise, <c>false</c>.</value>
        public bool IsChallengeEnabled { get; }
    }
}