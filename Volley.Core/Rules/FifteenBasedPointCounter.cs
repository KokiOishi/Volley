using System;
using System.Collections.Generic;
using System.Text;
using Volley.Matches;
using Volley.Pointing;

namespace Volley.Rules
{
    public sealed class FifteenBasedPointCounter : CancellablePointCounterBase<PointNumber>
    {
        public FifteenBasedPointCounter(bool hasDeuce, EnumTeams initialServiceRight) : base(PointNumber.Love, PointNumber.Love, initialServiceRight)
        {
            HasDeuce = hasDeuce;
        }

        /// <summary>
        /// <c>true</c> when No-Ad Scoring, otherwise, Standard Scoring.
        /// </summary>
        public bool HasDeuce { get; }

        public override bool IsGameSet => (InternalPointA.IsGameSet() || InternalPointB.IsGameSet());

        protected override string FormatPoint(PointNumber value) => value.ToShortString();

        protected override PointCounterHistoryItem<PointNumber> HandleDecrementA()
        {
            var (npA, npB) = RemovePoint(InternalPointA, InternalPointB);
            return new PointCounterHistoryItem<PointNumber>(new TeamedValuePair<PointNumber>(npA, npB), CurrentServiceRight);
        }

        protected override PointCounterHistoryItem<PointNumber> HandleDecrementB()
        {
            var (npB, npA) = RemovePoint(InternalPointB, InternalPointA);
            return new PointCounterHistoryItem<PointNumber>(new TeamedValuePair<PointNumber>(npA, npB), CurrentServiceRight);
        }

        protected override PointCounterHistoryItem<PointNumber> HandleIncrementA()
        {
            var (npA, npB) = AddPoint(InternalPointA, InternalPointB);
            return new PointCounterHistoryItem<PointNumber>(new TeamedValuePair<PointNumber>(npA, npB), CurrentServiceRight);
        }

        protected override PointCounterHistoryItem<PointNumber> HandleIncrementB()
        {
            var (npB, npA) = AddPoint(InternalPointB, InternalPointA);
            return new PointCounterHistoryItem<PointNumber>(new TeamedValuePair<PointNumber>(npA, npB), CurrentServiceRight);
        }

        private (PointNumber winner, PointNumber loser) AddPoint(PointNumber winner, PointNumber loser)
        {
            switch (winner)
            {
                case PointNumber.Love:
                case PointNumber.Fifteen:
                case PointNumber.Thirty:
                    return (winner + 1, loser);

                case PointNumber.Fourty:
                    switch (loser)
                    {
                        case PointNumber.Advantage:
                            return (PointNumber.Fourty, PointNumber.Fourty);

                        case PointNumber.Fourty when HasDeuce:
                            return (PointNumber.Advantage, PointNumber.Fourty);

                        default:
                            return (PointNumber.Game, loser);
                    }
                case PointNumber.Advantage:
                    switch (loser)
                    {
                        case PointNumber.Advantage:
                            throw new InvalidOperationException("The both winner and loser must not be Advantage!");
                        default:
                            return (PointNumber.Game, loser);
                    }
                case PointNumber.Game:
                    throw new InvalidOperationException("The winner must not be Game!");
                default:
                    throw new ArgumentException($"The value {winner} is Invalid!");
            }
        }

        private (PointNumber deprivated, PointNumber opponent) RemovePoint(PointNumber deprivated, PointNumber opponent)
        {
            switch (deprivated)
            {
                case PointNumber.Love:
                    //Tried to remove point while the player has no point.
                    throw new ArgumentOutOfRangeException(nameof(deprivated), $"{nameof(deprivated)} has no point!");
                case PointNumber.Fifteen when opponent == PointNumber.Advantage:
                case PointNumber.Thirty when opponent == PointNumber.Advantage:
                    //WTF the opponent is Advantage illegaly!
                    throw new ArgumentException($"{nameof(opponent)} cannot be {PointNumber.Advantage} while {nameof(deprivated)} is not {PointNumber.Fourty}!");
                case PointNumber.Fourty when opponent == PointNumber.Advantage:
                    //Tried to remove point while the player has lost previously.
                    throw new ArgumentException($"The last point of {nameof(deprivated)} cannot be canceled while the point of {nameof(opponent)} is {PointNumber.Advantage}!");
                case PointNumber.Advantage when opponent != PointNumber.Fourty:
                    //WTF the player is Advantage illegally!
                    throw new ArgumentException($"{nameof(deprivated)} cannot be {PointNumber.Advantage} while {nameof(opponent)} is not {PointNumber.Fourty}!");
                case PointNumber.Fifteen:
                case PointNumber.Thirty:
                case PointNumber.Fourty:
                case PointNumber.Advantage:
                case PointNumber.Game when HasDeuce && opponent == PointNumber.Fourty:
                    //Legally cancelling the previous input.
                    return (deprivated - 1, opponent);

                case PointNumber.Game:
                    //Legally cancelling the previous input.
                    return (PointNumber.Fourty, opponent);

                default:
                    throw new ArgumentException($"The value {deprivated} is Invalid!");
            }
        }

        public static IParametricFactory<FifteenBasedPointCounter, EnumTeams> CreateFactory(bool hasDeuce) =>
            Extensions.CreateParametricFactory<FifteenBasedPointCounter, EnumTeams>(isr => new FifteenBasedPointCounter(hasDeuce, isr));
    }
}