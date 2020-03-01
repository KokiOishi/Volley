using System;
using System.Collections.Generic;
using System.Text;
using Volley.Standalone.Games;
using Volley.Standalone.Players;
using Volley.Team;

namespace Volley.Standalone.Stats
{
    public interface IStatCalculator<out TValue>
    {
        IEnumerable<IStatItem<TValue>> Calculate<TTeam>(Rally<TTeam> gameManager, PlayerStandalone player) where TTeam : class, ITeamInMatch;
    }
}
