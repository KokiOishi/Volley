using System;
using System.Collections.Generic;
using System.Text;
using Volley.Matches;

namespace Volley.Rules
{
    public sealed class MatchTieBreakFinalSetGameCounter : CancellableGameCounterBase
    {
        public MatchTieBreakFinalSetGameCounter(IPointCounter initialPointCounter, EnumTeams initialServiceRight) : base(initialPointCounter, initialServiceRight)
        {
        }

        //The GameCount* represents a game point with negated(two's complement of actual) point e.g.-6 => won a game with 6 games.
        //The (GameCountA | GameCountB) will always be negative if one of them is negative(=winner).
        public override bool IsSetOver => (GameCountA | GameCountB) < 0;

        protected override GameCounterHistoryItem HandleIncrementA()
            => new GameCounterHistoryItem(null, new TeamedValuePair<int>(-1, 0), CurrentServiceRight);

        protected override GameCounterHistoryItem HandleIncrementB()
            => new GameCounterHistoryItem(null, new TeamedValuePair<int>(0, -1), CurrentServiceRight);

        public IParametricFactory<MatchTieBreakFinalSetGameCounter, EnumTeams> CreateFactory(IPointCounter initialPointCounter) =>
            Extensions.CreateParametricFactory<MatchTieBreakFinalSetGameCounter, EnumTeams>(isr => new MatchTieBreakFinalSetGameCounter(initialPointCounter, isr));
    }
}