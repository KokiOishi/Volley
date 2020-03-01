using System.Collections.Generic;
using Volley.Games;
using Volley.Matches.Competitors;
using Volley.Matches.Rules;
using Volley.Sets;
using Volley.Team;

namespace Volley.Matches
{
    public interface IMatch<TTeam> where TTeam : class, ITeamInMatch
    {
        Set<TTeam> CurrentSet { get; }
        MatchRule Rule { get; }

        //ServiceRightManager<TTeam> ServiceRight { get; }
        int SetCountA { get; }

        int SetCountB { get; }
        IList<Set<TTeam>> Sets { get; }
        TTeam TeamA { get; }
        TTeam TeamB { get; }
        TTeam Winner { get; }

        ((IEnumerable<Receive>, Game<TTeam>), Set<TTeam>) CancelPreviousSet();

        void CurrentSetFinished();

        void StartMatch();
    }
}