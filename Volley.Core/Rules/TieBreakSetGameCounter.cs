using System;
using System.Collections.Generic;
using System.Text;
using Volley.Matches;

namespace Volley.Rules
{
    public sealed class TieBreakSetGameCounter : CancellableGameCounterBase
    {
        public TieBreakSetGameCounter(IParametricFactory<IPointCounter, EnumTeams> normalGamePointCounterFactory,
            IParametricFactory<TieBrakingPointCounter, EnumTeams> tieBreakingGamePointCounterFactory, int leastWinningGames, int baseWinningDifference, EnumTeams initialServiceRight)
            : base(normalGamePointCounterFactory?.Create(initialServiceRight) ?? throw new ArgumentNullException(nameof(normalGamePointCounterFactory)), initialServiceRight)
        {
            NormalGamePointCounterFactory = normalGamePointCounterFactory ?? throw new ArgumentNullException(nameof(normalGamePointCounterFactory));
            TieBreakingGamePointCounterFactory = tieBreakingGamePointCounterFactory ?? throw new ArgumentNullException(nameof(tieBreakingGamePointCounterFactory));
            LeastWinningGames = leastWinningGames;
            BaseWinningDifference = baseWinningDifference;
        }

        //The GameCount* represents a game point with negated(two's complement of actual) point e.g.-6 => won a game with 6 games.
        //The (GameCountA | GameCountB) will always be negative if one of them is negative(=winner).
        public override bool IsSetOver => (GameCountA | GameCountB) < 0;

        public IParametricFactory<IPointCounter, EnumTeams> NormalGamePointCounterFactory { get; }

        public IParametricFactory<TieBrakingPointCounter, EnumTeams> TieBreakingGamePointCounterFactory { get; }

        public int LeastWinningGames { get; }

        public int BaseWinningDifference { get; }

        protected override GameCounterHistoryItem HandleIncrementA()
        {
            var npa = GameCountA + 1;
            if (CurrentPointCounter is TieBrakingPointCounter || SatisfiesGameEndingCondition(npa, GameCountB))
            {
                npa = -npa;
                return new GameCounterHistoryItem(null, new TeamedValuePair<int>(npa, GameCountB), CurrentServiceRight);
            }
            else
            {
                var newPoints = new TeamedValuePair<int>(npa, GameCountB);
                EnumTeams nsr = CurrentServiceRight.Flip();
                return SatisfiesTieBreakingCondition(npa, GameCountB)
                    ? new GameCounterHistoryItem(TieBreakingGamePointCounterFactory.Create(nsr), newPoints, nsr)
                    : new GameCounterHistoryItem(NormalGamePointCounterFactory.Create(nsr), newPoints, nsr);
            }
        }

        protected override GameCounterHistoryItem HandleIncrementB()
        {
            var npb = GameCountB + 1;
            if (CurrentPointCounter is TieBrakingPointCounter || SatisfiesGameEndingCondition(npb, GameCountA))
            {
                npb = -npb;
                return new GameCounterHistoryItem(null, new TeamedValuePair<int>(GameCountA, npb), CurrentServiceRight);
            }
            else
            {
                var newPoints = new TeamedValuePair<int>(GameCountA, npb);
                EnumTeams nsr = CurrentServiceRight.Flip();
                return SatisfiesTieBreakingCondition(npb, GameCountA)
                    ? new GameCounterHistoryItem(TieBreakingGamePointCounterFactory.Create(nsr), newPoints, nsr)
                    : new GameCounterHistoryItem(NormalGamePointCounterFactory.Create(nsr), newPoints, nsr);
            }
        }

        private bool SatisfiesGameEndingCondition(int winner, int loser) => winner - loser >= BaseWinningDifference && winner >= LeastWinningGames;

        private bool SatisfiesTieBreakingCondition(int winner, int loser) => winner == loser && loser == LeastWinningGames;

        public static IParametricFactory<TieBreakSetGameCounter, EnumTeams> CreateFactory(IParametricFactory<IPointCounter, EnumTeams> normalGamePointCounterFactory,
            IParametricFactory<TieBrakingPointCounter, EnumTeams> tieBreakingGamePointCounterFactory, int leastWinningGames, int baseWinningDifference) =>
            Extensions.CreateParametricFactory<TieBreakSetGameCounter, EnumTeams>(isr => new TieBreakSetGameCounter(normalGamePointCounterFactory,
                tieBreakingGamePointCounterFactory, leastWinningGames, baseWinningDifference, isr));
    }
}