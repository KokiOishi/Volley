using System;
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
    public class StandardGameTest
    {
        public StandardGameTest()
        {
        }

        private static void PrepareStandardMatch(MatchRuleStandalone matchRule, out PlayerStandalone playerA,
                                         out PlayerStandalone playerB, out SinglesTeam teamA, out SinglesTeam teamB,
                                         out MatchStandalone<SinglesTeam> match)
        {
            playerA = new PlayerStandalone(Guid.NewGuid(), "Nozomi Tojo", Gender.Female, 159.0, 60, Hands.Right, new Color(1.0f, 1.0f, 0.0f));
            playerB = new PlayerStandalone(Guid.NewGuid(), "Kotori Minami", Gender.Female, 159.0, 54, Hands.Right, new Color(0.0f, 1.0f, 1.0f));
            teamA = new SinglesTeam(playerA, Guid.NewGuid());
            teamB = new SinglesTeam(playerB, Guid.NewGuid());
            var pcf = FifteenBasedPointCounter.CreateFactory(true);
            var gcf = AdvantageSetGameCounter.CreateFactory(pcf, matchRule.GamesPerSet, 2);
            match = new MatchStandalone<SinglesTeam>(teamA, teamB, matchRule, EnumTeams.TeamA, new StandardSetCounter(gcf, matchRule.Sets, EnumTeams.TeamA));
        }

        [Test]
        public void FullPointGameTest()
        {
            PrepareStandardMatch(new MatchRuleStandalone(false, true, 2, 7, true), out PlayerStandalone playerA, out PlayerStandalone playerB,
                         out SinglesTeam teamA, out SinglesTeam teamB, out MatchStandalone<SinglesTeam> match);
            match.StartMatch();
            match.CurrentSet.StartSet();
            match.CurrentSet.CurrentGame.StartGame();
            var game = match.CurrentSet.CurrentGame;
            for (int i = 0; i < 4; i++)
            {
                game.RallyFinished(new PointWinner<SinglesTeam>(PointKinds.Winner, teamA, ShotKind.Stroke, new Receive[] { new Receive(playerA, HandSide.Fore, ShotKind.Stroke) }, playerA, HandSide.Fore));
            }
            Assert.AreNotSame(game, match.CurrentSet.CurrentGame);
        }

        [Test]
        public void AdvantageTest()
        {
            PrepareStandardMatch(new MatchRuleStandalone(false, true, 2, 7, true), out PlayerStandalone playerA, out PlayerStandalone playerB,
                         out SinglesTeam teamA, out SinglesTeam teamB, out MatchStandalone<SinglesTeam> match);
            match.StartMatch();
            match.CurrentSet.StartSet();
            match.CurrentSet.CurrentGame.StartGame();
            var game = match.CurrentSet.CurrentGame;
            for (int i = 0; i < 3; i++)
            {
                game.RallyFinished(new PointWinner<SinglesTeam>(PointKinds.Winner, teamA, ShotKind.Stroke, new Receive[] { new Receive(playerA, HandSide.Fore, ShotKind.Stroke) }, playerA, HandSide.Fore));
                game.RallyFinished(new PointWinner<SinglesTeam>(PointKinds.Winner, teamB, ShotKind.Stroke, new Receive[] { new Receive(playerB, HandSide.Fore, ShotKind.Stroke) }, playerB, HandSide.Fore));
            }
            game.RallyFinished(new PointWinner<SinglesTeam>(PointKinds.Winner, teamA, ShotKind.Stroke, new Receive[] { new Receive(playerA, HandSide.Fore, ShotKind.Stroke) }, playerA, HandSide.Fore));
            Assert.Multiple(() =>
            {
                Assert.AreSame(game, match.CurrentSet.CurrentGame, "Current game must be same.");
                Assert.AreEqual(PointNumber.Fourty.ToShortString(), game.PointB, $"{nameof(game.PointB)} must be {PointNumber.Fourty}!");
                Assert.AreEqual(PointNumber.Advantage.ToShortString(), game.PointA, $"{nameof(game.PointA)} must be {PointNumber.Advantage}!");
            });
        }

        [Test]
        public void AdvantageResurrectionTest()
        {
            PrepareStandardMatch(new MatchRuleStandalone(false, true, 2, 7, true), out PlayerStandalone playerA, out PlayerStandalone playerB,
                         out SinglesTeam teamA, out SinglesTeam teamB, out MatchStandalone<SinglesTeam> match);
            match.StartMatch();
            match.CurrentSet.StartSet();
            match.CurrentSet.CurrentGame.StartGame();
            var game = match.CurrentSet.CurrentGame;
            for (int i = 0; i < 4; i++)
            {
                game.RallyFinished(new PointWinner<SinglesTeam>(PointKinds.Winner, teamA, ShotKind.Stroke, new Receive[] { new Receive(playerA, HandSide.Fore, ShotKind.Stroke) }, playerA, HandSide.Fore));
                game.RallyFinished(new PointWinner<SinglesTeam>(PointKinds.Winner, teamB, ShotKind.Stroke, new Receive[] { new Receive(playerB, HandSide.Fore, ShotKind.Stroke) }, playerB, HandSide.Fore));
            }
            Assert.Multiple(() =>
            {
                Assert.AreSame(game, match.CurrentSet.CurrentGame, "Current game must be same.");
                Assert.AreEqual(PointNumber.Fourty.ToShortString(), game.PointB, $"{nameof(game.PointB)} must be {PointNumber.Fourty}!");
                Assert.AreEqual(PointNumber.Fourty.ToShortString(), game.PointA, $"{nameof(game.PointA)} must be {PointNumber.Fourty}!");
            });
        }
    }
}