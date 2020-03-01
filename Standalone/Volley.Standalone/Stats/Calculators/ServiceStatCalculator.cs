using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Volley.Standalone.Games;
using Volley.Standalone.Players;
using Volley.Team;

namespace Volley.Standalone.Stats.Calculators
{
    public class ServiceStatCalculator : IStatCalculator<double>
    {
        public IEnumerable<IStatItem<double>> Calculate<TTeam>(Rally<TTeam> gameManager, PlayerStandalone player) where TTeam : class, ITeamInMatch
        {
            var services = gameManager.AllPoints.SelectMany(a => a.Receives.TakeWhile(p => p.ReceivedPlayer == player));
            var sFaults = services.Where(a => a.IsServiceFault);
            var cSum = services.Count();
            var cFaults = sFaults.Count();
            var failRate = (double)cFaults / cSum;
            yield return new SimpleStatItem<double>("Service Faults", $"{cFaults} ({failRate:P})", failRate);
        }
    }
}
