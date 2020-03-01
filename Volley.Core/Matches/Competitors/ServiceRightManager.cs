using System;
using Volley.Players;
using Volley.Team;

namespace Volley.Matches.Competitors
{
    /// <summary>
    /// Service right manager.
    /// </summary>
    [Obsolete("", true)]
    public class ServiceRightManager<TTeam> where TTeam : class, ITeamInMatch
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="T:Volley.Matches.Competitors.ServiceRightManager`1"/> class.
        /// </summary>
        /// <param name="teamA">Team a.</param>
        /// <param name="teamB">Team b.</param>
        /// <param name="initialService">Initial service.</param>
        public ServiceRightManager(TTeam teamA, TTeam teamB, EnumTeams initialService)
        {
            TeamA = teamA ?? throw new ArgumentNullException(nameof(teamA));
            TeamB = teamB ?? throw new ArgumentNullException(nameof(teamB));
            CurrentServiceRight = initialService;
        }

        /// <summary>
        /// Gets the current service right.
        /// </summary>
        /// <value>The current service right.</value>
        public EnumTeams CurrentServiceRight { get; protected set; }

        /// <summary>
        /// Gets the team a.
        /// </summary>
        /// <value>The team a.</value>
        public TTeam TeamA { get; }

        /// <summary>
        /// Gets the team b.
        /// </summary>
        /// <value>The team b.</value>
        public TTeam TeamB { get; }

        /// <summary>
        /// Gets the service team.
        /// </summary>
        /// <value>The service team.</value>
        public TTeam ServiceTeam => CurrentServiceRight == EnumTeams.TeamA ? TeamA : TeamB;

        /// <summary>
        /// Gets the Receive team.
        /// </summary>
        /// <value>The Receive team.</value>
        public TTeam ReceiveTeam => CurrentServiceRight == EnumTeams.TeamB ? TeamA : TeamB;

        /// <summary>
        /// Called when the game is over.
        /// </summary>
        public virtual void OnGameOver()
        {
            ServiceTeam.OnGameOver();
            switch (CurrentServiceRight)
            {
                case EnumTeams.TeamA:
                    CurrentServiceRight = EnumTeams.TeamB;
                    break;

                case EnumTeams.TeamB:
                    CurrentServiceRight = EnumTeams.TeamA;
                    break;

                default:
                    break;
            }
        }

        /// <summary>
        /// Called when the game is cancelled.
        /// </summary>
        public virtual void OnGameCancelled()
        {
            switch (CurrentServiceRight)
            {
                case EnumTeams.TeamA:
                    CurrentServiceRight = EnumTeams.TeamB;
                    break;

                case EnumTeams.TeamB:
                    CurrentServiceRight = EnumTeams.TeamA;
                    break;

                default:
                    break;
            }
            ServiceTeam.OnGameCancelled();
        }
    }
}