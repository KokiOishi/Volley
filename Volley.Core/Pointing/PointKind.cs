using System;
using System.Collections.Generic;
using System.Text;

namespace Volley.Pointing
{
    /// <summary>
    /// Represents a kind of point.
    /// </summary>
    [Flags]
    public enum PointKinds
    {
        #region Flags

        /// <summary>
        /// Represents the point is error.
        /// </summary>
        Error = 1,

        /// <summary>
        /// Represents the error is not forced.
        /// </summary>
        Unforced = 2,

        #endregion Flags

        #region Kinds

        /// <summary>
        /// Winner(The point that the opponent did not touch the ball.)
        /// </summary>
        Winner = 0,

        /// <summary>
        /// Forced Error(The point that the opponent touched the ball but could not return the ball.)
        /// </summary>
        ForcedError = Error,

        /// <summary>
        /// Unforced Error(The point that the opponent tried to return the ball well but failed.)
        /// </summary>
        UnforcedError = Error | Unforced,

        #endregion Kinds
    }
}
