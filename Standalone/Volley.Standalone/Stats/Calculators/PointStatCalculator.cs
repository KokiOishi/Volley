using System.Collections.Generic;
using System.Linq;
using Volley.Players;
using Volley.Pointing;
using Volley.Standalone.Games;
using Volley.Standalone.Players;
using Volley.Team;

namespace Volley.Standalone.Stats.Calculators
{
    public class PointStatCalculator : IStatCalculator<double>
    {
        private static IEnumerable<(ShotKind kind, HandSide side, string name)> Combinations
        {
            get
            {
                yield return (ShotKind.Stroke, HandSide.Fore, "SF");
                yield return (ShotKind.Volley, HandSide.Fore, "VF");
                yield return (ShotKind.Stroke, HandSide.Back, "SB");
                yield return (ShotKind.Volley, HandSide.Back, "VB");
            }
        }

        public IEnumerable<IStatItem<double>> Calculate<TTeam>(Rally<TTeam> gameManager, PlayerStandalone player) where TTeam : class, ITeamInMatch
        {
            var positive = gameManager.AllPoints.OfType<PointWinner<TTeam>>().Where(a => a.ActualWinner == player);
            var errors = gameManager.AllPoints.OfType<PointError<TTeam>>().Where(a => a.ErroredPlayer == player);
            var doubleFaults = gameManager.AllPoints.OfType<PointServiceDoubleFault<TTeam>>().Where(a => a.ErroredPlayer == player);
            var negative = errors.OfType<Point<TTeam>>().Concat(doubleFaults.OfType<Point<TTeam>>());
            var all = positive.OfType<Point<TTeam>>().Concat(negative);
            double divisor = 1.0 / all.Count();
            //Ace
            var aces = positive.Where(a => !a.Receives.Any(p => p.ReceivedPlayer != player));
            int aceC = aces.Count();
            var aceR = aceC * divisor;
            yield return new SimpleStatItem<double>("Ace", $"{aceC} ({aceR:P})", aceR);
            //Double fault
            int dfC = doubleFaults.Count();
            var dfR = dfC * divisor;
            yield return new SimpleStatItem<double>("Double fault", $"{dfC} ({dfR:P})", dfR);
            //Ret.Ace
            {
                var retAce = positive.Where(a => a.Receives.Count(p => p.ReceivedPlayer != player && !p.IsServiceFault) == 1);
                int raC = retAce.Count();
                var raR = raC * divisor;
                yield return new SimpleStatItem<double>("Winner", $"{raC} ({raR:P})", raR);
                foreach (var (kind, side, name) in Combinations)
                {
                    var comb = retAce.Where(a => a.ShotKind == kind && a.Side == side);
                    int raCC = comb.Count();
                    var raRC = raCC * divisor;
                    yield return new SimpleStatItem<double>($"Winner({name})", $"{raCC} ({raRC:P})", raRC);
                }
            }
            //Winner
            {
                int wC = positive.Count();
                var wR = wC * divisor;
                yield return new SimpleStatItem<double>("Winner", $"{wC} ({wR:P})", wR);
                foreach (var (kind, side, name) in Combinations)
                {
                    var comb = positive.Where(a => a.ShotKind == kind && a.Side == side);
                    int wCC = comb.Count();
                    var wRC = wCC * divisor;
                    yield return new SimpleStatItem<double>($"Winner({name})", $"{wCC} ({wRC:P})", wRC);
                }
                if (wC > 0)
                {
                    var wCRally = positive.Average(a => (double)a.Receives.Count(p => !p.IsServiceFault));
                    yield return new SimpleStatItem<double>($"Ave. shots of Winner", $"{wCRally}", wCRally);
                }
            }
            //Error
            {
                var forcedErr = errors.Where(a => a.Forced);
                int feC = forcedErr.Count();
                var feR = feC * divisor;
                yield return new SimpleStatItem<double>("Winner", $"{feC} ({feR:P})", feR);
                foreach (var (kind, side, name) in Combinations)
                {
                    var comb = forcedErr.Where(a => a.ShotKind == kind && a.Side == side);
                    int feCC = comb.Count();
                    var feRC = feCC * divisor;
                    yield return new SimpleStatItem<double>($"Winner({name})", $"{feCC} ({feRC:P})", feRC);
                }
                if (feC > 0)
                {
                    var feCRally = forcedErr.Average(a => (double)a.Receives.Count(p => !p.IsServiceFault));
                    yield return new SimpleStatItem<double>($"Ave. shots of Error", $"{feCRally}", feCRally);
                }
            }
            //U.Error
            {
                var unforcedErr = errors.Where(a => !a.Forced);
                int ueC = unforcedErr.Count();
                var ueR = ueC * divisor;
                yield return new SimpleStatItem<double>("Winner", $"{ueC} ({ueR:P})", ueR);
                foreach (var (kind, side, name) in Combinations)
                {
                    var comb = unforcedErr.Where(a => a.ShotKind == kind && a.Side == side);
                    int ueCC = comb.Count();
                    var ueRC = ueCC * divisor;
                    yield return new SimpleStatItem<double>($"Winner({name})", $"{ueCC} ({ueRC:P})", ueRC);
                }

                if (ueC > 0)
                {
                    var ueCRally = unforcedErr.Average(a => (double)a.Receives.Count(p => !p.IsServiceFault));
                    yield return new SimpleStatItem<double>($"Ave. shots of U.Error", $"{ueCRally}", ueCRally);
                }
            }
        }
    }
}
