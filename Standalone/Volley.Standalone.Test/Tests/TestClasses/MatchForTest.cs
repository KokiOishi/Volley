using System;
using Volley.Matches;
using Volley.Matches.Competitors;
using Volley.Matches.Rules;
using Volley.Rules;
using Volley.Sets;
using Volley.Team;

namespace Volley.Standalone.Test.Tests.TestClasses
{
    public class MatchForTest<TTeam> : Match<TTeam> where TTeam : class, ITeamInMatch
    {
        public MatchForTest(TTeam teamA, TTeam teamB, MatchRule rule, ISetCounter setCounter)
            : base(teamA, teamB, rule, setCounter)
        {
        }

        protected override void OnMatchOver()
        {
            throw new NotImplementedException();
        }

        protected override Set<TTeam> OnMatchStart()
        {
            throw new NotImplementedException();
        }

        protected override void OnSetCanceled(Set<TTeam> set)
        {
            throw new NotImplementedException();
        }

        protected override Set<TTeam> OnSetEnd(Set<TTeam> set)
        {
            throw new NotImplementedException();
        }
    }
}