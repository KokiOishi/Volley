using System;
using System.Collections.Generic;
using System.Text;
using Volley.Matches;

namespace Volley.Rules
{
    public sealed class MatchTieBreakSetCounter : CancellableSetCounterBase
    {
        public MatchTieBreakSetCounter(IParametricFactory<IGameCounter, EnumTeams> gameCounterFactory, int totalSets, int matchTieBreakPointCount,
            int matchTieBreakPointLeastDifference, EnumTeams initialServiceRight) : base(gameCounterFactory.Create(initialServiceRight) ??
                throw new ArgumentNullException(nameof(gameCounterFactory)), initialServiceRight)
        {
            GameCounterFactory = gameCounterFactory ?? throw new ArgumentNullException(nameof(gameCounterFactory));
            TotalSets = totalSets;
            MatchTieBreakPointCount = matchTieBreakPointCount;
            MatchTieBreakPointLeastDifference = matchTieBreakPointLeastDifference;
        }

        public override bool IsMatchOver => (SetCountA | SetCountB) < 0;

        public IParametricFactory<IGameCounter, EnumTeams> GameCounterFactory { get; }

        public int TotalSets { get; }

        public int MatchTieBreakPointCount { get; }

        public int MatchTieBreakPointLeastDifference { get; }

        protected override SetCounterHistoryItem HandleIncrementA()
        {
            var npa = SetCountA + 1;
            if (CurrentGameCounter is MatchTieBreakFinalSetGameCounter || SatisfiesMatchEndingCondition(npa, SetCountB))
            {
                npa = -npa;
            }
            else
            {
                var newPoint = new TeamedValuePair<int>(npa, SetCountB);
                EnumTeams newServiceRight = CurrentGameCounter.CurrentServiceRight.Flip();
                return new SetCounterHistoryItem(SatisfiesTieBreakCondition(npa, SetCountB) ?
                    new MatchTieBreakFinalSetGameCounter(new TieBrakingPointCounter(MatchTieBreakPointCount, MatchTieBreakPointLeastDifference, newServiceRight), newServiceRight) :
                    GameCounterFactory.Create(newServiceRight), newPoint, newServiceRight);
            }
            return new SetCounterHistoryItem(null, new TeamedValuePair<int>(npa, SetCountB), CurrentServiceRight);
        }

        protected override SetCounterHistoryItem HandleIncrementB()
        {
            var npb = SetCountB + 1;
            if (CurrentGameCounter is MatchTieBreakFinalSetGameCounter || SatisfiesMatchEndingCondition(npb, SetCountA))
            {
                npb = -npb;
            }
            else
            {
                var newPoint = new TeamedValuePair<int>(SetCountA, npb);
                EnumTeams newServiceRight = CurrentGameCounter.CurrentServiceRight.Flip();
                return new SetCounterHistoryItem(SatisfiesTieBreakCondition(npb, SetCountA) ?
                    new MatchTieBreakFinalSetGameCounter(new TieBrakingPointCounter(MatchTieBreakPointCount, MatchTieBreakPointLeastDifference, newServiceRight), newServiceRight) :
                    GameCounterFactory.Create(newServiceRight), newPoint, newServiceRight);
            }
            return new SetCounterHistoryItem(null, new TeamedValuePair<int>(SetCountA, npb), CurrentServiceRight);
        }

        private bool SatisfiesMatchEndingCondition(int winner, int loser) => !SatisfiesTieBreakCondition(winner, loser) && winner > loser && winner + loser >= TotalSets - 1;

        //winner * 2 + 1 compiles into lea eax [rcx * 2 + 1]
        private bool SatisfiesTieBreakCondition(int winner, int loser) => winner == loser && winner * 2 + 1 == TotalSets;
    }
}