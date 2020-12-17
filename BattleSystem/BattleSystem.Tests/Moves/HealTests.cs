﻿using BattleSystem.Moves;
using NUnit.Framework;
using NUnit.Framework.Constraints;

namespace BattleSystem.Tests.Moves
{
    /// <summary>
    /// Unit tests for <see cref="Heal"/>.
    /// </summary>
    [TestFixture]
    public class HealTests
    {
        [TestCase(5, true)]
        [TestCase(1, true)]
        [TestCase(0, false)]
        [TestCase(-5, false)]
        public void CanUse_ReturnsCorrectly(int remainingUses, bool expectedCanUse)
        {
            // Arrange
            var heal = new Heal("yeti", remainingUses, 1, HealingMode.Absolute);
            Constraint constraint = expectedCanUse ? Is.True : Is.False;

            // Act and Assert
            Assert.That(heal.CanUse(), constraint);
        }

        [Test]
        public void Use_ReducesRemainingUses()
        {
            // Arrange
            var heal = new Heal("yeti", 2, 1, HealingMode.Absolute);

            // Act
            heal.Use(TestHelpers.CreateBasicCharacter(), TestHelpers.CreateBasicCharacter());

            // Assert
            Assert.That(heal.RemainingUses, Is.EqualTo(1));
        }
    }
}
