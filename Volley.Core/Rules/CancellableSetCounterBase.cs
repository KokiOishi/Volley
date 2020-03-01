using System;
using System.Collections.Generic;
using System.Text;
using Volley.Matches;

namespace Volley.Rules
{
    public readonly struct SetCounterHistoryItem : IEquatable<SetCounterHistoryItem>
    {
        public readonly IGameCounter GameCounter;
        public readonly TeamedValuePair<int> SetCounts;
        public readonly EnumTeams ServiceRight;

        public SetCounterHistoryItem(IGameCounter gameCounter, TeamedValuePair<int> setCounts, EnumTeams serviceRight)
        {
            GameCounter = gameCounter;
            SetCounts = setCounts;
            ServiceRight = serviceRight;
        }

        public override bool Equals(object obj) => obj is SetCounterHistoryItem item && Equals(item);

        public bool Equals(SetCounterHistoryItem other) => EqualityComparer<IGameCounter>.Default.Equals(GameCounter, other.GameCounter) && SetCounts.Equals(other.SetCounts) && ServiceRight == other.ServiceRight;

        public override int GetHashCode()
        {
            var hashCode = -622953166;
            hashCode = hashCode * -1521134295 + EqualityComparer<IGameCounter>.Default.GetHashCode(GameCounter);
            hashCode = hashCode * -1521134295 + SetCounts.GetHashCode();
            hashCode = hashCode * -1521134295 + ServiceRight.GetHashCode();
            return hashCode;
        }

        public void Deconstruct(out IGameCounter gameCounter, out TeamedValuePair<int> setCounts, out EnumTeams serviceRight)
        {
            gameCounter = GameCounter;
            setCounts = SetCounts;
            serviceRight = ServiceRight;
        }

        public static bool operator ==(SetCounterHistoryItem left, SetCounterHistoryItem right) => left.Equals(right);

        public static bool operator !=(SetCounterHistoryItem left, SetCounterHistoryItem right) => !(left == right);
    }

    public abstract class CancellableSetCounterBase : ISetCounter
    {
        protected CancellableSetCounterBase(IGameCounter currentGameCounter, EnumTeams initialServiceRight)
        {
            SetCountA = SetCountB = 0;
            CurrentGameCounter = currentGameCounter ?? throw new ArgumentNullException(nameof(currentGameCounter));
            History = new Stack<SetCounterHistoryItem>();
            CurrentServiceRight = initialServiceRight;
            PushHistory(new SetCounterHistoryItem(currentGameCounter, new TeamedValuePair<int>(0, 0), initialServiceRight));
        }

        public int SetCountA { get; private set; }
        public int SetCountB { get; private set; }
        public IGameCounter CurrentGameCounter { get; private set; }
        public abstract bool IsMatchOver { get; }
        public EnumTeams CurrentServiceRight { get; private set; }
        private Stack<SetCounterHistoryItem> History { get; }

        public void IncrementA() => PushHistory(HandleIncrementA());

        public void IncrementB() => PushHistory(HandleIncrementB());

        public void Rollback() => PopHistory();

        protected abstract SetCounterHistoryItem HandleIncrementA();

        protected abstract SetCounterHistoryItem HandleIncrementB();

        private void PopHistory()
        {
            (_, _, CurrentServiceRight) = History.Pop();
            (CurrentGameCounter, (SetCountA, SetCountB), _) = History.Peek();
        }

        private void PushHistory(SetCounterHistoryItem values)
        {
            History.Push(new SetCounterHistoryItem(CurrentGameCounter, values.SetCounts, CurrentServiceRight));
            (CurrentGameCounter, (SetCountA, SetCountB), CurrentServiceRight) = values;
        }
    }
}