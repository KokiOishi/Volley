using System;
using Volley.Games;
using Volley.Pointing;
using Volley.Sets;
using Volley.Team;

namespace Volley.Standalone.Test.Tests.TestClasses
{
    public class GameForTest<TTeam> : Game<TTeam> where TTeam : class, ITeamInMatch, IDisposable
    {
        public override string PointA { get; protected set; }
        public override string PointB { get; protected set; }

        public event Action<GameForTest<TTeam>> GameOver;

        public event Action<GameForTest<TTeam>> GameStart;

        public event Action<GameForTest<TTeam>, Point<TTeam>> RallyCanceled;

        public event Func<GameForTest<TTeam>, Point<TTeam>, bool> EventRallyFinished;

        public GameForTest(Set<TTeam> set) : base(set)
        {
        }

        protected override void OnGameOver()
        {
            GameOver?.Invoke(this);
        }

        protected override void OnGameStart()
        {
            GameStart?.Invoke(this);
        }

        protected override void OnRallyCanceled(Point<TTeam> point)
        {
            RallyCanceled?.Invoke(this, point);
        }

        protected override bool OnRallyFinished(Point<TTeam> point)
        {
            return EventRallyFinished?.Invoke(this, point) ?? false;
        }
    }
}