using System;
using System.Collections.Generic;
using System.Text;
using Volley.Matches;

namespace Volley.Rules
{
    /// <summary>
    /// Counts several points for tie-braking games.
    /// </summary>
    public sealed class TieBrakingPointCounter : CancellablePointCounterBase<int>
    {
        public TieBrakingPointCounter(int leastWinningPoint, int leastWinningDifference, EnumTeams initialServiceRight) : base(0, 0, initialServiceRight)
        {
            LeastWinningPoint = leastWinningPoint;
            LeastWinningDifference = leastWinningDifference;
        }

        public int LeastWinningPoint { get; }

        public int LeastWinningDifference { get; }

        //The InternalPoint* represents a game point with negated(two's complement of actual) point e.g.-7 => won a game with 7 point.
        //The (InternalPointA | InternalPointB) will always be negative if one of them is negative(=winner).
        public override bool IsGameSet => (InternalPointA | InternalPointB) < 0;

        protected override PointCounterHistoryItem<int> HandleIncrementA()
        {
            var npa = InternalPointA + 1;
            if (SatisfiesGameEndingCondition(npa, InternalPointB)) npa = -npa;
            return new PointCounterHistoryItem<int>(new TeamedValuePair<int>(npa, InternalPointB), FlipIfEven(npa + InternalPointB));
        }

        protected override PointCounterHistoryItem<int> HandleIncrementB()
        {
            var npb = InternalPointB + 1;
            if (SatisfiesGameEndingCondition(npb, InternalPointA)) npb = -npb;
            return new PointCounterHistoryItem<int>(new TeamedValuePair<int>(InternalPointA, npb), FlipIfEven(InternalPointA + npb));
        }

        private EnumTeams FlipIfEven(int newPointSum) => newPointSum < 0 ? CurrentServiceRight : ((newPointSum & 1) > 0 ? CurrentServiceRight.Flip() : CurrentServiceRight);

        protected override PointCounterHistoryItem<int> HandleDecrementA()
        {
            if (InternalPointA < 0) //The game is already set
            {
                //~a means (-a)-1 witch can be calculated with single NOT instruction
                int npa = ~InternalPointA;
                return new PointCounterHistoryItem<int>(new TeamedValuePair<int>(npa, InternalPointB), FlipIfEven(npa + InternalPointB));
            }
            else
            {
                int npa = InternalPointA - 1;
                return new PointCounterHistoryItem<int>(new TeamedValuePair<int>(npa, InternalPointB), FlipIfEven(npa + InternalPointB));
            }
        }

        protected override PointCounterHistoryItem<int> HandleDecrementB()
        {
            if (InternalPointB < 0) //The game is already set
            {
                //~b means (-b)-1 witch can be calculated with single NOT instruction
                int npb = ~InternalPointB;
                return new PointCounterHistoryItem<int>(new TeamedValuePair<int>(InternalPointA, npb), FlipIfEven(InternalPointA + npb));
            }
            else
            {
                int npb = InternalPointB - 1;
                return new PointCounterHistoryItem<int>(new TeamedValuePair<int>(InternalPointA, npb), FlipIfEven(InternalPointA + npb));
            }
        }

        private bool SatisfiesGameEndingCondition(int winner, int loser) => winner - loser >= LeastWinningDifference && winner >= LeastWinningPoint;

        protected override string FormatPoint(int value) => value < 0 ? $"*{-value}" : $"{value}";

        public static IParametricFactory<TieBrakingPointCounter, EnumTeams> CreateFactory(int leastWinningPoint, int leastWinningDifference) =>
            Extensions.CreateParametricFactory<TieBrakingPointCounter, EnumTeams>(isr => new TieBrakingPointCounter(leastWinningPoint, leastWinningDifference, isr));
    }
}