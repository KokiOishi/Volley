using System;
using System.Collections.Generic;
using System.Text;
using Volley.Standalone.Players;
using Volley.Standalone.Stats;

namespace Volley.Mobile.Forms
{
    public class StatPlayerModel : PlayerModel
    {
        public IEnumerable<IStatItem<double>> Stats { get; }

        public StatPlayerModel(PlayerStatsPair<double> player) : base(player.Player)
        {
            Stats = player.Stats;
        }
    }
}
