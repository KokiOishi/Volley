using System;
using System.Collections.Generic;
using System.Text;
using Volley.Matches.Rules;
using Volley.Team;
using Volley.Games;
using Volley.Sets;
using System.Linq;
using Volley.Matches.Competitors;
using Volley.Rules;

namespace Volley.Matches
{
    /// <summary>
    /// Defines a base infrastructure of tennis match.
    /// </summary>
    public abstract class Match<TTeam> : IMatch<TTeam> where TTeam : class, ITeamInMatch
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="T:Volley.Matches.Match`1"/> class.
        /// </summary>
        /// <param name="teamA">Team a.</param>
        /// <param name="teamB">Team b.</param>
        /// <param name="rule">Rule.</param>
        /// <param name="initialServiceRight">The service right manager.</param>
        /// <param name="setCounter">The set counter.</param>
        protected Match(TTeam teamA, TTeam teamB, MatchRule rule, ISetCounter setCounter)
        {
            //ServiceRight = initialServiceRight ?? throw new ArgumentNullException(nameof(initialServiceRight));
            SetCounter = setCounter ?? throw new ArgumentNullException(nameof(setCounter));
            TeamA = teamA ?? throw new ArgumentNullException(nameof(teamA));
            TeamB = teamB ?? throw new ArgumentNullException(nameof(teamB));
            Rule = rule ?? throw new ArgumentNullException(nameof(rule));
        }

        /// <summary>
        /// Gets the service right.
        /// </summary>
        /// <value>The service right.</value>
        //public ServiceRightManager<TTeam> ServiceRight { get; }

        protected internal ISetCounter SetCounter { get; }

        /// <summary>
        /// Gets the team a.
        /// </summary>
        /// <value>The team a.</value>
        public TTeam TeamA { get; }

        /// <summary>
        /// Gets the set count of a team.
        /// </summary>
        /// <value>The set count a.</value>
        public virtual int SetCountA { get; protected set; }

        /// <summary>
        /// Gets the team b.
        /// </summary>
        /// <value>The team b.</value>
        public TTeam TeamB { get; }

        /// <summary>
        /// Gets the set count of b team.
        /// </summary>
        /// <value>The set count b.</value>
        public virtual int SetCountB { get; protected set; }

        /// <summary>
        /// Gets the winner of the match.
        /// </summary>
        /// <value>The winner.</value>
        public TTeam Winner { get; private set; }

        /// <summary>
        /// Gets the rule of this match.
        /// </summary>
        /// <value>The rule.</value>
        public MatchRule Rule { get; }

        /// <summary>
        /// Gets the list of the sets.
        /// None of the Sets in match are stored in it.
        /// </summary>
        /// <value>The sets.</value>
        public IList<Set<TTeam>> Sets { get; } = new List<Set<TTeam>>();

        /// <summary>
        /// Gets the current set.
        /// </summary>
        /// <value>The current set.</value>
        public Set<TTeam> CurrentSet { get; protected set; }

        /// <summary>
        /// Starts the match.
        /// </summary>
        public void StartMatch()
        {
            CurrentSet = OnMatchStart();
        }

        /// <summary>
        /// Called when the match is starting.
        /// </summary>
        /// <returns>The match start.</returns>
        protected abstract Set<TTeam> OnMatchStart();

        /// <summary>
        /// Adds the set. Call when the <see cref="CurrentSet"/> finished.
        /// </summary>
        public void CurrentSetFinished()
        {
            Set<TTeam> finishingSet = CurrentSet;
            Sets.Add(finishingSet);
            if (finishingSet.Winner == TeamA) SetCounter.IncrementA();
            else SetCounter.IncrementB();
            (SetCountA, SetCountB) = (SetCounter.SetCountA, SetCounter.SetCountB);
            CurrentSet = OnSetEnd(finishingSet);  //外注
            if (SetCounter.IsMatchOver)
            {
                //The match is over
                if (SetCountA == SetCountB) throw new InvalidProgramException("Cannot win the match while in tie!");
                Winner = SetCountA < 0 ? TeamA : TeamB;
                OnMatchOver();
            }
        }

        /// <summary>
        /// Cancels the last set. Call when the last set information was invalid or incorrect.
        /// </summary>
        public ((IEnumerable<Receive>, Game<TTeam>), Set<TTeam>) CancelPreviousSet()
        {
            if (Sets.Count < 1) throw new InvalidOperationException("Cannot rollback from empty state!");
            var s = CurrentSet = Sets.Last();
            Sets.Remove(s);
            SetCounter.Rollback();
            OnSetCanceled(s);
            (SetCountA, SetCountB) = (SetCounter.SetCountA, SetCounter.SetCountB);
            return (s.CancelPreviousGame(), s);
        }

        /// <summary>
        /// Adds the set. Call when the set got finished.
        /// </summary>
        /// <param name="set">Set.</param>
        protected abstract Set<TTeam> OnSetEnd(Set<TTeam> set);

        /// <summary>
        /// Cancels the last set. Call when the last set information was invalid or incorrect.
        /// </summary>
        /// <param name="set">Set.</param>
        protected abstract void OnSetCanceled(Set<TTeam> set);

        /// <summary>
        /// Called when the match is over.
        /// </summary>
        protected abstract void OnMatchOver();
    }
}