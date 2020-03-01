using System;
using System.Collections.Generic;
using System.Text;

namespace Volley.Players
{
    /// <summary>
    /// Represents which the "hand" is refered.
    /// </summary>
    [Flags]
    public enum Hands
    {
        /// <summary>
        /// None of hands.
        /// </summary>
        None = 0b00,

        /// <summary>
        /// Left hand
        /// </summary>
        Left = 0b01,

        /// <summary>
        /// Right hand
        /// </summary>
        Right = 0b10
    }
}
