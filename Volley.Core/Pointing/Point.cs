using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Volley.Players;
using Volley.Team;

namespace Volley.Pointing
{
    /// <summary>
    /// Defines a base structure of tennis points.
    /// </summary>
    public abstract class Point<TTeam> where TTeam : class, ITeamInMatch
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="T:Volley.Points.Point`1"/> class.
        /// </summary>
        /// <param name="pointKind">Point kind.</param>
        /// <param name="winnerTeam">Winner team.</param>
        /// <param name="shotKind">Shot kind.</param>
        /// <param name="side"></param>
        /// <param name="receives">Recieves performed.</param>
        protected Point(PointKinds pointKind, TTeam winnerTeam, ShotKind shotKind, HandSide side, IEnumerable<Receive> receives)
        {
            PointKind = pointKind;
            WinnerTeam = winnerTeam ?? throw new ArgumentNullException(nameof(winnerTeam));
            ShotKind = shotKind;
            Side = side;
            Receives = receives?.ToArray() ?? throw new ArgumentNullException(nameof(receives));
        }

        /// <summary>
        /// Gets the kind of point.
        /// </summary>
        public PointKinds PointKind { get; }

        /// <summary>
        /// Gets the winner.
        /// </summary>
        /// <value>The winner.</value>
        public TTeam WinnerTeam { get; }

        /// <summary>
        /// Gets the kind of the shot.
        /// </summary>
        /// <value>The kind of the shot.</value>
        public ShotKind ShotKind { get; }

        public HandSide Side { get; }

        /// <summary>
        /// Gets the number of rallies.
        /// </summary>
        /// <value>The number rallies.</value>
        public int NumberRallies => Receives?.Length ?? 0;

        public Receive[] Receives { get; }
    }
}
