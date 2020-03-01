using System;
using System.Collections.Generic;
using System.Text;
using Volley.Rules;

namespace Volley.Mobile.Forms.ViewModels.Parameters
{
    public readonly struct RuleSetupParameters : IEquatable<RuleSetupParameters>
    {
        public readonly MatchSetupParameters RuleParameters;
        public readonly ISetCounter SetCounter;

        public RuleSetupParameters(MatchSetupParameters ruleParameters, ISetCounter setCounter)
        {
            RuleParameters = ruleParameters;
            SetCounter = setCounter ?? throw new ArgumentNullException(nameof(setCounter));
        }

        public override bool Equals(object obj) => obj is RuleSetupParameters parameters && Equals(parameters);

        public bool Equals(RuleSetupParameters other) => RuleParameters.Equals(other.RuleParameters) && EqualityComparer<ISetCounter>.Default.Equals(SetCounter, other.SetCounter);

        public override int GetHashCode()
        {
            var hashCode = -1873195528;
            hashCode = hashCode * -1521134295 + RuleParameters.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<ISetCounter>.Default.GetHashCode(SetCounter);
            return hashCode;
        }

        public static bool operator ==(RuleSetupParameters left, RuleSetupParameters right) => left.Equals(right);

        public static bool operator !=(RuleSetupParameters left, RuleSetupParameters right) => !(left == right);
    }
}
