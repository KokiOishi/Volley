using System;
using System.Collections.Generic;
using System.Text;
using Volley.Matches;
using Volley.Team;

namespace Volley
{
    /// <summary>
    /// Holds some values for Team A and B.
    /// </summary>
    /// <typeparam name="TValue">The type of holding value.</typeparam>
    public readonly struct TeamedValuePair<TValue> : IEquatable<TeamedValuePair<TValue>>
    {
        /// <summary>
        /// The value for team A.
        /// </summary>
        public readonly TValue ValueA;

        /// <summary>
        /// The value for team B.
        /// </summary>
        public readonly TValue ValueB;

        /// <summary>
        /// TODO: Documentation
        /// </summary>
        /// <param name="valueA"></param>
        /// <param name="valueB"></param>
        public TeamedValuePair(TValue valueA, TValue valueB)
        {
            ValueA = valueA;
            ValueB = valueB;
        }

        /// <summary>
        /// Gets the value for specified <paramref name="team"/>.
        /// </summary>
        /// <param name="team">The <see cref="EnumTeams"/> value to specify the team.</param>
        /// <returns>The value for specified <paramref name="team"/>.</returns>
        public TValue this[EnumTeams team] => team == EnumTeams.TeamA ? ValueA : ValueB;

        /// <summary>
        /// TODO: Documentation
        /// </summary>
        /// <param name="valueA"></param>
        /// <param name="valueB"></param>
        public void Deconstruct(out TValue valueA, out TValue valueB)
        {
            valueA = ValueA;
            valueB = ValueB;
        }

        public override bool Equals(object obj) => obj is TeamedValuePair<TValue> pair && Equals(pair);

        public bool Equals(TeamedValuePair<TValue> other) => EqualityComparer<TValue>.Default.Equals(ValueA, other.ValueA) && EqualityComparer<TValue>.Default.Equals(ValueB, other.ValueB);

        public override int GetHashCode()
        {
            var hashCode = -1421735599;
            hashCode = hashCode * -1521134295 + EqualityComparer<TValue>.Default.GetHashCode(ValueA);
            hashCode = hashCode * -1521134295 + EqualityComparer<TValue>.Default.GetHashCode(ValueB);
            return hashCode;
        }

        public static bool operator ==(TeamedValuePair<TValue> left, TeamedValuePair<TValue> right) => left.Equals(right);

        public static bool operator !=(TeamedValuePair<TValue> left, TeamedValuePair<TValue> right) => !(left == right);
    }
}