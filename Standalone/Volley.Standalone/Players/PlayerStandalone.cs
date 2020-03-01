using System;
using Volley.Players;
using Ceras;
using Ceras.Formatters.AotGenerator;

namespace Volley.Standalone.Players
{
    /// <summary>
    /// Represents and manipulates a tennis player.
    /// </summary>
    public class PlayerStandalone : Player
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="T:Volley.Standalone.Players.PlayerStandalone"/> class.
        /// </summary>
        /// <param name="identifier">Identifier.</param>
        /// <param name="name">Name.</param>
        /// <param name="gender">Gender.</param>
        /// <param name="height">Height.</param>
        /// <param name="weight">Weight.</param>
        /// <param name="dominantHands">Dominant hands.</param>
        public PlayerStandalone(Guid identifier, string name, Gender gender, double height, double weight, Hands dominantHands, Color color) : base(identifier, name, gender, height, weight, dominantHands)
        {
            Color = color;
        }

        public Color Color { get; }
    }
}