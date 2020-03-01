using System;
using System.Collections.Generic;
using System.ComponentModel;
using Volley.Matches;
using Volley.Matches.Competitors;
using Volley.Matches.Rules;
using Volley.Rules;
using Volley.Sets;
using Volley.Standalone.Sets;
using Volley.Team;

namespace Volley.Standalone.Matches
{
    /// <summary>
    /// Defines a base infrastructure of tennis match.
    /// </summary>
    public class MatchStandalone<TTeam> : Match<TTeam>, INotifyPropertyChanged where TTeam : class, ITeamInMatch
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MatchStandalone{TTeam}"/> class.
        /// </summary>
        /// <param name="teamA">Team a.</param>
        /// <param name="teamB">Team b.</param>
        /// <param name="rule">Rule.</param>
        /// <param name="initialService">The team that serves initially.</param>
        /// <param name="setCounter">The set counter.</param>
        /// <exception cref="ArgumentException">Occures when the <paramref name="initialService"/> is equal to neither <paramref name="teamA"/> nor <paramref name="teamB"/>.</exception>
        public MatchStandalone(TTeam teamA, TTeam teamB, MatchRule rule, EnumTeams initialService, ISetCounter setCounter)
            : base(teamA, teamB, rule, setCounter)
        {
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public override int SetCountA
        {
            get => base.SetCountA;
            protected set => this.SetAndNotifyIfChanged(PropertyChanged, base.SetCountA, base.SetCountA = value, nameof(SetCountA));
        }

        public override int SetCountB
        {
            get => base.SetCountB;
            protected set => this.SetAndNotifyIfChanged(PropertyChanged, base.SetCountB, base.SetCountB = value, nameof(SetCountB));
        }

        /// <summary>
        /// Called when the match is over.
        /// </summary>
        protected override void OnMatchOver()
        {
        }

        /// <summary>
        /// Called when the match is starting.
        /// </summary>
        /// <returns>The match start.</returns>
        protected override Set<TTeam> OnMatchStart()
        {
            TTeam serviceTeam = SetCounter.CurrentServiceRight.Switch(TeamA, TeamB), returnTeam = SetCounter.CurrentServiceRight.Flip().Switch(TeamA, TeamB);
            var set = new SetStandalone<TTeam>(this);
            set.StartSet();
            set.PropertyChanged += Set_PropertyChanged;
            return set;
        }

        private void Set_PropertyChanged(object sender, PropertyChangedEventArgs e) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(CurrentSet)));

        /// <summary>
        /// Cancels the last set. Call when the last set information was invalid or incorrect.
        /// </summary>
        /// <param name="set">Set.</param>
        protected override void OnSetCanceled(Set<TTeam> set)
        {
        }

        /// <summary>
        /// Adds the set. Call when the set got finished.
        /// </summary>
        /// <param name="set">Set.</param>
        protected override Set<TTeam> OnSetEnd(Set<TTeam> set)
        {
            if (set is INotifyPropertyChanged NotifyPropertyChanged) NotifyPropertyChanged.PropertyChanged -= Set_PropertyChanged;
            return SetCountA < 0 || SetCountB < 0 ? null : OnMatchStart();
        }
    }
}