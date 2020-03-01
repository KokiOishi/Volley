using System;
using System.Collections.Generic;
using System.Text;
using Volley.Matches;
using Volley.Rules;

namespace Volley.Mobile.Forms.ViewModels.Parameters
{
    public readonly struct MatchSetupParameters : IEquatable<MatchSetupParameters>
    {
        /// <summary>
        /// Gets a value indicating whether this <see cref="T:Volley.Matches.Rules.MatchRule"/> is doubles.
        /// </summary>
        /// <value><c>true</c> if doubles; otherwise, <c>false</c>.</value>
        public readonly bool IsDoubles;

        /// <summary>
        /// Gets how many sets required to win.
        /// </summary>
        /// <value>The sets.</value>
        public readonly int SetsPerMatch;

        /// <summary>
        /// Gets the games per set.
        /// </summary>
        /// <value>The games per set.</value>
        public readonly int GamesPerSet;

        public readonly IParametricFactory<ISetCounter, IParametricFactory<IGameCounter, EnumTeams>> SetCounterFactory;

        public MatchSetupParameters(bool isDoubles, int setsPerMatch, int gamesPerSet, IParametricFactory<ISetCounter, IParametricFactory<IGameCounter, EnumTeams>> setCounterFactory)
        {
            IsDoubles = isDoubles;
            SetsPerMatch = setsPerMatch;
            GamesPerSet = gamesPerSet;
            SetCounterFactory = setCounterFactory ?? throw new ArgumentNullException(nameof(setCounterFactory));
        }

        public override bool Equals(object obj) => obj is MatchSetupParameters parameters && Equals(parameters);

        public bool Equals(MatchSetupParameters other) => IsDoubles == other.IsDoubles &&
            SetsPerMatch == other.SetsPerMatch && GamesPerSet == other.GamesPerSet && SetCounterFactory == other.SetCounterFactory;

        public override int GetHashCode()
        {
            var hashCode = 1804465250;
            hashCode = hashCode * -1521134295 + IsDoubles.GetHashCode();
            hashCode = hashCode * -1521134295 + SetsPerMatch.GetHashCode();
            hashCode = hashCode * -1521134295 + GamesPerSet.GetHashCode();
            hashCode = hashCode * -1521134295 + SetCounterFactory.GetHashCode();
            return hashCode;
        }

        public static bool operator ==(MatchSetupParameters left, MatchSetupParameters right) => left.Equals(right);

        public static bool operator !=(MatchSetupParameters left, MatchSetupParameters right) => !(left == right);
    }
}