using System;
namespace Volley.Tournaments
{
    /// <summary>
    /// Defines a base representations of a tournament for standalone.
    /// Defines a base infrastructure of a tournament for government.
    /// </summary>
    public abstract class Tournament
    {
        /// <summary>
        /// Gets the name.
        /// </summary>
        /// <value>The name.</value>
        public string Name { get; }

        /// <summary>
        /// Gets the location.
        /// </summary>
        /// <value>The location.</value>
        public string Location { get; }
    }
}
