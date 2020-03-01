using System;
using System.Collections.Generic;
using System.Text;
using Volley.Players;
using Volley.Pointing;

namespace Volley
{
    /// <summary>
    /// Represents a Receive action.
    /// </summary>
    public readonly struct Receive : IEquatable<Receive>
    {
        /// <summary>
        /// TODO:XML DOCUMENT
        /// </summary>
        /// <param name="receivedPlayer"></param>
        /// <param name="side"></param>
        /// <param name="kind"></param>
        public Receive(Player receivedPlayer, HandSide side, ShotKind kind)
        {
            ReceivedPlayer = receivedPlayer;
            Side = side;
            Kind = kind;
        }

        /// <summary>
        /// Gets the <see cref="Player"/> who returned the ball.
        /// </summary>
        public Player ReceivedPlayer { get; }

        /// <summary>
        /// Gets the value which indicates whether the hand the <see cref="ReceivedPlayer"/> has used to return.
        /// </summary>
        public HandSide Side { get; }

        /// <summary>
        /// Gets the kind of shot, <see cref="ShotKind.Stroke"/> or <see cref="ShotKind.Volley"/>.
        /// </summary>
        public ShotKind Kind { get; }

        public bool IsServiceFault => (Kind & ShotKind.ServiceFault) > 0;

        public override bool Equals(object obj) => obj is Receive Receive && Equals(Receive);

        public bool Equals(Receive other) => EqualityComparer<Player>.Default.Equals(ReceivedPlayer, other.ReceivedPlayer) && Side == other.Side && Kind == other.Kind;

        public override int GetHashCode()
        {
            var hashCode = 0x64153F44;
            hashCode = hashCode * -0x5AAAAAD7 + EqualityComparer<Player>.Default.GetHashCode(ReceivedPlayer);
            hashCode = hashCode * -0x5AAAAAD7 + Side.GetHashCode();
            hashCode = hashCode * -0x5AAAAAD7 + Kind.GetHashCode();
            return hashCode;
        }

        public static bool operator ==(Receive left, Receive right) => left.Equals(right);

        public static bool operator !=(Receive left, Receive right) => !(left == right);

        public Receive WithPlayer(Player player) => new Receive(player, Side, Kind);
    }
}