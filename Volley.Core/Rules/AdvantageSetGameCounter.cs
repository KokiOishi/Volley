using System;
using System.Collections.Generic;
using System.Text;
using Volley.Matches;

namespace Volley.Rules
{
    public sealed class AdvantageSetGameCounter : CancellableGameCounterBase
    {
        public AdvantageSetGameCounter(IParametricFactory<IPointCounter, EnumTeams> normalGamePointCounterFactory, int leastWinningGames, int leastWinningDifference, EnumTeams initialServiceRight)
            : base(normalGamePointCounterFactory?.Create(initialServiceRight) ?? throw new ArgumentNullException(nameof(normalGamePointCounterFactory)), initialServiceRight)
        {
            NormalGamePointCounterFactory = normalGamePointCounterFactory ?? throw new ArgumentNullException(nameof(normalGamePointCounterFactory));
            LeastWinningGames = leastWinningGames > 0 ? leastWinningGames :
                throw new ArgumentOutOfRangeException(nameof(leastWinningGames), $"The leastWinningGames must be grater than 0!");
            LeastWinningDifference = leastWinningDifference >= 0 ? leastWinningDifference :
                throw new ArgumentOutOfRangeException(nameof(leastWinningDifference), $"The leastWinningGames must be grater than or equal to 0!");
        }

        //The GameCount* represents a game point with negated(two's complement of actual) point e.g.-6 => won a game with 6 games.
        //The (GameCountA | GameCountB) will always be negative if one of them is negative(=winner).
        public override bool IsSetOver => (GameCountA | GameCountB) < 0;

        public IParametricFactory<IPointCounter, EnumTeams> NormalGamePointCounterFactory { get; }

        public int LeastWinningGames { get; }

        public int LeastWinningDifference { get; }

        protected override GameCounterHistoryItem HandleIncrementA()
        {
            var npa = GameCountA + 1;
            if (SatisfiesSetEndingCondition(npa, GameCountB))
            {
                npa = -npa;
            }
            else
            {
                EnumTeams newServiceRight = CurrentServiceRight.Flip();
                return new GameCounterHistoryItem(NormalGamePointCounterFactory.Create(newServiceRight), new TeamedValuePair<int>(npa, GameCountB), newServiceRight);
            }
            return new GameCounterHistoryItem(null, new TeamedValuePair<int>(npa, GameCountB), CurrentServiceRight);
        }

        protected override GameCounterHistoryItem HandleIncrementB()
        {
            var npb = GameCountB + 1;
            if (SatisfiesSetEndingCondition(npb, GameCountA))
            {
                npb = -npb;
            }
            else
            {
                EnumTeams newServiceRight = CurrentServiceRight.Flip();
                return new GameCounterHistoryItem(NormalGamePointCounterFactory.Create(newServiceRight), new TeamedValuePair<int>(GameCountA, npb), newServiceRight);
            }
            return new GameCounterHistoryItem(null, new TeamedValuePair<int>(GameCountA, npb), CurrentServiceRight);
        }

        private bool SatisfiesSetEndingCondition(int winner, int loser) => winner - loser >= LeastWinningDifference && winner >= LeastWinningGames;

        public static IParametricFactory<AdvantageSetGameCounter, EnumTeams> CreateFactory(IParametricFactory<IPointCounter, EnumTeams> normalGamePointCounterFactory, int leastWinningGames, int leastWinningDifference)
            => Extensions.CreateParametricFactory<AdvantageSetGameCounter, EnumTeams>(isr =>
            new AdvantageSetGameCounter(normalGamePointCounterFactory, leastWinningGames, leastWinningDifference, isr));
    }
}