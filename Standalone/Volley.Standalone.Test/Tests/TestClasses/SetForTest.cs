using System;
using Volley.Games;
using Volley.Matches;
using Volley.Sets;
using Volley.Team;

namespace Volley.Standalone.Test.Tests.TestClasses
{
    public class SetForTest<TTeam> : Set<TTeam> where TTeam : class, ITeamInMatch
    {
        public override int GameCountA { get; protected set; }
        public override int GameCountB { get; protected set; }

        public event Func<SetForTest<TTeam>, Game<TTeam>, Game<TTeam>> EventCurrentGameFinished;

        public event Action<SetForTest<TTeam>, Game<TTeam>> PreviousGameCanceled;

        public event Action<SetForTest<TTeam>> SetOver;

        public event Func<SetForTest<TTeam>, Game<TTeam>> SetStart;

        public SetForTest(Match<TTeam> match) : base(match)
        {
        }

        protected override Game<TTeam> OnCurrentGameFinished(Game<TTeam> game) => EventCurrentGameFinished?.Invoke(this, game) ?? null;

        protected override void OnPreviousGameCanceled(Game<TTeam> game) => PreviousGameCanceled?.Invoke(this, game);

        protected override void OnSetOver() => SetOver?.Invoke(this);

        protected override Game<TTeam> OnSetStart() => SetStart?.Invoke(this) ?? null;
    }
}