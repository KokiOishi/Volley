using System;
using System.Numerics;
using NUnit.Framework;
using Volley.Matches;
using Volley.Players;
using Volley.Pointing;
using Volley.Standalone.Matches;
using Volley.Standalone.Players;
using Volley.Standalone.Teams;

namespace Volley.Standalone.Test.Tests
{
    [TestFixture]
    public class PlayerStandaloneTest
    {
        public PlayerStandaloneTest()
        {
        }

        [Test]
        public void ThrowsOnNullName()
        {
            Assert.Catch<ArgumentNullException>(() =>
            {
                _ = new PlayerStandalone(Guid.NewGuid(), null, Gender.Female, 159.0, 60, Hands.Right, new Color(Vector3.Zero));
            });
        }

        [Test]
        public void ThrowsOnEmptyName()
        {
            Assert.Catch<ArgumentNullException>(() =>
            {
                _ = new PlayerStandalone(Guid.NewGuid(), "", Gender.Female, 159.0, 60, Hands.Right, new Color(Vector3.Zero));
            });
        }

        [Test]
        public void ThrowsOnNegativeHeight()
        {
            Assert.Catch<ArgumentOutOfRangeException>(() =>
            {
                _ = new PlayerStandalone(Guid.NewGuid(), "Nozomi Tojo", Gender.Female, -159.0, 60, Hands.Right, new Color(Vector3.Zero));
            });
        }

        [Test]
        public void ThrowsOnInfinityHeight()
        {
            Assert.Catch<ArgumentOutOfRangeException>(() =>
            {
                _ = new PlayerStandalone(Guid.NewGuid(), "Nozomi Tojo", Gender.Female, double.PositiveInfinity, 60, Hands.Right, new Color(Vector3.Zero));
            });
        }

        [Test]
        public void ThrowsOnNaNHeight()
        {
            Assert.Catch<ArgumentOutOfRangeException>(() =>
            {
                _ = new PlayerStandalone(Guid.NewGuid(), "Nozomi Tojo", Gender.Female, double.NaN, 60, Hands.Right, new Color(Vector3.Zero));
            });
        }

        [Test]
        public void ThrowsOnNegativeWeight()
        {
            Assert.Catch<ArgumentOutOfRangeException>(() =>
            {
                _ = new PlayerStandalone(Guid.NewGuid(), "Nozomi Tojo", Gender.Female, 159.0, -60, Hands.Right, new Color(Vector3.Zero));
            });
        }

        [Test]
        public void ThrowsOnInfinityWeight()
        {
            Assert.Catch<ArgumentOutOfRangeException>(() =>
            {
                _ = new PlayerStandalone(Guid.NewGuid(), "Nozomi Tojo", Gender.Female, 159.0, double.PositiveInfinity, Hands.Right, new Color(Vector3.Zero));
            });
        }

        [Test]
        public void ThrowsOnNaNWeight()
        {
            Assert.Catch<ArgumentOutOfRangeException>(() =>
            {
                _ = new PlayerStandalone(Guid.NewGuid(), "Nozomi Tojo", Gender.Female, 159.0, double.NaN, Hands.Right, new Color(Vector3.Zero));
            });
        }
    }
}