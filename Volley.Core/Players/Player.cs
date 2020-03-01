using System;
using System.Collections.Generic;
using System.Text;

namespace Volley.Players
{
    /// <summary>
    /// Represents and manipulates a tennis player.
    /// </summary>
    public class Player
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="T:Volley.Players.Player"/> class.
        /// </summary>
        /// <param name="identifier">Identifier.</param>
        /// <param name="name">Name.</param>
        /// <param name="gender">Gender.</param>
        /// <param name="height">Height.</param>
        /// <param name="weight">Weight.</param>
        /// <param name="dominantHands">Dominant hands.</param>
        public Player(Guid identifier, string name, Gender gender, double height, double weight, Hands dominantHands)
        {
            Identifier = identifier;
            Name = string.IsNullOrEmpty(name) ? throw new ArgumentNullException(nameof(name)) : name;
            Gender = gender;
            Height = height <= 0 ? throw new ArgumentOutOfRangeException(nameof(height), $"{nameof(height)} must be grater than 0!") :
                double.IsNaN(height) ? throw new ArgumentOutOfRangeException(nameof(height), $"{nameof(height)} must not be NaN!") :
                double.IsPositiveInfinity(height) ? throw new ArgumentOutOfRangeException(nameof(height), $"{nameof(height)} must not be positive infinity!") : height;
            Weight = weight <= 0 ? throw new ArgumentOutOfRangeException(nameof(weight), $"{nameof(weight)} must be grater than 0!") :
                double.IsNaN(weight) ? throw new ArgumentOutOfRangeException(nameof(weight), $"{nameof(weight)} must not be NaN!") :
                double.IsPositiveInfinity(weight) ? throw new ArgumentOutOfRangeException(nameof(weight), $"{nameof(weight)} must not be positive infinity!") : weight;
            DominantHands = dominantHands;
        }

        /// <summary>
        /// Gets the identifier of the player.
        /// </summary>
        public Guid Identifier { get; }

        /// <summary>
        /// Gets the player's name.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Gets the player's gender.
        /// </summary>
        public Gender Gender { get; }

        /// <summary>
        /// Gets the height of the player.
        /// </summary>
        public double Height { get; }

        /// <summary>
        /// Gets the weight of the player.
        /// </summary>
        public double Weight { get; }

        /// <summary>
        /// Gets the dominant hands.
        /// </summary>
        public Hands DominantHands { get; }
    }
}
