using System;
using NUnit.Framework;
using Volley.Standalone.Matches;

namespace Volley.Standalone.Test.Tests
{
    [TestFixture]
    public class MatchRuleStandaloneTest
    {
        public MatchRuleStandaloneTest()
        {
        }
        [Test]
        public void ThrowsOnNegativeSets()
        {
            Assert.Catch<ArgumentOutOfRangeException>(() =>
            {
                _ = new MatchRuleStandalone(true, true, -1, 1, true);
            });
        }

        [Test]
        public void ThrowsOnZeroSets()
        {
            Assert.Catch<ArgumentOutOfRangeException>(() =>
            {
                _ = new MatchRuleStandalone(true, true, 0, 1, true);
            });
        }

        [Test]
        public void DoesNotThrowOnPositiveSets()
        {
            Assert.DoesNotThrow(() =>
            {
                _ = new MatchRuleStandalone(true, true, 1, 1, true);
            });
        }

        [Test]
        public void ThrowsOnNegativeGames()
        {
            Assert.Catch<ArgumentOutOfRangeException>(() =>
            {
                _ = new MatchRuleStandalone(true, true, 1, -1, true);
            });
        }

        [Test]
        public void ThrowsOnZeroGames()
        {
            Assert.Catch<ArgumentOutOfRangeException>(() =>
            {
                _ = new MatchRuleStandalone(true, true, 1, 0, true);
            });
        }

        [Test]
        public void DoesNotThrowOnPositiveGames()
        {
            Assert.DoesNotThrow(() =>
            {
                _ = new MatchRuleStandalone(true, true, 1, 1, true);
            });
        }
    }
}
