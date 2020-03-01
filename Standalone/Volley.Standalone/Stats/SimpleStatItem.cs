using System;
using System.Collections.Generic;
using System.Text;

namespace Volley.Standalone.Stats
{
    public readonly struct SimpleStatItem<T> : IStatItem<T>, IEquatable<SimpleStatItem<T>>
    {
        public SimpleStatItem(string name, string textValue, T value)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));
            TextValue = textValue ?? throw new ArgumentNullException(nameof(textValue));
            Value = value;
        }

        public string Name { get; }

        public string TextValue { get; }

        public T Value { get; }

        public override bool Equals(object obj) => obj is SimpleStatItem<T> item && Equals(item);

        public bool Equals(SimpleStatItem<T> other) => Name == other.Name && TextValue == other.TextValue && EqualityComparer<T>.Default.Equals(Value, other.Value);

        public override int GetHashCode()
        {
            var hashCode = 900920807;
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Name);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(TextValue);
            hashCode = hashCode * -1521134295 + EqualityComparer<T>.Default.GetHashCode(Value);
            return hashCode;
        }

        public static bool operator ==(SimpleStatItem<T> left, SimpleStatItem<T> right) => left.Equals(right);

        public static bool operator !=(SimpleStatItem<T> left, SimpleStatItem<T> right) => !(left == right);
    }
}
