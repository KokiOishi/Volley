using System;
using System.Collections.Generic;
using System.Text;
using Volley.Matches;

namespace Volley.Rules
{
    public sealed class StandardSetCounter : CancellableSetCounterBase
    {
        public StandardSetCounter(IParametricFactory<IGameCounter, EnumTeams> gameCounterFactory, int totalSets, EnumTeams initialServiceRight)
            : base(gameCounterFactory?.Create(initialServiceRight) ?? throw new ArgumentNullException(nameof(gameCounterFactory)), initialServiceRight)
        {
            GameCounterFactory = gameCounterFactory ?? throw new ArgumentNullException(nameof(gameCounterFactory));
            TotalSets = totalSets;
        }

        public override bool IsMatchOver => (SetCountA | SetCountB) < 0;

        public IParametricFactory<IGameCounter, EnumTeams> GameCounterFactory { get; }

        public int TotalSets { get; }

        protected override SetCounterHistoryItem HandleIncrementA()
        {
            var npa = SetCountA + 1;
            if (SatisfiesMatchEndingCondition(npa, SetCountB)) npa = -npa;
            else
            {
                var nsr = CurrentGameCounter.CurrentServiceRight.Flip();
                return new SetCounterHistoryItem(GameCounterFactory.Create(nsr), new TeamedValuePair<int>(npa, SetCountB), nsr);
            }
            return new SetCounterHistoryItem(null, new TeamedValuePair<int>(npa, SetCountB), CurrentServiceRight);
        }

        protected override SetCounterHistoryItem HandleIncrementB()
        {
            var npb = SetCountB + 1;
            if (SatisfiesMatchEndingCondition(npb, SetCountA)) npb = -npb;
            else
            {
                var nsr = CurrentGameCounter.CurrentServiceRight.Flip();
                return new SetCounterHistoryItem(GameCounterFactory.Create(nsr), new TeamedValuePair<int>(SetCountA, npb), nsr);
            }
            return new SetCounterHistoryItem(null, new TeamedValuePair<int>(SetCountA, npb), CurrentServiceRight);
        }

        private bool SatisfiesMatchEndingCondition(int winner, int loser) => winner + loser >= TotalSets;
    }
}