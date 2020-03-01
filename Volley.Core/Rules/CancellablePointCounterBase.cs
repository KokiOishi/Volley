using System;
using System.Collections.Generic;
using System.Text;
using Volley.Matches;

namespace Volley.Rules
{
    public readonly struct PointCounterHistoryItem<TPoint> : IEquatable<PointCounterHistoryItem<TPoint>> where TPoint : unmanaged
    {
        public readonly TeamedValuePair<TPoint> PointCounts;
        public readonly EnumTeams ServiceRight;

        public PointCounterHistoryItem(TeamedValuePair<TPoint> pointCounts, EnumTeams serviceRight)
        {
            PointCounts = pointCounts;
            ServiceRight = serviceRight;
        }

        public override bool Equals(object obj) => obj is PointCounterHistoryItem<TPoint> item && Equals(item);

        public bool Equals(PointCounterHistoryItem<TPoint> other) => PointCounts.Equals(other.PointCounts) && ServiceRight == other.ServiceRight;

        public override int GetHashCode()
        {
            var hashCode = 641748231;
            hashCode = hashCode * -1521134295 + PointCounts.GetHashCode();
            hashCode = hashCode * -1521134295 + ServiceRight.GetHashCode();
            return hashCode;
        }

        public static bool operator ==(PointCounterHistoryItem<TPoint> left, PointCounterHistoryItem<TPoint> right) => left.Equals(right);

        public static bool operator !=(PointCounterHistoryItem<TPoint> left, PointCounterHistoryItem<TPoint> right) => !(left == right);
    }

    public abstract class CancellablePointCounterBase<TPoint> : IPointCounter where TPoint : unmanaged
    {
        protected CancellablePointCounterBase(TPoint internalPointA, TPoint internalPointB, EnumTeams initialServiceRight)
        {
            InternalPointA = internalPointA;
            InternalPointB = internalPointB;
            History = new Stack<PointCounterHistoryItem<TPoint>>();
            CurrentServiceRight = initialServiceRight;
            PushHistory(new PointCounterHistoryItem<TPoint>(new TeamedValuePair<TPoint>(internalPointA, internalPointB), initialServiceRight));
        }

        public abstract bool IsGameSet { get; }
        public string PointA => FormatPoint(InternalPointA);
        public string PointB => FormatPoint(InternalPointB);

        public EnumTeams CurrentServiceRight { get; private set; }
        protected TPoint InternalPointA { get; private set; }
        protected TPoint InternalPointB { get; private set; }
        private Stack<PointCounterHistoryItem<TPoint>> History { get; }

        public void DecrementA() => PushHistory(HandleDecrementA());

        public void DecrementB() => PushHistory(HandleDecrementB());

        public void IncrementA() => PushHistory(HandleIncrementA());

        public void IncrementB() => PushHistory(HandleIncrementB());

        public void Rollback() => PopHistory();

        protected abstract string FormatPoint(TPoint value);

        protected abstract PointCounterHistoryItem<TPoint> HandleDecrementA();

        protected abstract PointCounterHistoryItem<TPoint> HandleDecrementB();

        protected abstract PointCounterHistoryItem<TPoint> HandleIncrementA();

        protected abstract PointCounterHistoryItem<TPoint> HandleIncrementB();

        private void PopHistory()
        {
            CurrentServiceRight = History.Pop().ServiceRight;
            PointCounterHistoryItem<TPoint> peek = History.Peek();
            (InternalPointA, InternalPointB) = peek.PointCounts;
        }

        private void PushHistory(PointCounterHistoryItem<TPoint> points)
        {
            History.Push(new PointCounterHistoryItem<TPoint>(points.PointCounts, CurrentServiceRight));
            (InternalPointA, InternalPointB) = points.PointCounts;
            CurrentServiceRight = points.ServiceRight;
        }
    }
}