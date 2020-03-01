using System;
using System.Collections.Generic;
using System.Text;
using Volley.Matches;

//using TItem = System.ValueTuple<Volley.Scoring.IPointCounter, Volley.TeamedValuePair<int>>;

namespace Volley.Rules
{
    public readonly struct GameCounterHistoryItem : IEquatable<GameCounterHistoryItem>
    {
        public readonly IPointCounter Counter;

        public readonly TeamedValuePair<int> GameCounts;

        public readonly EnumTeams ServiceRight;

        public GameCounterHistoryItem(IPointCounter counter, TeamedValuePair<int> gameCounts, EnumTeams serviceRight)
        {
            Counter = counter;
            GameCounts = gameCounts;
            ServiceRight = serviceRight;
        }

        public void Deconstruct(out IPointCounter pointCounter, out TeamedValuePair<int> gameCounts, out EnumTeams serviceRight)
        {
            pointCounter = Counter;
            gameCounts = GameCounts;
            serviceRight = ServiceRight;
        }

        public override bool Equals(object obj) => obj is GameCounterHistoryItem item && Equals(item);

        public bool Equals(GameCounterHistoryItem other) => EqualityComparer<IPointCounter>.Default.Equals(Counter, other.Counter) && GameCounts.Equals(other.GameCounts) && ServiceRight == other.ServiceRight;

        public override int GetHashCode()
        {
            var hashCode = 1505146720;
            hashCode = hashCode * -1521134295 + EqualityComparer<IPointCounter>.Default.GetHashCode(Counter);
            hashCode = hashCode * -1521134295 + GameCounts.GetHashCode();
            hashCode = hashCode * -1521134295 + ServiceRight.GetHashCode();
            return hashCode;
        }

        public static bool operator ==(GameCounterHistoryItem left, GameCounterHistoryItem right) => left.Equals(right);

        public static bool operator !=(GameCounterHistoryItem left, GameCounterHistoryItem right) => !(left == right);
    }

    public abstract class CancellableGameCounterBase : IGameCounter
    {
        protected CancellableGameCounterBase(IPointCounter initialPointCounter, EnumTeams initialServiceRight)
        {
            GameCountA = GameCountB = 0;
            CurrentPointCounter = initialPointCounter ?? throw new ArgumentNullException(nameof(initialPointCounter));
            History = new Stack<GameCounterHistoryItem>();
            CurrentServiceRight = initialServiceRight;
            PushHistory(new GameCounterHistoryItem(initialPointCounter, new TeamedValuePair<int>(0, 0), initialServiceRight));
        }

        public abstract bool IsSetOver { get; }
        public int GameCountA { get; private set; }
        public int GameCountB { get; private set; }
        public IPointCounter CurrentPointCounter { get; private set; }
        public EnumTeams CurrentServiceRight { get; private set; }
        private Stack<GameCounterHistoryItem> History { get; }

        public void IncrementA() => PushHistory(HandleIncrementA());

        public void IncrementB() => PushHistory(HandleIncrementB());

        public void Rollback() => PopHistory();

        protected abstract GameCounterHistoryItem HandleIncrementA();

        protected abstract GameCounterHistoryItem HandleIncrementB();

        private void PopHistory()
        {
            (_, _, CurrentServiceRight) = History.Pop();
            (CurrentPointCounter, (GameCountA, GameCountB), _) = History.Peek();
        }

        private void PushHistory(GameCounterHistoryItem values)
        {
            History.Push(new GameCounterHistoryItem(CurrentPointCounter, values.GameCounts, CurrentServiceRight));
            (CurrentPointCounter, (GameCountA, GameCountB), CurrentServiceRight) = values;
        }
    }
}