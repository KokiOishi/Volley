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
    public class TieBreakSetGameTest
    {
        private static void PrepareTieBreakGameMatch(MatchRuleStandalone matchRule, out PlayerStandalone playerA,
                                         out PlayerStandalone playerB, out SinglesTeam teamA, out SinglesTeam teamB,
                                         out MatchStandalone<SinglesTeam> match)
        {
            playerA = new PlayerStandalone(Guid.NewGuid(), "Nozomi Tojo", Gender.Female, 159.0, 60, Hands.Right, new Color(1.0f, 1.0f, 0.0f));
            playerB = new PlayerStandalone(Guid.NewGuid(), "Kotori Minami", Gender.Female, 159.0, 54, Hands.Right, new Color(0.0f, 1.0f, 1.0f));
            teamA = new SinglesTeam(playerA, Guid.NewGuid());
            teamB = new SinglesTeam(playerB, Guid.NewGuid());
            var pcf = TieBrakingPointCounter.CreateFactory(7, 2);
            var gcf = AdvantageSetGameCounter.CreateFactory(pcf, matchRule.GamesPerSet, 2);
            match = new MatchStandalone<SinglesTeam>(teamA, teamB, matchRule, EnumTeams.TeamA, new StandardSetCounter(gcf, matchRule.Sets, EnumTeams.TeamA));
        }

        [Test]
        public void FullPointGameTest()
        {
            PrepareTieBreakGameMatch(new MatchRuleStandalone(false, true, 2, 7, true), out PlayerStandalone playerA, out PlayerStandalone playerB,
                         out SinglesTeam teamA, out SinglesTeam teamB, out MatchStandalone<SinglesTeam> match);
            match.StartMatch();
            match.CurrentSet.StartSet();
            match.CurrentSet.CurrentGame.StartGame();
            var game = match.CurrentSet.CurrentGame;
            for (int i = 0; i < 7; i++)
            {
                game.RallyFinished(new PointWinner<SinglesTeam>(PointKinds.Winner, teamA, ShotKind.Stroke, new Receive[] { new Receive(playerA, HandSide.Fore, ShotKind.Stroke) }, playerA, HandSide.Fore));
            }
            Assert.AreNotSame(game, match.CurrentSet.CurrentGame);
        }

        [Test]
        public void AdvantageTest()
        {
            PrepareTieBreakGameMatch(new MatchRuleStandalone(false, true, 2, 7, true), out PlayerStandalone playerA, out PlayerStandalone playerB,
                         out SinglesTeam teamA, out SinglesTeam teamB, out MatchStandalone<SinglesTeam> match);
            match.StartMatch();
            match.CurrentSet.StartSet();
            match.CurrentSet.CurrentGame.StartGame();
            var game = match.CurrentSet.CurrentGame;
            for (int i = 0; i < 6; i++)
            {
                game.RallyFinished(new PointWinner<SinglesTeam>(PointKinds.Winner, teamA, ShotKind.Stroke, new Receive[] { new Receive(playerA, HandSide.Fore, ShotKind.Stroke) }, playerA, HandSide.Fore));
                game.RallyFinished(new PointWinner<SinglesTeam>(PointKinds.Winner, teamB, ShotKind.Stroke, new Receive[] { new Receive(playerB, HandSide.Fore, ShotKind.Stroke) }, playerB, HandSide.Fore));
            }
            game.RallyFinished(new PointWinner<SinglesTeam>(PointKinds.Winner, teamA, ShotKind.Stroke, new Receive[] { new Receive(playerA, HandSide.Fore, ShotKind.Stroke) }, playerA, HandSide.Fore));
            Assert.Multiple(() =>
            {
                Assert.AreSame(game, match.CurrentSet.CurrentGame, "Current game must be same.");
                Assert.AreEqual("6", game.PointB, $"{nameof(game.PointB)} must be {6}!");
                Assert.AreEqual("7", game.PointA, $"{nameof(game.PointA)} must be {7}!");
            });
        }

        [Test]
        public void DeuceTest()
        {
            PrepareTieBreakGameMatch(new MatchRuleStandalone(false, true, 2, 7, true), out PlayerStandalone playerA, out PlayerStandalone playerB,
                         out SinglesTeam teamA, out SinglesTeam teamB, out MatchStandalone<SinglesTeam> match);
            match.StartMatch();
            match.CurrentSet.StartSet();
            match.CurrentSet.CurrentGame.StartGame();
            var game = match.CurrentSet.CurrentGame;
            for (int i = 0; i < 7; i++)
            {
                game.RallyFinished(new PointWinner<SinglesTeam>(PointKinds.Winner, teamA, ShotKind.Stroke, new Receive[] { new Receive(playerA, HandSide.Fore, ShotKind.Stroke) }, playerA, HandSide.Fore));
                game.RallyFinished(new PointWinner<SinglesTeam>(PointKinds.Winner, teamB, ShotKind.Stroke, new Receive[] { new Receive(playerB, HandSide.Fore, ShotKind.Stroke) }, playerB, HandSide.Fore));
            }
            Assert.Multiple(() =>
            {
                Assert.AreSame(game, match.CurrentSet.CurrentGame, "Current game must be same.");
                Assert.AreEqual("7", game.PointB, $"{nameof(game.PointB)} must be {7}!");
                Assert.AreEqual("7", game.PointA, $"{nameof(game.PointA)} must be {7}!");
            });
        }

        [Test]
        public void DeuceOutTest()
        {
            PrepareTieBreakGameMatch(new MatchRuleStandalone(false, true, 2, 7, true), out PlayerStandalone playerA, out PlayerStandalone playerB,
                         out SinglesTeam teamA, out SinglesTeam teamB, out MatchStandalone<SinglesTeam> match);
            match.StartMatch();
            match.CurrentSet.StartSet();
            match.CurrentSet.CurrentGame.StartGame();
            var game = match.CurrentSet.CurrentGame;
            for (int i = 0; i < 6; i++)
            {
                game.RallyFinished(new PointWinner<SinglesTeam>(PointKinds.Winner, teamA, ShotKind.Stroke, new Receive[] { new Receive(playerA, HandSide.Fore, ShotKind.Stroke) }, playerA, HandSide.Fore));
                game.RallyFinished(new PointWinner<SinglesTeam>(PointKinds.Winner, teamB, ShotKind.Stroke, new Receive[] { new Receive(playerB, HandSide.Fore, ShotKind.Stroke) }, playerB, HandSide.Fore));
            }
            game.RallyFinished(new PointWinner<SinglesTeam>(PointKinds.Winner, teamA, ShotKind.Stroke, new Receive[] { new Receive(playerA, HandSide.Fore, ShotKind.Stroke) }, playerA, HandSide.Fore));
            game.RallyFinished(new PointWinner<SinglesTeam>(PointKinds.Winner, teamA, ShotKind.Stroke, new Receive[] { new Receive(playerA, HandSide.Fore, ShotKind.Stroke) }, playerA, HandSide.Fore));
            Assert.Multiple(() =>
            {
                Assert.AreNotSame(game, match.CurrentSet.CurrentGame, "Current game must be same.");
                Assert.AreEqual("6", game.PointB, $"{nameof(game.PointB)} must be 6!");
                Assert.AreEqual("*8", game.PointA, $"{nameof(game.PointA)} must be *8!");
            });
        }
    }
}