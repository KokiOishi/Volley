using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using Volley.Matches;
using Volley.Players;
using Volley.Pointing;
using Volley.Rules;
using Volley.Standalone.Matches;
using Volley.Standalone.Players;
using Volley.Standalone.Teams;

namespace Volley.Standalone.Test.Tests
{
    [TestFixture]
    public sealed class MatchTieBreakGameTest
    {
        private static void PrepareMatchTieBreakGameMatch(MatchRuleStandalone matchRule, out PlayerStandalone playerA,
                                         out PlayerStandalone playerB, out SinglesTeam teamA, out SinglesTeam teamB,
                                         out MatchStandalone<SinglesTeam> match)
        {
            playerA = new PlayerStandalone(Guid.NewGuid(), "Nozomi Tojo", Gender.Female, 159.0, 60, Hands.Right, new Color(1.0f, 1.0f, 0.0f));
            playerB = new PlayerStandalone(Guid.NewGuid(), "Kotori Minami", Gender.Female, 159.0, 54, Hands.Right, new Color(0.0f, 1.0f, 1.0f));
            teamA = new SinglesTeam(playerA, Guid.NewGuid());
            teamB = new SinglesTeam(playerB, Guid.NewGuid());
            var pcf = TieBrakingPointCounter.CreateFactory(2, 2);
            var gcf = AdvantageSetGameCounter.CreateFactory(pcf, matchRule.GamesPerSet, 2);
            match = new MatchStandalone<SinglesTeam>(teamA, teamB, matchRule, EnumTeams.TeamA, new MatchTieBreakSetCounter(gcf, matchRule.Sets * 2 - 1, 7, 2, EnumTeams.TeamA));
        }

        [Test]
        public void FullPointMatchTest()
        {
            PrepareMatchTieBreakGameMatch(new MatchRuleStandalone(false, true, 2, 2, true), out PlayerStandalone playerA, out PlayerStandalone playerB,
                         out SinglesTeam teamA, out SinglesTeam teamB, out MatchStandalone<SinglesTeam> match);
            match.StartMatch();
            match.CurrentSet.StartSet();
            match.CurrentSet.CurrentGame.StartGame();
            for (int i = 0; i < 8; i++)
            {
                match.CurrentSet.CurrentGame.RallyFinished(
                    new PointWinner<SinglesTeam>(PointKinds.Winner, teamA, ShotKind.Stroke, new Receive[] { new Receive(playerA, HandSide.Fore, ShotKind.Stroke) }, playerA, HandSide.Fore));
            }
            Assert.Multiple(() =>
            {
                Assert.Less(match.SetCountA, 0);
                Assert.AreEqual(0, match.SetCountB);
            });
        }
    }
}