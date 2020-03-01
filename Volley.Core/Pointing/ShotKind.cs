using System;

namespace Volley.Pointing
{
    /// <summary>
    /// Shot kind.
    /// </summary>
    [Flags]
    public enum ShotKind : uint
    {
        /// <summary>
        /// The Nothing (equivalent to Stroke)
        /// </summary>
        None = 0,

        /// <summary>
        /// The stroke.
        /// </summary>
        Stroke = None,

        /// <summary>
        /// The volley.
        /// </summary>
        Volley = 1,

        /// <summary>
        /// The Service fault.
        /// </summary>
        ServiceFault = 0b10
    }
}