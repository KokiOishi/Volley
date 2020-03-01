using System;
using NUnit.Framework;
using Volley.Players;
using Volley.Pointing;
using Volley.Standalone.Players;
using Volley.Standalone.Teams;

namespace Volley.Standalone.Test.Tests
{
    [TestFixture]
    public class PointStandaloneTest
    {
        private readonly PlayerStandalone playerA;
        private readonly PlayerStandalone playerB;
        private readonly SinglesTeam teamA;
        private readonly SinglesTeam teamB;

        public PointStandaloneTest()
        {
            playerA = new PlayerStandalone(Guid.NewGuid(), "Nozomi Tojo", Gender.Female, 159.0, 60, Hands.Right, new Color(0, 0, 0));
            playerB = new PlayerStandalone(Guid.NewGuid(), "Kotori Minami", Gender.Female, 159.0, 54, Hands.Right, new Color(0, 0, 0));
            teamA = new SinglesTeam(playerA, Guid.NewGuid());
            teamB = new SinglesTeam(playerB, Guid.NewGuid());
        }

        [Test]
        public void ThrowsOnNullTeam()
        {
            Assert.Catch<ArgumentNullException>(() =>
            {
                _ = new PointWinner<SinglesTeam>(PointKinds.Winner, null, ShotKind.Stroke, new Receive[] { new Receive(playerB, HandSide.Fore, ShotKind.Stroke) }, playerB, HandSide.Fore);
            });
        }

        [Test]
        public void ThrowsOnNullWonPlayer()
        {
            Assert.Catch<ArgumentNullException>(() =>
            {
                _ = new PointWinner<SinglesTeam>(PointKinds.Winner, teamA, ShotKind.Stroke, new Receive[] { new Receive(playerA, HandSide.Fore, ShotKind.Stroke) }, null, HandSide.Fore);
            });
        }

        [Test]
        public void ThrowsOnNullRally()
        {
            Assert.Catch<ArgumentNullException>(() =>
            {
                _ = new PointWinner<SinglesTeam>(PointKinds.Winner, teamB, ShotKind.Stroke, null, playerB, HandSide.Fore);
            });
        }

        [Test]
        public void DoesNotThrowOnNonNullRally()
        {
            Assert.DoesNotThrow(() =>
            {
                _ = new PointWinner<SinglesTeam>(PointKinds.Winner, teamB, ShotKind.Stroke, new Receive[] { new Receive(playerB, HandSide.Fore, ShotKind.Stroke) }, playerB, HandSide.Fore);
            });
        }

        [Test]
        public void ThrowsOnInvalidWonPlayer()
        {
            Assert.Catch<ArgumentException>(() =>
            {
                _ = new PointWinner<SinglesTeam>(PointKinds.Winner, teamA, ShotKind.Stroke, new Receive[] { new Receive(playerA, HandSide.Fore, ShotKind.Stroke) }, playerB, HandSide.Fore);
            });
        }

        [Test]
        public void DoesNotThrowOnValidWonPlayer()
        {
            Assert.DoesNotThrow(() =>
            {
                _ = new PointWinner<SinglesTeam>(PointKinds.Winner, teamA, ShotKind.Stroke, new Receive[] { new Receive(playerA, HandSide.Fore, ShotKind.Stroke) }, playerA, HandSide.Fore);
            });
        }

        [Test]
        public void ThrowsOnInvalidErroredPlayer()
        {
            Assert.Catch<ArgumentException>(() =>
            {
                _ = new PointWinner<SinglesTeam>(PointKinds.Winner, teamB, ShotKind.Stroke, new Receive[] { new Receive(playerB, HandSide.Fore, ShotKind.Stroke) }, playerA, HandSide.Fore);
            });
        }

        [Test]
        public void DoesNotThrowOnValidErroredPlayer()
        {
            Assert.DoesNotThrow(() =>
            {
                _ = new PointWinner<SinglesTeam>(PointKinds.Winner, teamB, ShotKind.Stroke, new Receive[] { new Receive(playerB, HandSide.Fore, ShotKind.Stroke) }, playerB, HandSide.Fore);
            });
        }
    }
}