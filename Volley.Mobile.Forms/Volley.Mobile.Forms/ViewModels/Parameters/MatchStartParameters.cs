using System;
using System.Collections.Generic;
using Volley.Standalone.Matches;
using Volley.Standalone.Teams;

namespace Volley.Mobile.Forms.ViewModels.Parameters
{
    public readonly struct MatchStartParameters : IEquatable<MatchStartParameters>
    {
        public readonly MatchStandalone<GenericTeam> Match;

        public MatchStartParameters(MatchStandalone<GenericTeam> match)
        {
            Match = match ?? throw new ArgumentNullException(nameof(match));
        }

        public override bool Equals(object obj) => obj is MatchStartParameters parameters && Equals(parameters);

        public bool Equals(MatchStartParameters other) => EqualityComparer<MatchStandalone<GenericTeam>>.Default.Equals(Match, other.Match);

        public override int GetHashCode() => -1831319130 + EqualityComparer<MatchStandalone<GenericTeam>>.Default.GetHashCode(Match);

        public static bool operator ==(MatchStartParameters left, MatchStartParameters right) => left.Equals(right);

        public static bool operator !=(MatchStartParameters left, MatchStartParameters right) => !(left == right);
    }
}