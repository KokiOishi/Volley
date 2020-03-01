using System;
using System.Collections.Generic;
using Volley.Pointing;
using Volley.Team;

namespace Volley.UI.ViewModels
{
    public readonly struct TeamScores : IEquatable<TeamScores>
    {
        public TeamScores(ITeamInMatch team, int setCount, int gameCount, PointNumber points)
        {
            Team = team;// ?? throw new ArgumentNullException(nameof(team));
            SetCount = setCount;
            GameCount = gameCount;
            Points = points;
        }

        public ITeamInMatch Team { get; }
        public int SetCount { get; }
        public int GameCount { get; }
        public PointNumber Points { get; }

        public override bool Equals(object obj) => obj is TeamScores scores && Equals(scores);
        public bool Equals(TeamScores other) => EqualityComparer<ITeamInMatch>.Default.Equals(Team, other.Team) && SetCount == other.SetCount && GameCount == other.GameCount && Points == other.Points;

        public override int GetHashCode()
        {
            int hashCode = 2062186786;
            hashCode = (hashCode * -1521134295) + EqualityComparer<ITeamInMatch>.Default.GetHashCode(Team);
            hashCode = (hashCode * -1521134295) + SetCount.GetHashCode();
            hashCode = (hashCode * -1521134295) + GameCount.GetHashCode();
            hashCode = (hashCode * -1521134295) + Points.GetHashCode();
            return hashCode;
        }

        public static bool operator ==(TeamScores left, TeamScores right) => left.Equals(right);
        public static bool operator !=(TeamScores left, TeamScores right) => !(left == right);
    }
}
