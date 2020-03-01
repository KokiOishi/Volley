using System;
using System.Collections.Generic;
using System.Text;
using Volley.Matches;
using Volley.Team;

namespace Volley.Standalone.Matches
{
    public sealed class Ends<TTeam> where TTeam : class, ITeamInMatch
    {
        public EnumTeams LeftTeam { get; }
        public EnumTeams RightTeam => LeftTeam == EnumTeams.TeamA ? EnumTeams.TeamB : EnumTeams.TeamA;
    }
}